using BelajarAPI.Models;
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
    public class DepartmentRepository
    {
        DynamicParameters parameters = new DynamicParameters();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        public int Create(DepartmentModels department)
        {
            //throw new NotImplementedException();
            var procName = "SP_InsertDepartment";
            parameters.Add("@Name", department.Name);
            var create = conn.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
            return create;
        }

        public int Delete(int Id)
        {
            //throw new NotImplementedException();
            var procName = "SP_DeleteDepartment";
            parameters.Add("@id", Id);
            var update = conn.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
            return update;
        }

        public IEnumerable<DepartmentModels> Get()
        {
            //throw new NotImplementedException();
            var procName = "SP_ViewDepartment";
            var getalldepartment = conn.Query<DepartmentModels>(procName, commandType: CommandType.StoredProcedure);
            return getalldepartment;
        }

        public async Task<IEnumerable<DepartmentModels>> Get(int Id)
        {
            //throw new NotImplementedException();
            var asName = "SP_ViewDepartmentGetId";
            parameters.Add("@Id", Id);
            var getDept = await conn.QueryAsync<DepartmentModels>(asName, parameters, commandType: CommandType.StoredProcedure);
            return getDept;
        }

        public int Update(int Id, DepartmentModels department)
        {
            //throw new NotImplementedException();
            var procName = "SP_UpdateDepartment";
            parameters.Add("@Id", Id);
            parameters.Add("@Name", department.Name);
            var update = conn.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
            return update;
        }
    }
}