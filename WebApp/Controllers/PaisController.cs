using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebAPI.Models;
using WebApp.Tools;


namespace WebApp.Controllers
{
    public class PaisController : Controller
    {
        private HttpClient httpClient = new HttpClient();
        private string url = "https://localhost:44313/api/Pais";

        // GET: Pais
        public async Task<ActionResult> Index()
        {
            var json = await httpClient.GetStringAsync(url);
            var listaPaises = JsonConvert.DeserializeObject<List<Pais>>(json);

            return View(listaPaises);
        }

        // GET: Pais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pais/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre")] Pais pais)
        {
            if (ModelState.IsValid)
            {

                Uri uri = new Uri(url);
                var data = "{\"Nombre\": \"" + pais.Nombre + "\"}";

                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

                var t = Task.Run(() => APIRequest.Post(uri, content));
                t.Wait();

                return RedirectToAction("Index");
            }
            return View(pais);
        }

        // GET: Pais/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var json = await httpClient.GetStringAsync(url + "/" + id);
            Pais pais = JsonConvert.DeserializeObject<Pais>(json);

            if (pais == null)
            {
                return HttpNotFound();
            }
            return View(pais);
        }

        // POST: Pais/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre")] Pais pais)
        {
            if (ModelState.IsValid)
            {
                Uri uri = new Uri(url + "/" + pais.Id);
                //var data = "{\"Id\": " + pais.Id + ",\"Nombre\": \"" + pais.Nombre + "\"}";
                var data = JsonConvert.SerializeObject(pais);

                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

                var t = Task.Run(() => APIRequest.Put(uri, content));
                t.Wait();

                return RedirectToAction("Index");
            }
            return View(pais);
        }

        // GET: Pais/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var json = await httpClient.GetStringAsync(url + "/" + id);
            Pais pais = JsonConvert.DeserializeObject<Pais>(json);
            if (pais == null)
            {
                return HttpNotFound();
            }
            return View(pais);
        }

        // POST: Pais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Uri uri = new Uri(url);
            var t = Task.Run(() => APIRequest.Delete(uri, id));
            t.Wait();

            return RedirectToAction("Index");
        }
    }
}
