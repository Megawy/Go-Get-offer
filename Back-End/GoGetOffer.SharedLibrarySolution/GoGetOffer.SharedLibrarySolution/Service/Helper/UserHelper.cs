using GoGetOffer.SharedLibrarySolution.Responses;
using System.Security.Claims;

namespace GoGetOffer.SharedLibrarySolution.Service.Helper
{
    public static class UserHelper
    {
        public static Response<T>? TryGetUserId<T>(ClaimsPrincipal user, out Guid userId)
        {
            userId = Guid.Empty;
            var userIdString = user.FindFirst("user")?.Value;

            if (!Guid.TryParse(userIdString, out userId))
            {
                return Response<T>.Failure("Invalid user ID in token.");
            }

            return null;
        }

        public static Response<IEnumerable<T>>? TryGetUserIdForEnumerable<T>(ClaimsPrincipal user, out Guid userId)
        {
            userId = Guid.Empty;
            var userIdString = user.FindFirst("user")?.Value;

            if (!Guid.TryParse(userIdString, out userId))
            {
                return Response<IEnumerable<T>>.Failure("Invalid user ID in token.");
            }

            return null;
        }
    }
}
