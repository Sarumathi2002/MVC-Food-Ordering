using Microsoft.AspNetCore.Mvc;
using Foodordering.Models;
using Microsoft.AspNetCore.Identity;
using Foodordering.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

using Message=System.Console;

namespace Foodordering.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> UserManger, SignInManager<ApplicationUser> signInManger)
        {
            _userManager = UserManger;
            _signInManager = signInManger;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
       // [Route("Account/Login/{Email?}")]
        public async Task<IActionResult> Login(LoginViewModel login, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid Login Attempt");

            }
            return View(login);
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            try
            {
            if (ModelState.IsValid)
            {
                if (register == null)
                {
                    throw new ArgumentNullException(nameof(register));
                }
                if (string.IsNullOrEmpty(register.Name))
                {
                    throw new ArgumentException("Name cannot be empty.", nameof(register.Name));
                }
                if (register.Name.Length<3 || register.Name.Length>20)
                {
                    throw new ApplicationException("Username length should be between 3 and 20 characters.");
                }
                var user = new ApplicationUser()
                {
                    Name = register.Name,
                    Address = register.Address,
                    Email = register.Email,
                    UserName = register.Email
                };
                var result = await _userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    await _signInManager.PasswordSignInAsync(user, register.Password, false, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        //USING KEYWORD
                        Message.WriteLine("", err.Description);
                    }
                }
            }
        }
        catch(DbUpdateException dbUpdateException)
            {
                ModelState.AddModelError("","An error occurred while saving to the database: "+dbUpdateException?.Message);
            }
            catch(ArgumentException argumentException)
            {
                ModelState.AddModelError("","Invalid argument:"+argumentException.Message);
            }
            catch(FormatException formatException)
            {
                ModelState.AddModelError("","Invalid format: "+formatException.Message);
            }
            catch(InvalidOperationException invalidOperationException)
            {
                ModelState.AddModelError("","Invalid Operation: "+invalidOperationException.Message);
            }
            catch(NullReferenceException nullReferenceException)
            {
                ModelState.AddModelError("","Null reference: "+nullReferenceException.Message);
            }
            catch(ApplicationException applicationException)
            {
                ModelState.AddModelError("",applicationException.Message);
            }
            catch(HttpRequestException httpRequestException)
            {
                //eg: network errors
                ModelState.AddModelError("","HttP request exception occurred: "+httpRequestException.Message);
            }
            catch(SqlException sqlException)
            {
                //eg: database connection failures
                 ModelState.AddModelError("","SQL Exception occurred: "+sqlException.Message);
            }
            catch (Exception exception)
            {
                //Handles general exceptions,eg: internal server errors
                ModelState.AddModelError("","An error occured.");
            }
            return View(register);
        }
    }
}