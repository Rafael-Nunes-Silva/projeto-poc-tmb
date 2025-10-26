namespace api_poc_tmb.Services.Interfaces
{
    public interface ILLMSqlService
    {
        Task<List<Dictionary<string, object>>> ExecuteDynamicSqlAsync(string sql);
    }
}
