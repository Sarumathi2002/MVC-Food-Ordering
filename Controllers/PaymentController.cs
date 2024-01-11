using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using Foodordering.Models;
using Order = Razorpay.Api.Order;

namespace Foodordering.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }
       [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        /*[HttpPost]
       public ActionResult Index(RazorPayOrder Py)
    {
        if (ModelState.IsValid) { //checking model state
            
            //update student to db
            
            return RedirectToAction("CreateOrder");
        }
        return View(Py);
    }*/
     /*[HttpPost]  
        public IActionResult Index(PaymentRequest model)  
        {  
            
            if (ModelState.IsValid)  
            {  
                var user=new PaymentRequest()
                {
                Name = model.Name,  
                Email = model.Email
                };
                return RedirectToAction("Index"); 
            } 
            else{
            TempData["errorMessage"]="Try Again!";
            return View(model);
            }  
        }  */

      [HttpPost]
            public ActionResult CreateOrder(PaymentRequest _requestData)
            {
                // Generate random receipt number for order
                Random randomObj = new Random();
                string transactionId = randomObj.Next(10000000, 100000000).ToString();
                RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_VH5W3tGtgKw2v6", "EbU6cZaU5Lsi0mcOI52T4dHs");
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", _requestData.Amount * 100);  // Amount will in paise
                options.Add("receipt", transactionId);
                options.Add("currency", "INR");
                options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
                                                     //options.Add("notes", "-- You can put any notes here --");
                Order orderResponse = client.Order.Create(options);
                string orderId = orderResponse["id"].ToString();
                // Create order model for return on view
                RazorPayOrder orderModel = new RazorPayOrder
                {
                    OrderId = orderResponse.Attributes["id"],
                    RazorPayAPIKey = "rzp_test_VH5W3tGtgKw2v6",
                    Amount = _requestData.Amount * 100,
                    Currency = "INR",
                    Name = _requestData.Name,
                    Email = _requestData.Email,
                };
                // Return on PaymentPage with Order data
                return View("CreateOrder", orderModel);
            }

            [HttpPost]
            public ActionResult Complete()
            {
                // Payment data comes in url so we have to get it from url
                // This id is razorpay unique payment id which can be use to get the payment details from razorpay server
                string paymentId = HttpContext.Request.Form["rzp_paymentid"].ToString();
                // This is orderId
                string orderId = HttpContext.Request.Form["rzp_orderid"].ToString();
                RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_VH5W3tGtgKw2v6", "EbU6cZaU5Lsi0mcOI52T4dHs");
                Payment payment = client.Payment.Fetch(paymentId);
                // This code is for capture the payment 
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", payment.Attributes["amount"]);
                Payment paymentCaptured = payment.Capture(options);
                string amt = paymentCaptured.Attributes["amount"];

                //// Check payment made successfully
                if (paymentCaptured.Attributes["status"] == "captured")
                {
                    // Create these action method
                    ViewBag.Message = "Paid successfully";
                    ViewBag.OrderId = paymentCaptured.Attributes["id"];
                    return View("Result");
                }
                else
                {
                    ViewBag.Message = "Payment failed, something went wrong";
                    return View("Result");
                }
            }
        }
    }