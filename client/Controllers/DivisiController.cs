using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using BelajarAPI.Models;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using BelajarAPI.Context;

namespace client.Controllers
{
    public class DivisiController : Controller
    {
        MyContext conn = new MyContext();
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:52133/api/")
        };

        public JsonResult XLoadDivisi()
        {
            IEnumerable<DivisiViewModel> models = null;
            var responsTask = client.GetAsync("Divisi");
            responsTask.Wait();
            var result = responsTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<DivisiViewModel>>();
                readTask.Wait();
                models = readTask.Result;
            }
            else
            {
                models = Enumerable.Empty<DivisiViewModel>();
                ModelState.AddModelError(string.Empty, "server error, try later");
            }
            //return Json(new { data = models }, JsonRequestBehavior.AllowGet);
            return new JsonResult { Data = models, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        // GET: Division
        public ActionResult Index()
        {
            return View(XLoadDivisi());
        }
        public JsonResult Insert(DivisiModels divisi)
        {
            var myContent = JsonConvert.SerializeObject(divisi);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (divisi.Id == 0) //input
            {
                var result = client.PostAsync("Divisi", byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else //update
            {
                var result = client.PutAsync("Divisi/" + divisi.Id, byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public async Task<JsonResult> GetById(int Id)
        {
            HttpResponseMessage response = await client.GetAsync("Divisi");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<DivisiViewModel>>();
                var divisi = data.FirstOrDefault(s => s.Id == Id);
                var json = JsonConvert.SerializeObject(divisi, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
                return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return Json("Internal Server Error");
        }

        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("Divisi/" + Id).Result;
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}