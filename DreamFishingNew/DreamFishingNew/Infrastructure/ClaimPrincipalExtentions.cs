using System.Security.Claims;

namespace DreamFishingNew.Infrastructure
{
    public static class ClaimPrincipalExtentions
    {
        public static string Id(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier).Value;
    }

    
}
