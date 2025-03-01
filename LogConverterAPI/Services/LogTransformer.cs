using System;
using System.Collections.Generic;
using System.Linq;

namespace LogConverterAPI.Services
{
    public class LogTransformer : ILogTransformer
    {
        public string TransformarLog(string conteudo)
        {
            if (string.IsNullOrWhiteSpace(conteudo))
            {
                throw new ArgumentException("O conteúdo do log não pode ser nulo ou vazio.");
            }

            var linhas = conteudo.Split('\n');
            var resultado = new List<string>
            {
                "#Version: 1.0",
                $"#Date: {DateTime.Now:dd/MM/yyyy HH:mm:ss}",
                "#Fields: provider http-method status-code uri-path time-taken response-size cache-status"
            };

            foreach (var linha in linhas)
            {
                if (string.IsNullOrWhiteSpace(linha)) continue;

                var partes = linha.Split('|');
                if (partes.Length != 5) continue; // Ignora linhas inválidas

                var tamanhoResposta = partes[0];
                var statusCode = partes[1];
                var cacheStatus = partes[2].ToUpper() == "INVALIDATE" ? "REFRESH_HIT" : partes[2];
                var requisicao = partes[3].Trim('"').Split(' ');
                var metodoHttp = requisicao[0];
                var caminhoUri = requisicao[1];
                var tempoResposta = Math.Round(double.Parse(partes[4]));

                resultado.Add($"\"MINHA CDN\" {metodoHttp} {statusCode} {caminhoUri} {tempoResposta} {tamanhoResposta} {cacheStatus}");
            }

            return string.Join("\n", resultado);
        }
    }
}