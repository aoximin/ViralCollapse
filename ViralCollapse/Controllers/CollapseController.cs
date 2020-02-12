using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViralCollapse.Timer;

namespace ViralCollapse.Controllers
{
    public class CollapseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData()
        {
            return new JsonResult(new { address = timer.getAddress(), province = timer.getProvinceData(),provinceMap= timer.getProvinceMapData(), city=timer.getCityData() });
        }
    }
}