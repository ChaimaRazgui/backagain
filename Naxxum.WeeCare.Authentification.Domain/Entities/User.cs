namespace Naxxum.WeeCare.Authentification.Domain.Entities;

public class User : AggregateRoot
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public bool Active { get; set; }

    private User(int userId, string email, string passwordHash, string passwordSalt, DateTime createdAtUtc, bool active) : base(userId)
    {
        UserId = userId;
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        CreatedAtUtc = createdAtUtc;
        Active = active;
    }

    public static User Create(string email, string passwordHash, string passwordSalt, bool active=false)
    {
        return new User(0, email, passwordHash, passwordSalt, DateTime.UtcNow, active);
    }
}