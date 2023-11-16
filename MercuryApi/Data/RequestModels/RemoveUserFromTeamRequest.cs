namespace MercuryApi.Data.RequestModels
{
    public class RemoveUserFromTeamRequest
    {
        public int TeamId { get; set; }

        public int UserId { get; set; }
    }
}
