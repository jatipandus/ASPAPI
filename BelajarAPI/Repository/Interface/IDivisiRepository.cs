using BelajarAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelajarAPI.Repository.Interface
{
    interface IDivisiRepository
    {
        IEnumerable<DivisiViewModel> Get();
        Task<IEnumerable<DivisiViewModel>> Get(int Id);
        int Create(DivisiModels Divisi);
        int Update(int Id, DivisiModels Divisi);
        int Delete(int Id);
    }
}
