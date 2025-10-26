using api_poc_tmb.Data;
using api_poc_tmb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_poc_tmb.Services
{
    public class LLMSqlService : ILLMSqlService
    {
        private readonly DatabaseContext _dbContext;

        public LLMSqlService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Dictionary<string, object>>> ExecuteDynamicSqlAsync(string sql)
        {
            using var connection = _dbContext.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = sql;

            var resultList = new List<Dictionary<string, object>>();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                }
                resultList.Add(row);
            }

            return resultList;
        }

    }
}
