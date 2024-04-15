using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using WebApplication1.Models;
using Product = WebApplication1.Models.Product;

namespace WebApplication1.Controllers
{
    public class ProductCheckoutController : Controller
    {
        public IActionResult Index()
        {
            List<Product> products = new List<Product>();

            products = new List<Product>
            {
               new Product
               {
                   Name = "Test 1",
                   Rate = 1500,
                   Quantity = 2,
                   ImagePath = "Img/Test1.png"
               },
               new Product
               {
                   Name = "Test 2",
                   Rate = 1000,
                   Quantity = 1,
                   ImagePath ="Img/Test2.png"
               }
            };
            return View(products);
        }

        public IActionResult Checkout()
        {
            List<Product> products = new List<Product>();

            products = new List<Product>
            {
               new Product
               {
                   Name = "Test 1",
                   Rate = 1500,
                   Quantity = 2,
                   ImagePath = "Img/Test1.png"
               },
               new Product
               {
                   Name = "Test 2",
                   Rate = 1000,
                   Quantity = 1,
                   ImagePath ="Img/Test2.png"
               }
            };

            var domain = "https://localhost:7142/";
            var option = new SessionCreateOptions
            {
                SuccessUrl = domain + $"ProductCheckout/OrderConfirmation",
                CancelUrl = domain + $"ProductCheckout/Login",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
            };

            foreach(var item in products)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Rate * 100),
                        Currency = "INR",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Name,
                        }
                    },
                    Quantity = item.Quantity,
                };
                option.LineItems.Add(sessionListItem);
            }
            
            var service = new SessionService();
            Session session = service.Create(option);

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }
    }
}
