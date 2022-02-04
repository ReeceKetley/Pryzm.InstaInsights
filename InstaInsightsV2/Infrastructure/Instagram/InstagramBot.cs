using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using InstaInsightsV2.Domain.Models;
using InstaInsightsV2.Helpers;
using InstaInsightsV2.Properties;
using NeoSmart.Unicode;
using Newtonsoft.Json.Linq;
using SelectPdf;

namespace InstaInsightsV2.Infrastructure.Instagram
{
    public class InstagramBot
    {
        private readonly string _username;
        private HttpClient _client = new HttpClient();
        private string _queryHash = "";
        private Dictionary<string, string> _tokenDictionary;

        public InstagramBot(string username, bool getAll)
        {
            GetAll = getAll;
            _username = username;
        }

        public BindingList<InstaObject> Posts { get; } = new BindingList<InstaObject>();

        public void GetPosts()
        {
            var initialise = Initialise();
            if (!initialise)
            {
                MessageBox.Show("Failled to initlise", "Warning");
                return;
            }
            while (_tokenDictionary["nextPage"] == "True")
            {
                var post = BuildPostInfo();
                var gis = BuildGis(post);
                var posts = GetNextPage(post);
                foreach (var instaObject in posts.Posts)
                {
                    Posts.Add(instaObject);
                }
            }

            var metrics = RunMetrics(Posts);
            if (metrics.SevenDaysList == null)
            {
                MessageBox.Show("No posts within the last month.");
                return;
            }
            BuildReport(metrics);
        }

        private void BuildReport(Report metrics)
        {
            if (metrics.SevenDaysList.Count == 0)
            {
                return;
            }
            var popularWords = "";
            foreach (var metricsPopularCaption in metrics.PopularCaptions)
            {
                popularWords = popularWords + " " + metricsPopularCaption;
            }
            var baseHtml = Resources.HtmlSource;
            baseHtml = baseHtml.Replace("[date]", DateTime.Now.ToLongDateString() + " - " + DateTime.Now.ToShortTimeString() + "<br><center><h4>" + _username + "</h4></center>");
            baseHtml = baseHtml.Replace("[emojis]", metrics.EmojisUsedWeek);
            baseHtml = baseHtml.Replace("[popularWords]", popularWords);
            baseHtml = baseHtml.Replace("[avgPostTime]", metrics.AveragePostWeek.ToShortTimeString());
            baseHtml = baseHtml.Replace("[biMonth]", metrics.BestDayMonth.TimeStamp.ToDateTime().ToString("dddd"));
            baseHtml = baseHtml.Replace("[biWeek]", metrics.BestDayWeek.TimeStamp.ToDateTime().ToString("dddd"));
            baseHtml = baseHtml.Replace("[wiMonth]", metrics.WorstInteractionMonth.TimeStamp.ToDateTime().ToString("dddd"));
            baseHtml = baseHtml.Replace("[wiWeek]", metrics.WorstInteractionWeek.TimeStamp.ToDateTime().ToString("dddd"));
            baseHtml = baseHtml.Replace("[weekend]", metrics.BestWeekendPost.TimeStamp.ToDateTime().ToString("dddd"));
            baseHtml = baseHtml.Replace("[weekday]", metrics.BestDayWeek.TimeStamp.ToDateTime().ToString("dddd"));
            baseHtml = baseHtml.Replace("[likes]", metrics.MostLikes.TimeStamp.ToDateTime().ToString("dddd"));
            baseHtml = baseHtml.Replace("[comments]", metrics.MostComments.TimeStamp.ToDateTime().ToString("dddd"));

            string tableData = "";

            var days = metrics.SevenDaysList.OrderBy(x => x.TimeStamp.ToDateTime().DayOfWeek);

            foreach (var instaObject in days)
            {
                var tableHtml = Resources.TableData;
                tableHtml = tableHtml.Replace("[weekday]", instaObject.TimeStamp.ToDateTime().DayOfWeek.ToString());


                tableHtml = tableHtml.Replace("[week]", "Week " + StringExtensions.GetWeekOfMonth(instaObject.TimeStamp.ToDateTime()));
                var type = "Image";
                if (instaObject.Views == "null")
                {
                    type = "Video";
                }

                tableHtml = tableHtml.Replace("[posttype]", type);
                tableHtml = tableHtml.Replace("[posttime]", instaObject.TimeStamp.ToDateTime().ToShortDateString() + " - " + instaObject.TimeStamp.ToDateTime().ToShortTimeString());
                tableHtml = tableHtml.Replace("[likes]", instaObject.Likes);
                tableHtml = tableHtml.Replace("[comments]", instaObject.Comments);
                tableHtml = tableHtml.Replace("[link]", "<a href=\"" + instaObject.Link + "\">" + instaObject.Link.Split('/')[instaObject.Link.Split('/').Length - 1] + "</a>");
                tableData = tableData + tableHtml;
            }

            baseHtml = baseHtml.Replace("[tabledata]", tableData);
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Reports", DateTime.Now.ToString("M"));
                Directory.CreateDirectory(path);
                var OutputPath = path.ToString() + "\\" + _username + ".pdf";
                HtmlToPdf converter = new HtmlToPdf();

                // convert the url to pdf 
                PdfDocument doc = converter.ConvertHtmlString(baseHtml);

                // save pdf document 
                doc.Save(OutputPath);

                // close pdf document 
                doc.Close();
                File.WriteAllText(OutputPath.Replace(".pdf", ".html"), baseHtml);
                System.Diagnostics.Process.Start(OutputPath);
                System.Diagnostics.Process.Start(path);
            }
            catch
            {
                MessageBox.Show("Failed to save check application permisons", "Error");
            }
        }

