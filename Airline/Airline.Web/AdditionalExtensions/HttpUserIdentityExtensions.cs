using System.Security.Claims;
using System.Security.Principal;

namespace Airline.Web.AdditionalExtensions
{
    public static class HttpUserIdentityExtensions
    {
        /// <summary>
        /// Get full username of authorized user by using Identity claim.
        /// </summary>
        public static string GetFullNameUser(this IIdentity identity)
        {
            if (identity == null)
                return null;

            var targetClaim = ((ClaimsIdentity)identity);

            var firstName = targetClaim.FindFirst("FirstName");
            var secondName = targetClaim.FindFirst(ClaimTypes.Surname);

            return $"{firstName.Value} {secondName.Value}";
        }

        /// <summary>
        /// Get first name of authorized user by using Identity claim.
        /// </summary>
        public static string GetFirstNameUser(this IIdentity identity)
        {
            if (identity == null)
                return null;

            var targetClaim = ((ClaimsIdentity)identity);

            var firstName = targetClaim.FindFirst("FirstName");

            return firstName.Value;
        }

        /// <summary>
        /// Get second name of authorized user by using Identity claim.
        /// </summary>
        public static string GetSecondNameUser(this IIdentity identity)
        {
            if (identity == null)
                return null;

            var targetClaim = ((ClaimsIdentity)identity);

            var secondName = targetClaim.FindFirst(ClaimTypes.Surname);

            return secondName.Value;
        }

        /// <summary>
        /// Get identifier of authorized user by using Identity claim.
        /// </summary>
        public static string GetIdUser(this IIdentity identity)
        {
            if (identity == null)
                return null;

            var targetClaim = ((ClaimsIdentity)identity);

            var id = targetClaim.FindFirst("UserId");

            return id.Value;
        }
    }
}