using Microsoft.AspNetCore.Mvc;

namespace bootcamp_api.Services
{
    public class LinkService
    {
        public LinkService() { }

        public static string GenerateLocalLink(ApiVersion version, int id)
        {
            return "/api/v" + version + "/Pets/" + id;
        }

        public static string GeneratePetfinderLink(int version, int id)
        {
            return "/v" + version + "/animals/" + id;
        }
    }
}