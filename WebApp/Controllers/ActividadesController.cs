using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAPI.Models;
using WebAPI.ModelsViews;

using WebApp.Data;

namespace WebApp.Controllers
{
    public class ActividadesController : Controller
    {
        private HttpClient httpClient = new HttpClient();
        private string url = "https://localhost:44313/api/actividades";
        // GET: Actividades
        public async Task<ActionResult> Index()
        {
            var json = await httpClient.GetStringAsync(url);
            var ListaActividades = JsonConvert.DeserializeObject<List<VMActividades>>(json);
            return View(ListaActividades);
        }
    }
}
