using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Areas.User.Controllers
{
    public class MessageController : Controller
    {
        public ActionResult TempMessage()
        {
            return PartialView();
        }
    }
}