using System.ComponentModel.DataAnnotations;

namespace LogConverterAPI.Models
{
    public class LogOriginal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(int.MaxValue)] // Define um limite máximo para o campo Conteudo
        public string Conteudo { get; set; } // Armazena o log no formato "MINHA CDN"
    }
    public class LogOriginalSalvar
    {
        [Required]
        [MaxLength(int.MaxValue)] // Define um limite máximo para o campo Conteudo
        public string Conteudo { get; set; } // Armazena o log no formato "MINHA CDN"
    }

    public class LogTransformado
    {
        [Key]
        public int Id { get; set; }

        public int LogOriginalId { get; set; } // Referência ao log original

        [Required]
        [MaxLength(int.MaxValue)] // Define um limite máximo para o campo Conteudo
        public string Conteudo { get; set; }
    }
}