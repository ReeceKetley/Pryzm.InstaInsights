using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using InstaInsightsV2.Annotations;
using InstaInsightsV2.Helpers;
using Newtonsoft.Json.Linq;

namespace InstaInsightsV2.Domain.Models
{
    public class InstaObject : INotifyPropertyChanged
    {
        public InstaObject(double timeStamp)
        {
            TimeStamp = timeStamp;
            Caption = "";
            Link = "";
            Uploaded = "";
            Location = "";
            Comments = "";
            Likes = "";
            Views = "";
        }

        public string Caption { get; }
        public string Link { get; }
        public string Uploaded { get; }
        public string Location { get; }
        public string Comments { get; }
        public string Likes { get; }
        public string Views { get; }
        public double TimeStamp { get; }

        public InstaObject(string caption, string link, string uploaded, string location, string comments, string likes, string views, double timeStamp)
        {
            Caption = caption;
            Link = link;
            Uploaded = uploaded;
            Location = location;
            Comments = comments;
            Likes = likes;
            Views = views;
            TimeStamp = timeStamp;
        }

        public static Dictionary<string, string> GetInitialPaginationInfo(string jsonData)
        {
            JObject jObject = null;
            try
            {
                jObject = JObject.Parse(jsonData);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
            }

            var user = jObject["entry_data"]["ProfilePage"][0]["graphql"]["user"];
            var nextPage = user["edge_owner_to_timeline_media"]["page_info"]["has_next_page"].Value<bool>().ToString();
            var id = user["id"].Value<string>();
            var endCursor = user["edge_owner_to_timeline_media"]["page_info"]["end_cursor"].Value<string>();
            if (string.IsNullOrEmpty(endCursor))
            {
                endCursor = "";
            }

            Console.WriteLine(jsonData);

            var rhxGis = "f2405b236d85e8296cf30347c9f08c2a";
            var dict = new Dictionary<string, string>();
            dict.Add("id", id);
            dict.Add("nextPage", nextPage);
            dict.Add("endCursor", endCursor);
            dict.Add("rhxGis", rhxGis);
            return dict;
        }

        public static BindingList<InstaObject>GetInitalList(string jsonData, bool getAll)
        {
            var dataList = new BindingList<InstaObject>();
            var jObject = JObject.Parse(jsonData);
            var user = jObject["entry_data"]["ProfilePage"][0]["graphql"]["user"];
            var obj = user["edge_owner_to_timeline_media"]["edges"];
            foreach (var token in obj)
            {
                string caption = token["node"]["edge_media_to_caption"]["edges"][0]["node"]["text"]?.Value<string>();
                string link = "https://www.instagram.com/p/" + token["node"]["shortcode"]?.Value<string>();
                string likes = token["node"]["edge_liked_by"]["count"]?.Value<string>();
                string comments = token["node"]["edge_media_to_comment"]["count"]?.Value<string>();
                string time = StringExtensions.UnixTimeStampToDateTime(Convert.ToDouble(token["node"]["taken_at_timestamp"].Value<string>())).DayOfWeek + " - " + StringExtensions.UnixTimeStampToDateTime(Convert.ToDouble(token["node"]["taken_at_timestamp"].Value<string>())).ToShortTimeString();
                double timeStamp = Convert.ToDouble(token["node"]["taken_at_timestamp"].Value<string>());
                var dateNow = DateTime.Now.AddMonths(-1);
                var postDate = StringExtensions.UnixTimeStampToDateTime(Convert.ToDouble(token["node"]["taken_at_timestamp"].Value<string>()));
                if (!getAll)
                {
                    if (postDate < dateNow)
                    {
                        break;
                    }
                }

                string location = "Unknown";
                try
                {
                    location = token["node"]["location"]["name"].Value<string>();
                }
                catch
                {
                    Console.WriteLine("Location issues");
                }

                string views = "null";
                try
                {
                    views = token["node"]["video_view_count"].Value<string>();
                }
                catch
                {
                    Console.WriteLine("Views issues");
                }
                var instaObj = new InstaObject(caption, link, time, location, comments, likes, views, timeStamp);
                dataList.Add(instaObj);
            }

            return dataList;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

