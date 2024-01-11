using Foodordering.Models;
using Microsoft.AspNetCore.Mvc;
using Foodordering.Repository;
using Microsoft.AspNetCore.Authorization;
using Foodordering.ContextDBConfig;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Foodordering.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IData data;
        private readonly FoodApplicationDBContext context;
        public CartController(IData data, FoodApplicationDBContext context)
        {
            this.data = data;
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await data.GetUser(HttpContext.User);
            var cartsList = context.Carts.Where(c => c.UserId == user.Id).ToList();
            return View(cartsList);
        }
        [HttpPost]
        public async Task<IActionResult> SaveCart(Cart cart)
        {
            var user = await data.GetUser(HttpContext.User);
            cart.UserId = user?.Id;
            if (ModelState.IsValid)
            {
                context.Carts.Add(cart);
                context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetAddedCarts()
        {
            var user = await data.GetUser(HttpContext.User);
            var carts = context.Carts.Where(c => c.UserId == user.Id).Select(c => c.RecipeId).ToList();

            return Ok(carts);
        }
        /* [HttpPost]
         public IActionResult RemoveCartFromList(string Id)
         {
             if(!string.IsNullOrEmpty(Id))
             {
                 var cart=context.Carts.Where(c => c.RecipeId == Id).FirstOrDefault();
                 if(cart!=null)
                 {
                   context.Carts.Remove(cart);
                   context.SaveChanges();
                   return Ok();
                 }
             }
             return BadRequest();
         }*/
        [HttpGet]
        public async Task<IActionResult> GetCartList()
        {
            var user = await data.GetUser(HttpContext.User);
            var cartList = context.Carts.Where(c => c.UserId == user.Id).Take(3).ToList();
            return PartialView("_CartList", cartList);
        }
        [HttpPost]
        //[Route("{Id:int}")]
        public async Task<IActionResult> RemoveCartFromList(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var cart = context.Carts.Where(c => c.RecipeId == Id).FirstOrDefault();
                if (cart != null)
                {
                    context.Carts.Remove(cart);
                    context.SaveChanges();
                    return Ok();
                }
            }

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                client.BaseAddress = new Uri("https://localhost:7116/api/Food");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("https://localhost:7116/api/Food/Get");

                if (response.IsSuccessStatusCode)
                {

                    var data = response.Content.ReadAsStringAsync().Result;

                    var employee = JsonConvert.DeserializeObject<List<Cart>>(data);
                    return View(employee);
                }
                else
                {
                    return View("Error");
                }
            }
        }
    }
}