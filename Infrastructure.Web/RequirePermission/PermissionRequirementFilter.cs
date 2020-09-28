using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Web.RequirePermission
{
    public class PermissionRequirementFilter : IAuthorizationFilter
    {
        private readonly PermissionType _permissionType;
        private const string ClaimType = "Permission";

        public PermissionRequirementFilter(PermissionType permissionType)
        {
            _permissionType = permissionType;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasPermission = context.HttpContext.User.Claims.Any(claim =>
                claim.Type == ClaimType &&
                claim.Value == ((int)_permissionType).ToString());

            if (!hasPermission)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
