namespace News_Site.Models.ViewModel
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalNews { get; set; }
        public int TotalCategories { get; set; }
        public List<CategoryNewsCount> CategoryCounts { get; set; }
        public List<NewsEmoteCount> EmoteCounts { get; set; }
        public List<RoleDistribution> RoleDistributions { get; set; }
        public List<NewsDailyStat> DailyNewsCounts { get; set; }
    }

    public class CategoryNewsCount
    {
        public string CategoryName { get; set; }
        public int NewsCount { get; set; }
    }

    public class NewsEmoteCount
    {
        public string NewsTitle { get; set; }
        public int TotalEmoteClicks { get; set; }
    }

    public class RoleDistribution
    {
        public string RoleName { get; set; }
        public int Count { get; set; }
    }

    public class NewsDailyStat
    {
        public string Date { get; set; } 
        public int Count { get; set; }
    }
}
