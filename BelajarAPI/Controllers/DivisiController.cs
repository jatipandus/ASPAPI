using BelajarAPI.Context;
using BelajarAPI.Models;
using BelajarAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace BelajarAPI.Controllers
{
    public class DivisiController : ApiController
    {
        //MyContext conn = new MyContext();
        DivisiRepository divisis = new DivisiRepository();
        [HttpGet]
        public IHttpActionResult Get()
        {
            if (divisis.Get() == null)
            {
                return Content(HttpStatusCode.NotFound, "Data Division is Empty!");
            }
            return Ok(divisis.Get());
        }
        [HttpGet]
        [ResponseType(typeof(DivisiViewModel))]

        public async Task<IEnumerable<DivisiViewModel>> Get(int Id)
        {
            if (await divisis.Get(Id) == null)
            {
                return null;
            }
            return await divisis.Get(Id);
        }
        public IHttpActionResult Post(DivisiModels divisi)
        {
            if ((divisi.Name != null) || (divisi.Name != ""))
            {
                divisis.Create(divisi);
                return Ok("Division Add Successfully!"); //Status 200 OK
            }
            return BadRequest("Failed to Add Division");
        }

        public IHttpActionResult Put(int Id, DivisiModels divisi)
        {
            if ((divisi.Name != null) && (divisi.Name != ""))
            {
                divisis.Update(Id, divisi);
                return Ok("Division Updated Successfully!"); //Status 200 OK
            }
            return BadRequest("Failed to Update Division");
        }

        public IHttpActionResult Delete(int Id)
        {
            var delete = divisis.Delete(Id);
            if (delete > 0)
            {
                return Ok("Divisi Delete Successfully");
            }
            return BadRequest("Failed to Delete");
        }

    }
}
