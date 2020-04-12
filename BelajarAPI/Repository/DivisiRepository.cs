using BelajarAPI.Models;
using BelajarAPI.Repository.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BelajarAPI.Repository
{
    public class DivisiRepository : IDivisiRepository
    {
        DynamicParameters parameters = new DynamicParameters();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);

        public int Create(DivisiModels divisi)
        {
            var procName = "SP_InsertDivisi";
            parameters.Add("@Name", divisi.Name);
            parameters.Add("@DepartmentId", divisi.DepartmentId);
            var create = conn.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
            return create;
        }

        public int Delete(int Id)
        {
            var procName = "SP_DeleteDivisi";
            parameters.Add("@Id", Id);
            var update = conn.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
            return update;
        }

        public IEnumerable<DivisiViewModel> Get()
        {
            var procName = "SP_ViewDivisi";
            var getalldivisi = conn.Query<DivisiViewModel>(procName, commandType: CommandType.StoredProcedure);
            return getalldivisi;
        }

        public async Task<IEnumerable<DivisiViewModel>> Get(int Id)
        {
            var asName = "SP_ViewDivisiGetId";
            parameters.Add("@Id", Id);
            var getDivisi = await conn.QueryAsync<DivisiViewModel>(asName, parameters, commandType: CommandType.StoredProcedure);
            return getDivisi;
        }

        public int Update(int Id, DivisiModels divisi)
        {
            var procName = "SP_UpdateDivisi";
            parameters.Add("@Id", Id);
            parameters.Add("@Name", divisi.Name);
            parameters.Add("@DepartmentId", divisi.DepartmentId);
            var update = conn.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
            return update;
        }

    }
}
