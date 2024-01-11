using System;
using Microsoft.AspNetCore.Mvc;
namespace Foodordering.Controllers
{
    public class ErrorController : Controller
    {

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch(statusCode)
            {
                case 404:
                 ViewBag.ErrorMessage="Sorry, the Page you requested could not be found";
                 break;
            }
            return View("NotFound");
        }
 
        public IActionResult ServerError()
        {
            //ViewBag.statusCode="500";
            //ViewBag.ErrorMessage="Sorry, An Server error Occured";
            return View("ServerError");
        }
        
    }
}