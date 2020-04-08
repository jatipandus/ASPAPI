using BelajarAPI.Context;
using BelajarAPI.Migrations;
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
    public class DepartmentController : ApiController
    {
        MyContext conn = new MyContext();
        DepartmentRepository departments = new DepartmentRepository();
        [HttpGet]

        public IEnumerable<DepartmentModels> Get()
        {
            return departments.Get();
        }
        [HttpGet]
        [ResponseType(typeof(DepartmentModels))]
        public async Task<IEnumerable<DepartmentModels>> Get(int Id)
        {
            return await departments.Get(Id);
        }
        public IHttpActionResult Post(DepartmentModels department)
        {
            if (department.Name =="")
            {
                return Content(HttpStatusCode.NotFound, "Failed To Add");                
            }
            departments.Create(department);
            return Ok("Department Add Successfully");
        }
        public IHttpActionResult Put(int Id, DepartmentModels department)
        {
            //var dept_id = conn.Departments.FirstOrDefault(x => x.Id == Id);

            //if (dept_id == null)
            //{
            //    return Content(System.Net.HttpStatusCode.NotFound, "Id not found");
            //}
            //else if (department.Name == "")
            //{
            //    return Content(System.Net.HttpStatusCode.NotFound, "Name cannot empty");
            //}
            //else
            //{
            //    departments.Update(Id, department);
            //    return Ok("Update successfully");
            //}
            var put = departments.Update(Id, department);
            if (put > 0)
            {
                return Ok("Department Update Successfully");
            }
            return BadRequest("Failed to Update Department");
        }
        public IHttpActionResult Delete(int Id)
        {
            //var dept_id = conn.Departments.FirstOrDefault(x => x.Id == Id);
            //if (dept_id == null)
            //{
            //    return BadRequest("Failed to delete department");
            //}
            //else
            //{
            //    departments.Delete(Id);
            //    return Ok("Deleted successfully");
            //}

            var delete = departments.Delete(Id);
            if (delete > 0)
            {
                return Ok("Department Delete Successfully");
            }
            return BadRequest("Failed to Delete");
        }
    }
}