        private string GetEmojis(List<InstaObject> instaObjects)
        {
            string emojis = "";
            foreach (var isntaObject in instaObjects)
            {
                foreach (var singleEmoji in Emoji.All)
                {
                    if (isntaObject.Caption.Contains(singleEmoji.Sequence.AsString))
                    {
                        if (emojis.Contains(singleEmoji.Sequence.AsString))
                        {
                            continue;
                        }
                        else
                        {
                            emojis = emojis + " " + singleEmoji.Sequence.AsString;
                        }
                    }

                }
            }

            return emojis;
        }

        private List<string> GetWords(List<InstaObject> instaObjects)
        {
            Dictionary<string, int> popularWords = new Dictionary<string, int>();
            foreach (var instaObject in instaObjects)
            {
                var cleanedString = instaObject.Caption;
                foreach (var singleEmoji in Emoji.All)
                {
                    cleanedString = cleanedString.Replace(singleEmoji.Sequence.AsString, " ");
                }

                var words = cleanedString.Split(' ');
                foreach (var word in words)
                {
                    if (word.Length < 4)
                    {
                        continue;
                    }

                    var ignore = false;
                    if (!File.Exists("filterwords.txt"))
                    {
                        File.WriteAllText("filterwords.txt", "");
                    }
                    foreach (var readAllLine in File.ReadAllLines("filterwords.txt"))
                    {
                        if (word.ToLower().Contains(readAllLine.ToLower()))
                        {
                            ignore = true;
                        }
                    }

                    if (ignore)
                    {
                        continue;
                    }

                    if (popularWords.ContainsKey(word.ToLower()))
                    {
                        ++popularWords[word.ToLower()];
                    }
                    else
                    {
                        popularWords.Add(word.ToLower(), 1);
                    }
                }
            }

            var list = popularWords.ToList();
            list.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            if (list.Count >= 5)
            {
                return list.GetRange(0, 5).Select(x => x.Key).ToList();
            }
            else
            {
                return list.Select(x => x.Key).ToList();
            }
        }

