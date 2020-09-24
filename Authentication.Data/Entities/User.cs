using Infrastructure.Data;

namespace Authentication.Data.Entities
{
    public class User : BaseEntity
    {
        public int RoleId { get; set; }

        public Role Role { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public RefreshToken RefreshToken { get; set; }
    }
}
