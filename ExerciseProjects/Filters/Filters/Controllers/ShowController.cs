﻿using Microsoft.AspNetCore.Mvc;

namespace Filters.Controllers
{
    public class ShowController : Controller
    {
        public string Index()
        {
            return "This is the Index action on the Show Controller";
        }
    }
}
