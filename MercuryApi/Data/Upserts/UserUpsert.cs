namespace MercuryApi.Data.Upserts
{
    public class UserUpsert
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
        // Eventually have to add team id, won't need team id for now to just create a user.
    }
}
