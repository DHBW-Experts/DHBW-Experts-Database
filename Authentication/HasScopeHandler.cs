using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DatabaseAPI.Authentication
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public HasScopeHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            // Split the scopes string into an array
            var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer).Value.Split(' ');

            var userIdsMatch = true;

            if(requirement.Scope.Contains("read:profile") || requirement.Scope.Contains("write:profile")){
                var userId = context.User.FindFirst(c => c.Type == "sub" && c.Issuer == requirement.Issuer).Value.Split('|').Last();

                var routeData = _httpContextAccessor.HttpContext.GetRouteData();
            
                var userIdOfRequest = routeData?.Values["userId"]?.ToString();
                userIdOfRequest = string.IsNullOrWhiteSpace(userIdOfRequest) ? string.Empty : userIdOfRequest;

                if(userId != userIdOfRequest && userId != "CwGPmQUnQ4s4R1jfkOxANVwQtfZSkmYr@clients"){
                    userIdsMatch = false;
                }
                Console.WriteLine("UserId:    " + userId);
                Console.WriteLine("RequestId: " + userIdOfRequest);
            }

            // Succeed if the scope array contains the required scope
            if (scopes.Any(s => s == requirement.Scope) && userIdsMatch)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}