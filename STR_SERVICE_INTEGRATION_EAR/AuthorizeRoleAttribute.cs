using System.Linq;
using System.Security.Claims;
using System.Web.Http.Controllers;
using System.Web.Http;


namespace STR_SERVICE_INTEGRATION_EAR
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedRoles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            this.allowedRoles = roles;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var identity = (ClaimsIdentity)actionContext.ControllerContext.RequestContext.Principal.Identity;
            var roleClaim = identity.FindFirst("rol");

            if (roleClaim != null && allowedRoles.Contains(roleClaim.Value))
            {
                return true;
            }

            return false;
        }
    }
}