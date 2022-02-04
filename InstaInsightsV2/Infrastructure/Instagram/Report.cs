using System;
using System.Collections.Generic;
using InstaInsightsV2.Domain.Models;

namespace InstaInsightsV2.Infrastructure.Instagram
{
    internal class Report
    {
        public List<InstaObject> SevenDaysList { get; }
        public InstaObject BestDayMonth { get; }
        public InstaObject BestDayWeek { get; }
        public InstaObject WorstInteractionMonth { get; }
        public InstaObject WorstInteractionWeek { get; }
        public InstaObject BestWeekendPost { get; }
        public InstaObject MostLikes { get; }
        public InstaObject MostComments { get; }
        public DateTime AveragePostWeek { get; }
        public List<string> PopularCaptions { get; }
        public string EmojisUsedWeek { get; }

        public Report(List<InstaObject> sevenDaysList, InstaObject bestDayMonth, InstaObject bestDayWeek, InstaObject worstInteractionMonth, InstaObject worstInteractionWeek, InstaObject bestWeekendPost, InstaObject mostLikes, InstaObject mostComments, DateTime averagePostWeek, List<string> popularCaptions, string emojisUsedWeek)
        {
            SevenDaysList = sevenDaysList;
            BestDayMonth = bestDayMonth;
            BestDayWeek = bestDayWeek;
            WorstInteractionMonth = worstInteractionMonth;
            WorstInteractionWeek = worstInteractionWeek;
            BestWeekendPost = bestWeekendPost;
            MostLikes = mostLikes;
            MostComments = mostComments;
            AveragePostWeek = averagePostWeek;
            PopularCaptions = popularCaptions;
            EmojisUsedWeek = emojisUsedWeek;
        }

        public Report(List<InstaObject> sevenDaysList)
        {
            SevenDaysList = sevenDaysList;
        }
    }
}