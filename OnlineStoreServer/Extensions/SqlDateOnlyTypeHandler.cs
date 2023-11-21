using Dapper;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnlineStoreServer.Extensions
{
    public class SqlDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override void SetValue(IDbDataParameter parameter, DateOnly date)
        {
            parameter.Value = date.ToString();
            Console.WriteLine(date.ToString());
        }

        public override DateOnly Parse(object value)
        {
            Console.WriteLine(value.ToString());
            return DateOnly.Parse(value.ToString());
        }
    }
}
