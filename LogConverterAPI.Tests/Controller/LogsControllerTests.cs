using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogConverterAPI.Controllers;
using LogConverterAPI.Models;
using LogConverterAPI.Services;
using LogConverterAPI.Data;
using System.Threading.Tasks;

namespace LogConverterAPI.Tests.Controllers
{
    public class LogsControllerTests
    {
        private readonly Mock<LogContext> _mockLogContext;
        private readonly Mock<LogTransformerService> _mockLogTransformerService;
        private readonly Mock<ILogTransformer> _mockILogTransformer;
        private readonly LogsController _controller;

        public LogsControllerTests()
        {
            // Cria mocks para as dependências
            _mockLogContext = new Mock<LogContext>(new DbContextOptions<LogContext>());
            _mockLogTransformerService = new Mock<LogTransformerService>();
            _mockILogTransformer = new Mock<ILogTransformer>();

            // Configura o comportamento do mock de ILogTransformer
            _mockILogTransformer.Setup(x => x.TransformarLog(It.IsAny<string>()))
                                .Returns("#Version: 1.0\n#Date: ...");

            // Cria o controlador com as dependências mockadas
            _controller = new LogsController(
                _mockLogContext.Object,
                _mockLogTransformerService.Object,
                _mockILogTransformer.Object
            );
        }

        [Fact]
        public async Task TransformarLog_ValidRequest_ShouldReturnOk()
        {
            // Arrange
            var request = new LogRequest
            {
                Conteudo = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2",
                SalvarNoServidor = false
            };

            // Act
            var result = await _controller.TransformarLog(request); // Aguarda a conclusão da tarefa

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }
    }
}