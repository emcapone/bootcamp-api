using Microsoft.AspNetCore.Mvc;

namespace bootcamp_api.Services
{
    public class LinkService
    {
        public LinkService() { }

        public static string GeneratePetsLink(ApiVersion version, int id)
        {
            return $"/api/v{version}/Pets/{id}";
        }

        public static string GenerateBookmarksLink(ApiVersion version, int id, int pf_version)
        {
            return $"/api/v{version}/Bookmarks/{id}/Petfinder/v{pf_version}";
        }

        public static string GeneratePetfinderLink(int version, int id)
        {
            return $"/v{version}/animals/{id}";
        }

        public static string GenerateCalendarEventsLink(ApiVersion version, int id)
        {
            return $"/api/v{version}/CalendarEvents/{id}";
        }

        public static string GenerateUserLink(ApiVersion version, string id)
        {
            return $"/api/v{version}/User/{id}";
        }
    }
}