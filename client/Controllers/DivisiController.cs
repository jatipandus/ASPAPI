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
using OfficeOpenXml;
using System.Text;
using Microsoft.Ajax.Utilities;
using client.Models;

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

        public async Task<ActionResult> DivisiExcel()
        {
            var columnHeaders = new string[]
            {
                "Name",
                "Department Name",
                "Ditambahkan"
            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Division");
                using (var cells = worksheet.Cells[1, 1, 1, 3])
                {
                    cells.Style.Font.Bold = true;
                }

                for (var i = 0; i < columnHeaders.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = columnHeaders[i];
                }

                var j = 2;
                HttpResponseMessage response = await client.GetAsync("divisi");
                if (response.IsSuccessStatusCode)
                {
                    var readTask = await response.Content.ReadAsAsync<IList<DivisiViewModel>>();
                    foreach (var divisi in readTask)
                    {
                        worksheet.Cells["A" + j].Value = divisi.DivisiName;
                        worksheet.Cells["B" + j].Value = divisi.DepartmentName;
                        worksheet.Cells["C" + j].Value = divisi.CreateDate.ToString("MM/dd/yyyy");
                        j++;
                    }
                }
                result = package.GetAsByteArray();
            }
            return File(result, "application/ms-excel", $"Division-{DateTime.Now.ToString("hh:mm:ss-MM/dd/yyyy")}.xlsx");
        }

        public async Task<ActionResult> DivisiCSV()
        {
            var columnHeaders = new string[]
            {
                "Nama",
                "Department Name",
                "Ditambahkan"
            };
            HttpResponseMessage response = await client.GetAsync("divisi");
            var readTask = await response.Content.ReadAsAsync<IList<DivisiViewModel>>();
            var departmentRecords = from divisi in readTask
                                    select new object[]{
                    $"{divisi.DivisiName}",
                    $"{divisi.DepartmentName}",
                    $"\"{divisi.CreateDate.ToString("MM/dd/yyyy")}\""
            }.ToList();
            var departmentcsv = new StringBuilder();
            departmentRecords.ForEach(line =>
            {
                departmentcsv.AppendLine(string.Join(",", line));
            });
            byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", columnHeaders)}\r\n{departmentcsv.ToString()}");
            return File(buffer, "text/csv", $"Division-{DateTime.Now.ToString("hh:mm:ss-MM/dd/yyyy")}.csv");
        }
        public ActionResult DivisiReport(DivisiViewModel department)
        {
            ReportDivisi deptreport = new ReportDivisi();
            byte[] abytes = deptreport.PrepareReport(exportToPdf());
            return File(abytes, "application/pdf");
        }

        public List<DivisiViewModel> exportToPdf()
        {
            IEnumerable<DivisiViewModel> models = null;
            var responsTask = client.GetAsync("divisi");
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
            return models.ToList();
        }

    }
}