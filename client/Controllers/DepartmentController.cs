using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using client.Models;
using BelajarAPI.Models;

namespace client.Controllers
{
    public class DepartmentController : Controller
    {
       DepartmentModels department = new DepartmentModels();

        // GET: Department
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:52133/api/")
        };

        public JsonResult XLoadDepartment()
        {
            IEnumerable<DepartmentModels> models = null;
            var responsTask = client.GetAsync("Department");
            responsTask.Wait();
            var result = responsTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<DepartmentModels>>();
                readTask.Wait();
                models = readTask.Result;
            }
            else
            {
                models = Enumerable.Empty<DepartmentModels>();
                ModelState.AddModelError(string.Empty, "server error, try later");
            }
            //return Json(new { data = models }, JsonRequestBehavior.AllowGet);
            return new JsonResult { Data = models, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Index()
        {
            return View(XLoadDepartment());
        }

        public JsonResult Insert(DepartmentModels department)
        {
            var myContent = JsonConvert.SerializeObject(department);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (department.Id == 0) //insert 
            {
                var result = client.PostAsync("Department", byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else //update
            {
                var result = client.PutAsync("Department/" + department.Id, byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public async Task<JsonResult> GetById(int Id)
        {
            HttpResponseMessage response = await client.GetAsync("Department");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<DepartmentModels>>();
                var dept = data.FirstOrDefault(s => s.Id == Id);
                var json = JsonConvert.SerializeObject(dept, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
                return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return Json("Internal Server Error");
        }

        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("Department/" + Id).Result;
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public async Task<ActionResult> Excel()
        {
            var columnHeaders = new string[]
            {
                "Name",
                "Tanggal Ditambahkan"
            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Department Excel");
                using (var cells = worksheet.Cells[1, 1, 1, 2])
                {
                    cells.Style.Font.Bold = true;
                }

                for (var i = 0; i < columnHeaders.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = columnHeaders[i];
                }

                var j = 2;
                HttpResponseMessage response = await client.GetAsync("department");
                if (response.IsSuccessStatusCode)
                {
                    var readTask = await response.Content.ReadAsAsync<IList<DepartmentModels>>();
                    foreach (var department in readTask)
                    {
                        worksheet.Cells["A" + j].Value = department.Name;
                        worksheet.Cells["B" + j].Value = department.CreateDate.ToString("MM/dd/yyyy");
                        j++;
                    }
                }
                result = package.GetAsByteArray();
            }
            return File(result, "application/ms-excel", $"Department.xlsx");
        }

        public async Task<ActionResult> CSV()
        {
            var columnHeaders = new string[]
            {
                "Nama Department",
                "Tanggal Ditambahkan"
            };

            HttpResponseMessage response = await client.GetAsync("department");

            var readTask = await response.Content.ReadAsAsync<IList<DepartmentModels>>();
            var departmentRecords = from department in readTask
                                    select new object[]
                                    {
                                      $"{department.Name}",
                                      $"\"{department.CreateDate.ToString("MM/dd/yyyy")}\""
                                    }.ToList();
            var departmentcsv = new StringBuilder();
            departmentRecords.ForEach(line =>
            {
                departmentcsv.AppendLine(string.Join(",", line));
            });

            byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", columnHeaders)}\r\n{departmentcsv.ToString()}");
            return File(buffer, "text/csv", $"Department.csv");
        }

        public ActionResult Report(DepartmentModels department)
        {
            ReportDepartment deptreport = new ReportDepartment();
            byte[] abytes = deptreport.PrepareReport(exportToPdf());
            return File(abytes, "application/pdf");
        }

        public List<DepartmentModels> exportToPdf()
        {
            IEnumerable<DepartmentModels> models = null;
            var responsTask = client.GetAsync("dept");
            responsTask.Wait();
            var result = responsTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<DepartmentModels>>();
                readTask.Wait();
                models = readTask.Result;
            }
            else
            {
                models = Enumerable.Empty<DepartmentModels>();
                ModelState.AddModelError(string.Empty, "server error, try later");
            }
            //return Json(new { data = models }, JsonRequestBehavior.AllowGet);
            return models.ToList();
        }
    }
}