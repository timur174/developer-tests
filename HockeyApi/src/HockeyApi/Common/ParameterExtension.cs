using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HockeyApi.Common
{
    public static class ParameterExtension
    {
		public static void CreateParameter(this IDbCommand command, object value, string name)
		{
			var param = command.CreateParameter();
			param.Value = value;
			param.ParameterName = name;
			command.Parameters.Add(param);
			//return param;
		}
	}
}
