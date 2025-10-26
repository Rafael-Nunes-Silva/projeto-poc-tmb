namespace api_poc_tmb.Services.Interfaces
{
    public interface IOpenAIService
    {
        public string GenerateSQLQuery(string userQuestion);

        public string GenerateFriendlyAnswer(string userQuestion, List<Dictionary<string, object>> queryResult);
    }
}
