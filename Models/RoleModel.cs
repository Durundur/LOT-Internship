using Microsoft.AspNetCore.Identity;

namespace LOT_TASK.Models
{
    public class RoleModel: IdentityRole<Guid>
    {
        public RoleModel(): base() { }
        public RoleModel(string roleName) : base(roleName) { }
    }
}
