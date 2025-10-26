using api_poc_tmb.Models;
using api_poc_tmb.Services.Interfaces;
using Azure;
using Microsoft.Azure.Amqp.Framing;
using OpenAI;
using OpenAI.Responses;
using System;
using System.Security.AccessControl;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace api_poc_tmb.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly string _apiKey;
        private readonly string _model;

        public OpenAIService(IConfiguration configuration)
        {
            _apiKey = configuration.GetSection("OpenAPI").GetValue<string>("ApiKey")!;
            _model = configuration.GetSection("OpenAPI").GetValue<string>("ModelName")!;
        }

        public string GenerateSQLQuery(string userQuestion)
        {
            string query = $@"Você é um expert em SQL, com foco em PostgreSQL.
            Tendo em consideração a estrutura a seguir:

            Table: orders
            - Id (int)
            - Cliente (string)
            - Produto (string)
            - Valor (float)
            - Status (enum: Pendente (0), Processando (1), Finalizado (2))
            - Data_criacao (datetime)

            Table: orderStatusHistories
            - Id (int)
            - OrderId (int)
            - StatusAntigo (enum: Pendente (0), Processando (1), Finalizado (2))
            - StatusNovo (enum: Pendente (0), Processando (1), Finalizado (2))
            - DataAlteracao (datetime)

            Regras importantes:
            - Sempre utilize aspas duplas para os nomes das tabelas e das colunas.

            Escreva uma query que responda a seguinte pergunta:
            ""{userQuestion}""

            Retorne apenas o código SQL.";

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            OpenAIResponseClient client = new(
                model: _model,
                apiKey: _apiKey
            );

            OpenAIResponse response = client.CreateResponse(query);
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            var rawReturn = response.GetOutputText();
            return Regex.Replace(rawReturn, @"```sql|```", "").Trim();
        }

        public string GenerateFriendlyAnswer(string userQuestion, List<Dictionary<string, object>> queryResult)
        {
            var jsonResult = JsonSerializer.Serialize(queryResult, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var prompt = $@"Você é um assistente amigável que responde perguntas baseadas em dados.
            A pergunta do usuário é: ""{userQuestion}""

            O resultado da consulta SQL retornou os seguintes dados em JSON:
            {jsonResult}

            Com base nisso, forneça uma resposta clara e resumida em português para o usuário.";

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            OpenAIResponseClient client = new(
                model: _model,
                apiKey: _apiKey
            );

            OpenAIResponse response = client.CreateResponse(prompt);
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            var rawReturn = response.GetOutputText();

            return rawReturn;
        }
    }
}
