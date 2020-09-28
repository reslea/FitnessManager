using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Infrastructure.Web.RequirePermission
{
    public class PermissionRequirementAttribute : TypeFilterAttribute
    {
        public PermissionRequirementAttribute(params PermissionType[] permissionTypes)
            : base(typeof(PermissionRequirementFilter))
        {
            Arguments = permissionTypes.Cast<object>().ToArray();
        }
    }
}
