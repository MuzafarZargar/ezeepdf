using System.Data;
using EzeePdf.Core.Repositories;

namespace EzeePdf.Core.Model.Sql
{
    public class SqlRequest(string procName, params Param[] parameters)
    {
        public string ProcName { get; set; } = procName;
        public List<Param>? Parameters { get; set; } = parameters?.ToList();
        public bool HasOuputParameters =>
            Parameters?.Any(p =>
                p.Direction == ParameterDirection.Output ||
                p.Direction == ParameterDirection.InputOutput ||
                p.Direction == ParameterDirection.ReturnValue) == true;

        public void Add(Param parameter)
        {
            if (Parameters is null)
            {
                Parameters = new List<Param>();
            }
            Parameters.Add(parameter);
        }
        public static implicit operator SqlRequest(string name) => new(name);
    }
}
