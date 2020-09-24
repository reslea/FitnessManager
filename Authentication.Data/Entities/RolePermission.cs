using Infrastructure.Data;

namespace Authentication.Data.Entities
{
    public class RolePermission : BaseEntity
    {
        public Role Role { get; set; }

        public int RoleId { get; set; }

        public PermissionType PermissionType { get; set; }
    }
}
