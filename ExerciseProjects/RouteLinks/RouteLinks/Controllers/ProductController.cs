using Microsoft.AspNetCore.Mvc;

namespace RouteLinks.Controllers
{
    public class ProductController : Controller
    {
        public string Index(int id, string name)
        {
            return $"Id value is: {id} Name value is: {name}";
        }
    }
}
