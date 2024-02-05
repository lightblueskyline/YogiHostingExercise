using Microsoft.AspNetCore.Mvc;

namespace UnderstandingControllersViews.Components
{
    public class PageSize : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://www.msn.com");
            return View(response.Content.Headers.ContentLength);
        }
    }
}
