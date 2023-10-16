using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{
    public class ContactUsModel : PageModel
    {
        private readonly ILogger<ContactUsModel> _logger;

        public ContactUsModel(ILogger<ContactUsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
