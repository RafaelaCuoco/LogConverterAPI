using LogConverterAPI.Models;
using System;
using Xunit;

namespace LogConverterAPI.Tests.Models
{
    public class LogRequestTests
    {
        [Fact]
        public void LogRequest_ValidContent_ShouldSetProperties()
        {
            // Arrange
            var conteudo = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";
            var salvarNoServidor = true;

            // Act
            var logRequest = new LogRequest
            {
                Conteudo = conteudo,
                SalvarNoServidor = salvarNoServidor
            };

            // Assert
            Assert.Equal(conteudo, logRequest.Conteudo);
            Assert.True(logRequest.SalvarNoServidor);
        }

        [Fact]
        public void LogRequest_EmptyContent_ShouldThrowException()
        {
            // Arrange & Act
            var exception = Record.Exception(() => new LogRequest { Conteudo = null });

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal("O conteúdo do log é obrigatório.", exception.Message);
        }
    }
}