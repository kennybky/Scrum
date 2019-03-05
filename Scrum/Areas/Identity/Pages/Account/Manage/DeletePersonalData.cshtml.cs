using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Scrum.Data;

namespace Scrum.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        

        public IActionResult OnGet()
        {
            return new ForbidResult();
        }

        public ActionResult OnPost()
        {
            return new ForbidResult();
        }
    }
}