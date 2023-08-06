using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using WebAppWithAD.Models;
using System.Security.Claims;

namespace WebAppWithAD.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
        /// <summary>
        /// Get the user name, user domain and email of the user from the authentication claims
        /// </summary>
        /// <param name="user">Auth Claims</param>
        /// <returns>Azure AD</returns>
        public static UserAzureAD GetUserOnAzureAd(ClaimsPrincipal user)
        {
            var preferredUsernameClaim = user.Claims.FirstOrDefault(c => c.Type.Equals("preferred_username"));

            if (preferredUsernameClaim != null)
            {
                return new UserAzureAD
                {
                    user_name = user.Claims.FirstOrDefault(p => p.Type.Equals("name")).Value,
                    user_email = preferredUsernameClaim.Value,
                    user_domain = string.Format(@"cpiccr\{0}", preferredUsernameClaim.Value.Split('@')[0])
                };
            }
            return null; // Or throw an exception if preferred_username claim is required
        }

    }
}