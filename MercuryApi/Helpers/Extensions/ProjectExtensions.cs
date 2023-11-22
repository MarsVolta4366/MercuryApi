namespace MercuryApi.Helpers.Extensions
{
    public static class ProjectExtensions
    {
        public static void UnassignUser(this Project project, int userId)
        {
            foreach (Ticket ticket in project.Tickets.Where(t => t.UserId == userId))
            {
                ticket.UserId = null;
            }
        }
    }
}
