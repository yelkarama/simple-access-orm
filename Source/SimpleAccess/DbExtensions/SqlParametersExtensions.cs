using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SimpleAccess
{
    public static class SqlParametersExtensions
    {
        /// <summary>
        /// Takes the dynamic object and creates the Sql Parameters from its properties
        /// </summary>
        /// <param name="sqlParameters"></param>
        /// <param name="otherParameters"></param>
        public static void CreateSqlParametersFromDynamic(this List<SqlParameter> sqlParameters, dynamic otherParameters)
        {

            var otherParametersObj = otherParameters as Object;
            if (otherParametersObj != null)
            {
                 var sqlParams = otherParametersObj.GetType().GetProperties().Select(
                    param => {
                        object value = param.GetValue(otherParameters);
                        if (param.Name.GetType().Name.ToLower() == "string" && value != null)
                        {
                            value = SafeSqlLiteral(value.ToString());
                        }
                        return new SqlParameter("@" + Clean(param.Name), value ?? DBNull.Value );
                    }).ToList();
                sqlParameters.AddRange(sqlParams);
            }
        }

        /// <summary>
        /// Takes the dynamic object and creates the Sql Parameters from its properties
        /// </summary>
        /// <param name="sqlParameters"></param>
        /// <param name="otherParameters"></param>
        public static SqlParameter[] CreateSqlParametersFromDynamic(this SqlParameter[] sqlParameters, dynamic otherParameters)
        {
            var sqlParameterList = new List<SqlParameter>(sqlParameters);

            var otherParametersObj = otherParameters as Object;
            if (otherParametersObj != null)
            {
                var sqlParams = otherParametersObj.GetType().GetProperties().Select(
                   param =>
                   {
                       object value = param.GetValue(otherParameters);
                       if (param.Name.GetType().Name.ToLower() == "string" && value != null)
                       {
                           value = SafeSqlLiteral(value.ToString());
                       }
                       return new SqlParameter("@" + Clean(param.Name), value);
                   }).ToList();
                sqlParameterList.AddRange(sqlParams);
            }
            return sqlParameterList.ToArray();
        }

        public static SqlParameter ToDataParam<T>(this T value, string paramName)
            where T : struct
        {

            return new SqlParameter("@" + paramName, value);
        }

        public static SqlParameter ToDataParam<T>(this T value, string paramName, SqlDbType sqlDbType)
            where T : struct
        {
            var isEnum = typeof(T).IsEnum;

            var sqlParam = new SqlParameter("@" + paramName, sqlDbType)
            {
                Value = !isEnum ? value : GetEnumValue(value, sqlDbType)
            };
            return sqlParam;
        }

        public static SqlParameter ToDataParam<T>(this T? value, string paramName)
            where T : struct
        {
            return new SqlParameter("@" + paramName, value.HasValue? (object)value : DBNull.Value);
        }

        public static SqlParameter ToDataParam<T>(this T? value, string paramName, SqlDbType sqlDbType)
            where T : struct
        {

            var isEnum = typeof(T).IsEnum;

            var sqlParam = new SqlParameter("@" + paramName, sqlDbType)
            {
                Value = value.HasValue ? (!isEnum ? (object)value : GetEnumValue(value, sqlDbType)) : DBNull.Value
            };
            return sqlParam;
        }

        public static SqlParameter ToDataParam(this string value, string paramName)
        {
            var sqlParam = new SqlParameter("@" + paramName, SqlDbType.NVarChar, 4000)
            {
                Value = string.IsNullOrEmpty(value) ? DBNull.Value : (object)SafeSqlLiteral(value)
            };
            return sqlParam;
        }

        public static SqlParameter ToDataParam(this string value, string paramName, int size)
        {
            var sqlParam = new SqlParameter("@" + paramName, SqlDbType.NVarChar, size)
            {
                Value = string.IsNullOrEmpty(value)? DBNull.Value :  (object)SafeSqlLiteral(value)
            };
            return sqlParam;
        }

        public static SqlParameter ToUnSafeDataParam(this string value, string paramName, int size)
        {
            var sqlParam = new SqlParameter("@" + paramName, SqlDbType.NVarChar, size)
            {
                Value = (object)value ?? DBNull.Value
            };
            return sqlParam;
        }

        private static object GetEnumValue(object value, SqlDbType sqlDbType)
        {
            if (value == null)
                return DBNull.Value;
            object v = null;
            switch (sqlDbType)
            {
                case SqlDbType.Bit:
                    {
                        v = Convert.ToBoolean(value);
                        break;
                    }
                case SqlDbType.TinyInt:
                    {
                        v = Convert.ToByte(value);
                        break;
                    }
                case SqlDbType.Int:
                    {
                        v = Convert.ToInt32(value);
                        break;
                    }
                case SqlDbType.BigInt:
                    {
                        v = Convert.ToInt64(value);
                        break;
                    }
                case SqlDbType.SmallInt:
                    {
                        v = Convert.ToInt16(value);
                        break;
                    }
                default:
                {
                    throw new InvalidCastException(string.Format("Value of {0} cannot be converted to {1}", value.GetType().Name, sqlDbType.ToString()));
                }
            }

            return v;
        }

        private static string SafeSqlLiteral(string inputSQL)
        {
            return inputSQL.Replace("'", "''");
        }

        private static string Clean(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                switch (name[0])
                {
                    case '@':
                    case ':':
                    case '?':
                        return name.Substring(1);
                }
            }
            return name;
        }

   }
}