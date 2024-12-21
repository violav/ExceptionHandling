using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Data.Data.EF.Context;
public static class DbContextExtensions
    {
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
    }

