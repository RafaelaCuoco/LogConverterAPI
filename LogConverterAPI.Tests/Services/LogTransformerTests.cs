using Xunit;
using LogConverterAPI.Services;
using System;

namespace LogConverterAPI.Tests.Services
{
    public class LogTransformerTests
    {
        private readonly LogTransformer _logTransformer;

        public LogTransformerTests()
        {
            _logTransformer = new LogTransformer();
        }

        [Fact]
        public void TransformarLog_ValidInput_ShouldReturnTransformedContent()
        {
            // Arrange
            var input = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";

            // Act
            var result = _logTransformer.TransformarLog(input);

            // Assert
            Assert.Contains("#Version: 1.0", result);
            Assert.Contains("\"MINHA CDN\" GET 200 /robots.txt 1002 312 HIT", result);
        }

        [Fact]
        public void TransformarLog_InvalidInput_ShouldHandleGracefully()
        {
            // Arrange
            var invalidInput = ""; // Entrada inválida

            // Act
            var exception = Record.Exception(() => _logTransformer.TransformarLog(invalidInput));

            // Assert
            Assert.NotNull(exception); // Verifica se uma exceção foi lançada
            Assert.IsType<ArgumentException>(exception); // Verifica o tipo da exceção
            Assert.Equal("O conteúdo do log não pode ser nulo ou vazio.", exception.Message); // Verifica a mensagem
        }
    }
}