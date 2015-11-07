﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using SimpleAccess.DbExtensions;

namespace SimpleAccess
{
    public class StoredProcedureParameters
    {
        private string _entityName;
        private List<PropertyInfo> _outParameterPropertyInfoCollection;
        private List<SqlParameter> _spOutParameters;
        private List<SqlParameter> _sqlParameters;

        private ParametersType? _storedParametersType = null;
        private SqlParameter IdentityKeySqlParameter { get; set; }


        public void AddSqlParameters(dynamic paramsObject)
        {
            _sqlParameters.CreateSqlParametersFromDynamic(paramsObject as Object);
        }

        public SqlParameter[] CreateSqlParametersFromProperties(ParametersType parametersType)
        {

            _outParameterPropertyInfoCollection = new List<PropertyInfo>();
            _spOutParameters = new List<SqlParameter>();

            var procedureType = this.GetType();
            var propertiesForSqlParams = procedureType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);

            _sqlParameters =
                propertiesForSqlParams.Select(propertyInfo => CreateSqlParameter(propertyInfo, parametersType, propertiesForSqlParams))
                    .Where<SqlParameter>(p => p != null).ToList();

            _storedParametersType = parametersType;
            return _sqlParameters.ToArray();
        }

        public SqlParameter[] GetSpParameters(ParametersType parametersType) 
        {
            if (_storedParametersType != parametersType)
                return CreateSqlParametersFromProperties(parametersType);

            if (_sqlParameters == null || _sqlParameters.Count < 1)
                return CreateSqlParametersFromProperties(parametersType);
            else
                return _sqlParameters.ToArray();
        }

        private SqlParameter CreateSqlParameter(PropertyInfo propertyInfo, ParametersType parametesType, IEnumerable<PropertyInfo> propertyInfos)
        {
            object value = propertyInfo.GetValue(this, new object[] { });

            var sqlParam = new SqlParameter(string.Format("@{0}", propertyInfo.Name), value);

            if (propertyInfo.PropertyType.Name == "String" && value != null)
            {
                value = SafeSqlLiteral(value.ToString());
            }
            
            if ((propertyInfo.PropertyType.IsGenericType
                || propertyInfo.PropertyType.Name == "String") & value == null)
            {
                sqlParam.IsNullable = true;
                sqlParam.Value = DBNull.Value;
            }

            var attrbutes = propertyInfo.GetCustomAttributes(true);

            /*
            if (propertyInfo.PropertyType.IsGenericType)
            {
                if (propertyInfo.PropertyType.GetGenericArguments()[0].IsEnum)
                {
                    sqlParam.Value = value.ToString();
                }
            }
            if (propertyInfo.PropertyType.IsEnum)
                sqlParam.Value = value.ToString();
            */
            if (propertyInfo.GetMethod.IsVirtual)
                return null;

            if (attrbutes.FirstOrDefault(a => a is NotASpParameterAttribute) != null)
                return null;                

            var dbColumnPropertyAttribute =
                attrbutes.FirstOrDefault(a => a is DbColumnPropertyAttribute) as DbColumnPropertyAttribute;
            
            if (dbColumnPropertyAttribute != null)
            {
                var dbColumnPropertyPropertyInfo =
                    propertyInfos.FirstOrDefault(p => p.Name == dbColumnPropertyAttribute.DbColumnProperty);

                value = dbColumnPropertyPropertyInfo.GetValue(this, new object[] { });
                sqlParam.Value = value;
            }
                

            if (parametesType == ParametersType.Insert)
            {
                //var propertyDataType = propertyInfo.DeclaringType;
                
                var outParaAttr = attrbutes.FirstOrDefault(a => a is ParameterDirectionAttribute) as ParameterDirectionAttribute;
                if (outParaAttr != null)
                {
                    sqlParam.Direction = outParaAttr.SpParameterDirection;
                    _outParameterPropertyInfoCollection.Add(propertyInfo);
                    this._spOutParameters.Add(sqlParam);
                }

                //if (propertyInfo.PropertyType.GetType() is DateTime
                //    || propertyInfo.PropertyType.GetType() is DateTime?)
                //{
                //    value = value == null || (DateTime)value == DateTime.MinValue ? new DateTime(2000, 1, 1) : value;                       
                //}
            }

            Debug.WriteLine(sqlParam.ParameterName);
            return sqlParam;
        }

        public void LoadOutParametersProperties() 
        {
            _outParameterPropertyInfoCollection.ForEach(p => {
                var propertyName = p.Name;
                try
                {
                    p.SetValue(this, _spOutParameters.Single(
                        sp => sp.ParameterName == string.Format("@{0}", propertyName)).Value, new object[] { });
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Error in reading @{0} out parameter value", propertyName), ex);
                }
            });
            ClearSpParameters();
        }

        public void ClearSpParameters()
        {
            _spOutParameters.Clear();
            _sqlParameters.Clear();
            _outParameterPropertyInfoCollection.Clear();
        }

        public IList<ValidationResult> Validate()
        {
            IList<ValidationResult> result = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this, null, null), result, true);

            return result;
        }

        private string SafeSqlLiteral(string inputSQL)
        {
            return inputSQL.Replace("'", "''");
        }

    }
}
