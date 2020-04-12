using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BelajarAPI.Models
{
    [Table("Tb_M_Divisi")]
    public class DivisiModels
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public virtual DepartmentModels Department { get; set; }
        public bool IsDelete { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Nullable <DateTimeOffset> UpdateDate { get; set; }
        public Nullable <DateTimeOffset> DeleteDate { get; set; }
        public DivisiModels()   { }

        public DivisiModels(DivisiModels divisi) //create
        {
            this.Name = divisi.Name;
            this.CreateDate = DateTimeOffset.Now;
            this.IsDelete = false;
            this.DepartmentId = Department.Id;
        }

        public void Update(DivisiModels divisi)
        {
            this.Name = divisi.Name;
            this.UpdateDate = DateTimeOffset.Now;
            this.DepartmentId = Department.Id;
        }

        public void Delete()
        {
            this.IsDelete = true;
            this.DeleteDate = DateTimeOffset.Now;
        }
    }
}