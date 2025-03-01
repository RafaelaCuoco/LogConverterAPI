using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogConverterAPI.Data;
using LogConverterAPI.Models;
using LogConverterAPI.Services;
using LogConverterAPI.Uteis;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace LogConverterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly LogContext _context;
        private readonly LogTransformerService _logTransformer;
        private string path = Path.Combine(Directory.GetCurrentDirectory(), 
            "logs", 
            DateTime.Now.ToString("yyyy"), 
            DateTime.Now.ToString("MM"));

        public LogsController(LogContext context, LogTransformerService logTransformer)
        {
            _context = context;
            _logTransformer = logTransformer;
        }

        // POST api/logs/transformar

        /// <summary>
        /// Transforma logs do formato "MINHA CDN" para o formato "Agora".
        /// </summary>
        /// <param name="request">Os dados do log a serem transformados.</param>
        /// <returns>O log transformado no formato "Agora".</returns>
        /// <response code="200">Retorna os logs Transformados com sucesso.</response>
        /// <response code="400">Se os logs fornecidos forem inválidos.</response>
        /// <response code="409">Retorna os logs ja existentes no banco.</response>
        /// <response code="500">Retorna o erro em salvar os logs no banco.</response>
        [HttpPost("transformar")]
        public async Task<IActionResult> TransformarLog([FromBody] LogRequest request)
        {
            // Validação básica do conteúdo do log
            if (request == null || string.IsNullOrWhiteSpace(request.Conteudo))
            {
                return BadRequest("O conteúdo do log é obrigatório.");
            }

            // Transforma o log para o formato "Agora"
            var logTransformado = _logTransformer.TransformarLog(request.Conteudo);
            Uteis.Uteis.CriarDiretorioSeNaoExistir(path); // Garante que o diretório existe
            var filePath = Path.Combine(path, $"{Guid.NewGuid()}.log");

            // Salva o log transformado em um arquivo, se necessário
            if (request.SalvarNoServidor)
            {

                // Verifica se o log já existe no banco de dados
                var logExistente = await _context.LogsOriginais.FirstOrDefaultAsync(l => l.Conteudo == request.Conteudo);

                if (logExistente != null)
                {
                    // Retorna o log existente com status HTTP 409 Conflict
                    await System.IO.File.WriteAllTextAsync(filePath, logTransformado);
                    return Conflict(new { Log = logTransformado, FilePath = filePath, Warning = "Os dados já constam no banco de dados." });
                }

                // Se o log não existe, cria um novo registro no banco de dados
                LogOriginal log = new LogOriginal { Conteudo = request.Conteudo };
                _context.LogsOriginais.Add(log);

                try
                {
                    // Salva o log original no banco de dados
                    await _context.SaveChangesAsync();


                    // Salva o log transformado no banco de dados
                    var logE = await _context.LogsOriginais.FirstOrDefaultAsync(l => l.Conteudo == request.Conteudo);
                    var logTransformadoEntity = new LogTransformado
                    {
                        LogOriginalId = logE.Id,
                        Conteudo = logTransformado
                    };

                    _context.LogsTransformados.Add(logTransformadoEntity);
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    // Em caso de erro ao salvar no banco de dados, retorna um erro interno
                    return StatusCode(500, $"Erro ao salvar o log no banco de dados: {ex.Message}");
                }

                try
                {

                    await System.IO.File.WriteAllTextAsync(filePath, logTransformado);

                    // Retorna o caminho do arquivo salvo junto com o log
                    return Ok(new { Log = log, FilePath = filePath });
                }
                catch (Exception ex)
                {
                    // Em caso de erro ao salvar o arquivo, retorna um erro interno
                    return StatusCode(500, $"Erro ao salvar o arquivo: {ex.Message}");
                }
            }
            else
            {

                // Retorna o caminho do arquivo salvo junto com o log
                return Ok(new { Log = logTransformado, FilePath = filePath });
            }
        }

        // GET api/logs/salvos

        /// <summary>
        /// Retorna uma lista de todos os logs originais salvos no banco de dados.
        /// </summary>
        /// <returns>Uma lista de objetos LogOriginal contendo os logs salvos.</returns>
        /// <response code="200">Retorna a lista de logs salvos com sucesso.</response>
        [HttpGet("salvos")]
        public async Task<IActionResult> GetLogsSalvos()
        {
            return Ok(await _context.LogsOriginais.ToListAsync());
        }

        // GET api/logs/transformados

        /// <summary>
        /// Retorna uma lista de todos os logs transformados salvos no banco de dados.
        /// </summary>
        /// <returns>Uma lista de objetos LogTransformado contendo os logs transformados.</returns>
        /// <response code="200">Retorna a lista de logs transformados com sucesso.</response>
        [HttpGet("transformados")]
        public async Task<IActionResult> GetLogsTransformados()
        {
            return Ok(await _context.LogsTransformados.ToListAsync());
        }

        // GET api/logs/salvos/{id}

        /// <summary>
        /// Retorna um log original específico com base no ID fornecido.
        /// </summary>
        /// <param name="id">O ID do log original a ser buscado.</param>
        /// <returns>O objeto LogOriginal correspondente ao ID fornecido.</returns>
        /// <response code="200">Retorna o log original com sucesso.</response>
        /// <response code="404">Se o log original com o ID especificado não for encontrado.</response>
        [HttpGet("salvos/{id}")]
        public async Task<IActionResult> GetLogSalvo(int id)
        {
            var log = await _context.LogsOriginais.FindAsync(id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        // GET api/logs/transformados/{id}

        /// <summary>
        /// Retorna um log transformado específico com base no ID fornecido.
        /// </summary>
        /// <param name="id">O ID do log transformado a ser buscado.</param>
        /// <returns>O objeto LogTransformado correspondente ao ID fornecido.</returns>
        /// <response code="200">Retorna o log transformado com sucesso.</response>
        /// <response code="404">Se o log transformado com o ID especificado não for encontrado.</response>
        [HttpGet("transformados/{id}")]
        public async Task<IActionResult> GetLogTransformado(int id)
        {
            var log = await _context.LogsTransformados.FindAsync(id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        // POST api/logs/salvar

        /// <summary>
        /// Salva um novo log original no banco de dados.
        /// </summary>
        /// <param name="log">O objeto LogOriginal contendo os dados do log a ser salvo.</param>
        /// <returns>O objeto LogOriginal recém-criado, incluindo o ID gerado.</returns>
        /// <response code="200">Retorna os logs originais criados com sucesso.</response>
        /// <response code="400">Retorna o erro se os logs fornecidos forem inválidos.</response>
        /// <response code="409">Retorna os logs ja existentes no banco.</response>
        /// <response code="500">Retorna o erro em salvar os logs no banco.</response>
        [HttpPost("salvar")]
        public async Task<IActionResult> SalvarLog([FromBody] LogOriginalSalvar log)
        {
            if (log == null || string.IsNullOrWhiteSpace(log.Conteudo))
            {
                return BadRequest("O conteúdo do log é obrigatório.");
            }

            // Verifica se o log já existe no banco de dados
            var logExistente = await _context.LogsOriginais
                .FirstOrDefaultAsync(l => l.Conteudo == log.Conteudo);

            if (logExistente != null)
            {
                // Retorna o log existente com status HTTP 409 Conflict
                return Conflict(new { Log = logExistente, Warning = "Os dados já constam no banco de dados." });
            }

            // Salva o novo log no banco de dados
            LogOriginal logOri = new LogOriginal { Conteudo = log.Conteudo };
            _context.LogsOriginais.Add(logOri);
            try
            {
                // Salva o log original no banco de dados
                await _context.SaveChangesAsync();


                // Salva o log transformado no banco de dados
                var logE = await _context.LogsOriginais.FirstOrDefaultAsync(l => l.Conteudo == logOri.Conteudo);
                var logTransformado = _logTransformer.TransformarLog(logOri.Conteudo);
                var logTransformadoEntity = new LogTransformado
                {
                    LogOriginalId = logE.Id,
                    Conteudo = logTransformado
                };

                _context.LogsTransformados.Add(logTransformadoEntity);
                await _context.SaveChangesAsync();
                return Ok(new { Log = logE });


            }
            catch (Exception ex)
            {
                // Em caso de erro ao salvar no banco de dados, retorna um erro interno
                return StatusCode(500, $"Erro ao salvar o log no banco de dados: {ex.Message}");
            }
        }
    }
}