using BelajarAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BelajarAPI.Repository.Interface
{
    interface IDepartmentRepository
    {
        IEnumerable<DepartmentModels> Get();
        Task<IEnumerable<DepartmentModels>> Get(int Id);
        int Create(DepartmentModels department);
        int Update(int Id, DepartmentModels department);
        int Delete(int Id);
    }
}