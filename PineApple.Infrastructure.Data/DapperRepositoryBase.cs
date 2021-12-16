using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Dapper.Contrib.Extensions;
using PineApple.Infrastructure.Data.Contracts;
using PineApple.Infrastructure.Data.Contracts.Helpers;
using static Dapper.SqlMapper;

namespace PineApple.Infrastructure.Data
{
    public abstract class DapperRepositoryBase : RepositoryBase
    {
        private Assembly _assembly;

        protected DapperRepositoryBase(IConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        private async Task<string> GetEmbeddedSqlAsync(string typeName, string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new ArgumentException("Argument can not be empty!", nameof(resourceName));

            // If the full resourceName is not specified, add the type name to narrow it down.
            if (!resourceName.Contains("."))
                resourceName = $"{typeName ?? GetType().Name}.{resourceName}.sql";

            if (_assembly == null)
                _assembly = GetEmbeddedResourceAssembly();

            string sql = await EmbeddedResourceHelper.GetContent(_assembly, resourceName);

            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentException($"Could not read embedded resource with name ending with '{resourceName}'.", nameof(resourceName));

            return sql;
        }

        /// <summary>
        /// Gets the assembly that contains the embedded resource sql files.
        /// </summary>
        /// <returns>The <seealso cref="Assembly"/>.</returns>
        protected virtual Assembly GetEmbeddedResourceAssembly()
        {
            return GetType().Assembly;
        }

        protected async Task<int> ExecuteAsync(object parameters, string typeName, [CallerMemberName] string resourceName = null)
        {
            var sql = await GetEmbeddedSqlAsync(typeName, resourceName);
            var cmd = new CommandDefinition(sql, parameters, Transaction);

            return await Connection.ExecuteAsync(cmd);
        }

        protected async Task<object> ExecuteScalarAsync<T>(object parameters, string typeName, [CallerMemberName] string resourceName = null)
        {
            var sql = await GetEmbeddedSqlAsync(typeName, resourceName);
            var cmd = new CommandDefinition(sql, parameters, Transaction);

            return await Connection.ExecuteScalarAsync<T>(cmd);
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(object parameters, string typeName, [CallerMemberName] string resourceName = null)
        {
            var sql = await GetEmbeddedSqlAsync(typeName, resourceName);
            var cmd = new CommandDefinition(sql, parameters, Transaction);

            return await Connection.QueryAsync<T>(cmd);
        }

        protected async Task<IEnumerable<TFirst>> QueryAsync<TFirst, TSecond>(object parameters, string typeName, Func<TFirst, TSecond, TFirst> map, [CallerMemberName] string resourceName = null)
        {
            var sql = await GetEmbeddedSqlAsync(typeName, resourceName);
            var cmd = new CommandDefinition(sql, parameters, Transaction);

            return await Connection.QueryAsync(cmd, map);
        }

        protected async Task<IEnumerable<TFirst>> QueryAsyncSplit<TFirst, TSecond>(object parameters, string typeName, Func<TFirst, TSecond, TFirst> map, string splitOn, [CallerMemberName] string resourceName = null)
        {
            var sql = await GetEmbeddedSqlAsync(typeName, resourceName);
            var cmd = new CommandDefinition(sql, parameters, Transaction);

            return await Connection.QueryAsync(cmd, map, splitOn);
        }

        protected async Task<IEnumerable<TFirst>> QueryAsync<TFirst, TSecond, TThird>(object parameters, string typeName, Func<TFirst, TSecond, TThird, TFirst> map, [CallerMemberName] string resourceName = null)
        {
            var sql = await GetEmbeddedSqlAsync(typeName, resourceName);
            var cmd = new CommandDefinition(sql, parameters, Transaction);

            return await Connection.QueryAsync(cmd, map);
        }

        protected async Task<IEnumerable<TFirst>> QueryAsync<TFirst, TSecond, TThird, TFourth>(object parameters, string typeName, Func<TFirst, TSecond, TThird, TFourth, TFirst> map, [CallerMemberName] string resourceName = null)
        {
            var sql = await GetEmbeddedSqlAsync(typeName, resourceName);
            var cmd = new CommandDefinition(sql, parameters, Transaction);

            return await Connection.QueryAsync(cmd, map);
        }

        protected async Task<T> QueryFirstAsync<T>(object parameters, string typeName, [CallerMemberName] string resourceName = null)
        {
            var sql = await GetEmbeddedSqlAsync(typeName, resourceName);
            var cmd = new CommandDefinition(sql, parameters, Transaction);

            return await Connection.QueryFirstAsync<T>(cmd);
        }

        protected async Task<T> QueryFirstOrDefaultAsync<T>(object parameters, string typeName, [CallerMemberName] string resourceName = null)
        {
            var sql = await GetEmbeddedSqlAsync(typeName, resourceName);
            var cmd = new CommandDefinition(sql, parameters, Transaction);

            return await Connection.QueryFirstOrDefaultAsync<T>(cmd);
        }

        protected async Task QueryMultipleAsync(object parameters, string typeName, Action<GridReader> action, [CallerMemberName] string resourceName = null)
        {
            var sql = await GetEmbeddedSqlAsync(typeName, resourceName);
            var cmd = new CommandDefinition(sql, parameters, Transaction);

            using (var reader = await Connection.QueryMultipleAsync(cmd))
            {
                action?.Invoke(reader);
            }
        }

        protected Task<int> InsertAsync<T>(T entity) where T : class
        {
            return Connection.InsertAsync<T>(entity);
        }
    }
}
