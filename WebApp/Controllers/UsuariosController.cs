using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAPI.Models;
using WebApp.Data;
using WebApp.Tools;

namespace WebApp.Controllers
{
    public class UsuariosController : Controller
    {
        private HttpClient httpClient = new HttpClient();
        private string url = "https://localhost:44313/api/Usuarios";

        // GET: Usuarios
        public async Task<ActionResult> Index()
        {
            var json = await httpClient.GetStringAsync(url);
            var listaUsuario = JsonConvert.DeserializeObject<List<Usuarios>>(json);
            return View(listaUsuario);
        }

        // GET: Usuarios/Create
        public async Task<ActionResult> Create()
        {
            // Se cargan los paises.
            var json = await httpClient.GetStringAsync("https://localhost:44313/api/pais");
            var listaPaises = JsonConvert.DeserializeObject<List<Pais>>(json);
            ViewBag.Pais = listaPaises;
            return View();
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Nombre,Apellido,EMail,FechaNacimiento,Telefono,SerContactado,paisId,IdPais")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                Uri uri = new Uri(url); 
                var data = JsonConvert.SerializeObject(usuarios);

                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

                var t = Task.Run(() => APIRequest.Post(uri, content));
                t.Wait();

                return RedirectToAction("Index");
            }

            // Se hace la recarga de los paises, por fallar la validadion de lo campos.
            var json = await httpClient.GetStringAsync("https://localhost:44313/api/pais");
            var listaPaises = JsonConvert.DeserializeObject<List<Pais>>(json);
            ViewBag.Pais = listaPaises;
            return View(usuarios);
        }

        // GET: Usuarios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Se cargan los paises.
            var json = await httpClient.GetStringAsync("https://localhost:44313/api/pais");
            var listaPaises = JsonConvert.DeserializeObject<List<Pais>>(json);
            ViewBag.Pais = listaPaises;

            json = await httpClient.GetStringAsync(url + "/" + id);
            Usuarios usuarios = JsonConvert.DeserializeObject<Usuarios>(json);

            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Apellido,EMail,FechaNacimiento,Telefono,SerContactado,pais,paisId")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {

                Uri uri = new Uri(url + "/" + usuarios.Id);
                var data = JsonConvert.SerializeObject(usuarios);

                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");

                var t = Task.Run(() => APIRequest.Put(uri, content));
                t.Wait();

                return RedirectToAction("Index");
            }
            return View(usuarios);
        }

        // GET: Usuarios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var json = await httpClient.GetStringAsync(url + "/" + id);
            Usuarios usuarios = JsonConvert.DeserializeObject<Usuarios>(json);

            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: Usuarios/Delete/5
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
