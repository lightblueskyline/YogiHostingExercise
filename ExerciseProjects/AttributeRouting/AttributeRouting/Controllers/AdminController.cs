using Microsoft.AspNetCore.Mvc;

namespace AttributeRouting.Controllers
{
    //[Route("News/[controller]/USA/[action]/{id?}")]
    [Route("News/[controller]/USA/[action]/{id:int}")]
    public class AdminController : Controller
    {
        //[Route("[controller]/CallMe")]
        public string Index()
        {
            return "'Admin' Controller, 'Index' View";
        }

        public string List()
        {
            return "'Admin' Controller, 'List' View";
        }

        //[Route("[controller]/CallMe/[action]")]
        public string Show()
        {
            return "'Admin' Controller, 'Show' View";
        }
    }
}
