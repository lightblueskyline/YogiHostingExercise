using Microsoft.AspNetCore.Mvc;

using UnderstandingControllersViews.Models;
using UnderstandingControllersViews.Services;

namespace UnderstandingControllersViews.Components
{
    public class Cart : ViewComponent
    {
        private Coupon coupon;

        public Cart(Coupon coupon)
        {
            this.coupon = coupon;
        }

        //public string Invoke()
        //{
        //    return "This is from View Component";
        //}

        #region View Component Returning “ContentViewComponentResult”
        ///// <summary>
        ///// returning encoded HTML from View Components
        ///// </summary>
        ///// <returns></returns>
        //public IViewComponentResult Invoke()
        //{
        //    return Content("This is from <h2>View Component</h2>");
        //}
        #endregion

        #region View Component Returning “HtmlContentViewComponentResult”
        ///// <summary>
        ///// “non-encoded HTML”
        ///// </summary>
        ///// <returns></returns>
        //public IViewComponentResult Invoke()
        //{
        //    return new HtmlContentViewComponentResult(new HtmlString("This is from <h2>View Component</h2>"));
        //}
        #endregion

        #region View Component Returning Partial View
        public IViewComponentResult Invoke(bool showCart)
        {
            Product[] products;
            if (showCart)
            {
                products = new Product[]
                {
                    new Product() { Name = "Women Shoes", Price = 99 },
                    new Product() { Name = "Mens Shirts", Price = 59 },
                    new Product() { Name = "Children Belts", Price = 19 },
                    new Product() { Name = "Girls Socks", Price = 9 }
                };
            }
            else
            {
                products = new Product[] { };
            }

            ViewBag.Coupon = this.coupon.GetCoupon();

            return View(products);
        }
        #endregion
    }
}
