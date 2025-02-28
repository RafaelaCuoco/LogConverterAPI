using LogConverterAPI.Controllers;
using Newtonsoft.Json;
using System;
using System.IO;

namespace LogConverterAPI.Uteis
{
    public static class Uteis
    {
        public static void CriarDiretorioSeNaoExistir(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public static LogRequest TransformStringToLogRequest(string jsonString)
        {
            try
            {
                // Desserializa a string JSON para o objeto LogRequest
                var logRequest = JsonConvert.DeserializeObject<LogRequest>(jsonString);

                // Retorna o objeto LogRequest
                return logRequest;
            }
            catch (JsonException ex)
            {
                // Trata erros de desserialização
                Console.WriteLine($"Erro ao desserializar: {ex.Message}");
                return null;
            }
        }

    }
}