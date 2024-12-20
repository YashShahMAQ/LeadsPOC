namespace PullLeadsMicroService.Services
{
    public interface IPullService
    {
        /// <summary>
        /// Pulls and processes leads data from the external API.
        /// </summary>
        Task PullDataAsync();
    }
}