        private Report RunMetrics(BindingList<InstaObject> posts)
        {
            var dictionary = GetDateObjects(posts);

            List<InstaObject> metrics = new List<InstaObject>(dictionary.Values);
            if (metrics.Count <= 0)
            {
                return new Report(metrics);
            }

            var bestPostMonth = posts.MaxObject(x => Convert.ToInt32(x.Likes));
            var worstPostMonth = posts.MinBy(x => Convert.ToInt32(x.Likes));
            var maxHour = posts.Max(x => x.TimeStamp.ToDateTime().Hour);
            List<InstaObject> weekendObjects = new List<InstaObject>();
            foreach (var instaObject in metrics.ToArray())
            {
                if (instaObject == null)
                {
                    metrics.Remove(instaObject);
                    continue;
                }

                if (instaObject.TimeStamp.ToDateTime().DayOfWeek == DayOfWeek.Saturday)
                {
                    weekendObjects.Add(instaObject);
                }

                if (instaObject.TimeStamp.ToDateTime().DayOfWeek == DayOfWeek.Sunday)
                {
                    weekendObjects.Add(instaObject);
                }
            }

            if (metrics.Count <= 0)
            {
                return new Report(null);
            }

            var bestDayWeek = metrics.MaxObject(x => Convert.ToInt32(x.Likes));
            var bestDayMonth = posts.MaxObject(x => Convert.ToInt32(x.Likes));
            var bestWeekendPost = new InstaObject(947289600);
            if (weekendObjects.Count > 0)
            {
                bestWeekendPost = weekendObjects.MaxObject(x => Convert.ToInt32(x.Likes));
            }

            var mostLikesArray = posts.MaxObject(x => Convert.ToInt32(x.Likes));
            var mostLikes = mostLikesArray;
            var worstDayWeek = metrics.MinBy(x => Convert.ToInt32(x.Likes));
            var mostComments = posts.MaxObject(x => Convert.ToInt32(x.Comments));

            var dates = new List<DateTime>();
            var days = new List<DayOfWeek>();
            foreach (var instaObject in metrics)
            {
                if (instaObject == null)
                {
                    continue;
                }

                var postTime = instaObject.TimeStamp.ToDateTime();
                var postDay = instaObject.TimeStamp.ToDateTime().DayOfWeek;
                days.Add(postDay);
                var time = new DateTime(1, 1, 1, postTime.Hour, postTime.Minute, postTime.Second);
                dates.Add(time);
            }
            var averageTime = dates.Average();
            if (!days.Contains(DayOfWeek.Monday))
            {
                metrics.Add(new InstaObject(946857600));
            }
            if (!days.Contains(DayOfWeek.Tuesday))
            {
                metrics.Add(new InstaObject(946944000));
            }
            if (!days.Contains(DayOfWeek.Wednesday))
            {
                metrics.Add(new InstaObject(947030400));
            }
            if (!days.Contains(DayOfWeek.Thursday))
            {
                metrics.Add(new InstaObject(947116800));
            }
            if (!days.Contains(DayOfWeek.Friday))
            {
                metrics.Add(new InstaObject(947203200));
            }
            if (!days.Contains(DayOfWeek.Saturday))
            {
                metrics.Add(new InstaObject(947289600));
            }
            if (!days.Contains(DayOfWeek.Sunday))
            {
                metrics.Add(new InstaObject(947376000));
            }


            return new Report(metrics, bestDayMonth, bestDayWeek, worstPostMonth, worstDayWeek, bestWeekendPost, mostLikes, mostComments, averageTime, GetWords(metrics), GetEmojis(metrics));
        }

