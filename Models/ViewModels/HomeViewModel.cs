// These are specifically for the RecentUsers and RecentJobs on home dashboard
namespace JobRecorderNet.Models.ViewModels
{
    public class HomeViewModel
    {
        public int UserCount { get; set; }
        public int JobCount { get; set; }

        public List<User> TopUsers { get; set; }
        public List<Job> RecentJobs { get; set; }
    }
}