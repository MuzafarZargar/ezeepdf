using System.Data;

namespace EzeePdf.Core.Repositories
{
    public class Param
    {
        public Param(string name, object? value, bool isTable = false)
        {
            Name = name;
            Value = value;
            IsTable = isTable;
        }

        public Param(string name, object? value, DbType type)
        {
            Name = name;
            Value = value;
            Type = type;
        }
        public Param(string name, object? value, ParameterDirection direction)
        {
            Name = name;
            Value = value;
            Direction = direction;
        }
        public Param(string name, object? value, ParameterDirection direction, DbType type)
        {
            Name = name;
            Value = value;
            Direction = direction;
            Type = type;
        }
        public bool IsTable { get; set; }
        public DbType? Type { get; set; }
        public ParameterDirection? Direction { get; set; } = ParameterDirection.Input;
        public string Name { get; set; }
        public object? Value { get; set; }
        public int? Size { get; set; }
    }
}