        private Dictionary<DayOfWeek, InstaObject> GetDateObjects(BindingList<InstaObject> posts)
        {
            var dateNow = DateTime.Now.AddMonths(-1);
            var postList = new List<InstaObject>();
            foreach (var item in posts)
            {
                if (item.TimeStamp.ToDateTime() < dateNow)
                {
                    continue;
                }
                postList.Add(item);
            }
            var results = new Dictionary<DayOfWeek, InstaObject>();
            foreach (var o in postList)
            {
                var dayOfWeek = o.TimeStamp.ToDateTime().DayOfWeek;
                var @where = postList.Where(x => x.TimeStamp.ToDateTime().DayOfWeek == dayOfWeek);
                if (results.ContainsKey(dayOfWeek))
                {
                    continue;
                }

                InstaObject bestPost = null;
                double bestInteractionsPerMin = 0;
                foreach (var post in @where)
                {
                    int totalInteractions = Convert.ToInt32(post.Likes);
                    var i = post.TimeStamp.ToDateTime().Hour * 60 + post.TimeStamp.ToDateTime().Minute;
                    i = 1440 - i;
                    double interactionsPerMin = (double)totalInteractions / (double)i;
                    if (dayOfWeek == DayOfWeek.Friday)
                    {
                        Console.WriteLine(interactionsPerMin);
                    }

                    if (bestPost == null)
                    {
                        bestPost = post;
                        bestInteractionsPerMin = interactionsPerMin;
                        continue;
                    }

                    if (interactionsPerMin > bestInteractionsPerMin)
                    {
                        bestPost = post;
                        bestInteractionsPerMin = interactionsPerMin;
                    }
                }

                results[dayOfWeek] = bestPost;
            }
#if DEBUG
            Console.WriteLine(results.Count);
#endif

            return results;
        }

        private string BuildGis(string json)
        {
            var gis = _tokenDictionary["rhxGis"] + ":" + json;
            var md5 = StringExtensions.GetMd5Hash(gis);
            _client.DefaultRequestHeaders.Remove("X-Instagram-GIS");
            _client.DefaultRequestHeaders.Add("X-Instagram-GIS", md5);
            return md5;
        }

        private bool Initialise()
        {
           try
            {
                _client.DefaultRequestHeaders.Add("User-Agent", " Mozilla/5.0 (iPhone; CPU iPhone OS 5_1_1 like Mac OS X) AppleWebKit/534.46 (KHTML, like Gecko) Version/5.1 Mobile/9B206 Safari/7534.48.3");
                var result = _client.GetAsync("https://www.instagram.com/" + _username);
                if (result.Result.StatusCode != HttpStatusCode.OK)
                {
                    return false;
                }

#if DEBUG
                MessageBox.Show(result.Result.ReasonPhrase);
#endif

                var s = result.Result.Content.ReadAsStringAsync().Result;
                var rawJson = s.Substring("{\"config", ";</script>");
                rawJson = "{\"config" + rawJson;
                ExtractTokens(s, rawJson);
                GetInitialList(rawJson);
                return true;
            }
            catch(Exception e)
            {
                throw e;
                MessageBox.Show("Failed to grab profile. Check username.", "Warning");
                return false;
            }
        }

        private void ExtractTokens(string result, string rawJson)
        {
            var profileContainer = result.Substring("/static/bundles/metro/ProfilePageContainer.js/", ".js");
            var profileContainerUrl = "https://www.instagram.com/static/bundles/metro/ProfilePageContainer.js/" + profileContainer + ".js";
            Console.WriteLine(profileContainerUrl);
            var profileJs = _client.GetStringAsync(profileContainerUrl).Result;
            var queryHash = profileJs.Substring(",queryId:\"", "\",");
            Console.WriteLine(queryHash);
            _queryHash = "e769aa130647d2354c40ea6a439bfc08";
            var obj = InstaObject.GetInitialPaginationInfo(rawJson);
            _tokenDictionary = obj;
            BuildPostInfo();
        }

