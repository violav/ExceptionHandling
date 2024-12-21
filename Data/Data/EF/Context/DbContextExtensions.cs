using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Data.EF.Context;
public static class DbContextExtensions
    {
        public static async Task LockEntityItem<T, TKeyValue>(this DbContext context, Expression<Func<T, TKeyValue>> selector, TKeyValue keyValue) where T : class
        {
            var expression = selector.Body as MemberExpression;
            if (expression != null)
            {
                var pi = expression.Member as PropertyInfo;
                if (pi != null)
                {
                    await LockEntityItem<T>(context, pi.Name, (object)keyValue);
                }
            }
        }

        public static async Task LockEntityItem<T>(this DbContext context, string keyName, object? keyValue) where T : class
            => await LockEntityItem<T>(context, (keyName, keyValue));

        public static async Task LockEntityItem<T>(this DbContext context, params (string key, object? value)[] keyValues) where T : class
        {
            try
            {
                var eType = context.Model.FindEntityType(typeof(T));
                if (eType == null)
                    throw new InvalidOperationException("Type not mapped to any entity type");

                var tableName = eType.GetSchemaQualifiedTableName();
                var sqlParameters = new List<SqlParameter>();
                var sql = new StringBuilder();
                
                foreach (var kv in keyValues)
                {
                    var columnName = eType.FindDeclaredProperty(kv.key).GetColumnBaseName();
                    var columnValue = new SqlParameter($"@{kv.key}", kv.value);
                    sqlParameters.Add(columnValue);
                    if (sql.Length == 0)
                    {
                        sql.Append($"SELECT TOP 1 'K' FROM {tableName} _T0 WITH (UPDLOCK, HOLDLOCK) WHERE (");
                    }
                    else
                    {
                        sql.Append(" AND ");
                    }
                    sql.Append($"_T0.[{columnName}] = @{columnName}");
                }
                sql.Append(")");

                var result = await context.Database.ExecuteSqlRawAsync(sql.ToString(), sqlParameters.ToArray());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<IEnumerable<T>> QuerySqlDirectAsync<T>(this DbContext context, string sql, object param)
        {
            var cnn = context.Database?.GetDbConnection();
            var closeCnn = false;
            IEnumerable<T> result = null;

            try
            {
                if (cnn == null)
                    return null;

                if (cnn.State == ConnectionState.Closed)
                {
                    closeCnn = true;
                    await cnn.OpenAsync();
                }

                if (param != null)
                {
                    var asDict = (param as IDictionary<string, object>);
                    if (asDict == null)
                        asDict = (param as Newtonsoft.Json.Linq.JObject)?.ToObject<IDictionary<string, object>>();

                    if (asDict != null)
                    {
                        var newParam = new DynamicParameters();
                        foreach (var entry in asDict)
                        {
                            newParam.Add(entry.Key, entry.Value);
                        }
                        param = newParam;
                    }
                }

                if (context.Database.CurrentTransaction == null)
                {
                    result = await cnn.QueryAsync<T>(sql, param: param);
                }
                else
                {
                    result = await cnn.QueryAsync<T>(sql, param: param, transaction: (IDbTransaction)context.Database.CurrentTransaction.GetDbTransaction());
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (closeCnn && cnn.State == ConnectionState.Open)
                {
                    closeCnn = false;
                    cnn.Close();
                }
            }

            return result;
        }

        public static async Task<SqlMapper.GridReader> QueryMultipleSqlDirectAsync(this DbContext context, string sql, object param, Action<SqlMapper.GridReader> readResult)
        {
            var cnn = context.Database?.GetDbConnection();
            var closeCnn = false;
            SqlMapper.GridReader reader = null;
            try
            {
                if (cnn == null)
                    return null;

                if (cnn.State == ConnectionState.Closed)
                {
                    closeCnn = true;
                    await cnn.OpenAsync();
                }

                if (param != null)
                {
                    var asDict = (param as IDictionary<string, object>);
                    if (asDict == null)
                        asDict = (param as Newtonsoft.Json.Linq.JObject)?.ToObject<IDictionary<string, object>>();

                    if (asDict != null)
                    {
                        var newParam = new DynamicParameters();
                        foreach (var entry in asDict)
                        {
                            newParam.Add(entry.Key, entry.Value);
                        }
                        param = newParam;
                    }
                }

                if (context.Database.CurrentTransaction == null)
                {
                    reader = await cnn.QueryMultipleAsync(sql, param: param);
                }
                else
                {
                    reader = await cnn.QueryMultipleAsync(sql, param: param, transaction: (IDbTransaction)context.Database.CurrentTransaction.GetDbTransaction());
                }

                readResult(reader);
                return reader;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (closeCnn && cnn.State == ConnectionState.Open)
                {
                    closeCnn = false;
                    cnn.Close();
                }
            }

        }
    }

