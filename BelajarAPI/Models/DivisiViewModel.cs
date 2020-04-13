using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BelajarAPI.Models
{
    public class DivisiViewModel
    {
        public int Id { get; set; }
        public string DivisiName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }

        public List<DepartmentModels> Department { get; set; }
    }
}