        private PaginationResult GetNextPage(string post)
        {
            // Delay to prevent throttling.
            Thread.Sleep(250);
            var response = _client.GetAsync("https://www.instagram.com/graphql/query/?query_hash=" + _queryHash + "&variables=" + post);
            if (!response.Result.IsSuccessStatusCode)
            {
                Console.WriteLine("Bad pagination info.");
                return null;
            }

            var result = response.Result.Content.ReadAsStringAsync().Result;
            JObject jObject = null;
            try
            {
                jObject = JObject.Parse(result);
            }
            catch
            {
                MessageBox.Show("JSON Decode failure check for pagaination issues.");
            }

            var media = jObject["data"]["user"]["edge_owner_to_timeline_media"];
            var nextPage = media["page_info"]["has_next_page"].Value<bool>();
            var endCursor = media["page_info"]["end_cursor"]?.Value<string>();
            if (nextPage)
            {
                _tokenDictionary["endCursor"] = endCursor;
                _tokenDictionary["nextPage"] = "True";
            }
            else
            {
                _tokenDictionary["nextPage"] = "False";
            }

            var postsList = new BindingList<InstaObject>();
            var posts = media["edges"]?.Value<JArray>();
            if (posts != null)
            {
                foreach (var jToken in posts)
                {
                    string location = "Unknown";
                    try
                    {
                        var caption = jToken["node"]["edge_media_to_caption"]["edges"][0]["node"]["text"]?.Value<string>();
                        var link = "https://www.instagram.com/p/" + jToken["node"]["shortcode"]?.Value<string>();
                        var likes = jToken["node"]["edge_media_preview_like"]["count"]?.Value<string>();
                        var comments = jToken["node"]["edge_media_to_comment"]["count"]?.Value<string>();
                        var time = StringExtensions.UnixTimeStampToDateTime(Convert.ToDouble(jToken["node"]["taken_at_timestamp"].Value<string>())).ToLongDateString() + " - " + StringExtensions.UnixTimeStampToDateTime(Convert.ToDouble(jToken["node"]["taken_at_timestamp"].Value<string>())).ToShortTimeString();
                        var timeStamp = Convert.ToDouble(jToken["node"]["taken_at_timestamp"].Value<string>());
                        var dateNow = DateTime.Now.AddMonths(-1);
                        var postDate = StringExtensions.UnixTimeStampToDateTime(Convert.ToDouble(jToken["node"]["taken_at_timestamp"].Value<string>()));
                        if (!GetAll)
                        {
                            if (postDate < dateNow)
                            {
                                _tokenDictionary["nextPage"] = "False";
                                break;
                            }
                        }

                        try
                        {
                            location = jToken["node"]["location"]["name"].Value<string>();
                        }
                        catch
                        {
                            Console.WriteLine("Location issues");
                        }

                        var views = "null";
                        try
                        {
                            views = jToken["node"]["video_view_count"].Value<string>();
                        }
                        catch
                        {
                           Console.WriteLine("Item had no views or was not a video post.");
                        }


                        var instaObj = new InstaObject(caption, link, time, location, comments, likes, views, timeStamp);
                        postsList.Add(instaObj);
                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine(e.Message);
                    }
                }
            }

            return new PaginationResult(postsList, nextPage, endCursor);
        }

        public bool GetAll { get; private set; }

        private string BuildPostInfo()
        {
            var obj = new JObject();
            obj.Add("id", _tokenDictionary["id"]);
            obj.Add("first", 50);
            obj.Add("after", _tokenDictionary["endCursor"]);
            BuildGis(obj.ToString());
            return obj.ToString();
        }

        private void GetInitialList(string json)
        {
            var instaObjects = InstaObject.GetInitalList(json, GetAll);
            foreach (var instaObject in instaObjects)
            {
                Posts.Add(instaObject);
            }
        }
    }

    internal class PaginationResult
    {
        public PaginationResult(BindingList<InstaObject> posts, bool hasNextPage, string endToken = "")
        {
            Posts = posts;
            HasNextPage = hasNextPage;
            EndToken = endToken;
        }

        public BindingList<InstaObject> Posts { get; }
        public bool HasNextPage { get; }
        public string EndToken { get; }
    }
}