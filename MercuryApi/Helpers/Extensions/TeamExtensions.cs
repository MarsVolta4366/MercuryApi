namespace MercuryApi.Helpers.Extensions
{
    public static class TeamExtensions
    {
        public static void UnassignUser(this Team team, int userId)
        {
            foreach (Project project in team.Projects)
            {
                project.UnassignUser(userId);
            }
        }
    }
}
