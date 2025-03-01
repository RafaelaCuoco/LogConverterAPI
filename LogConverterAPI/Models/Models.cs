using System;
using System.ComponentModel.DataAnnotations;

namespace LogConverterAPI.Models
{
    public class LogOriginal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(int.MaxValue)] 
        public string Conteudo { get; set; } // Armazena o log no formato "MINHA CDN"
    }
    public class LogOriginalSalvar
    {
        [Required]
        [MaxLength(int.MaxValue)] 
        public string Conteudo { get; set; } // Armazena o log no formato "MINHA CDN"
    }

    public class LogTransformado
    {
        [Key]
        public int Id { get; set; }

        public int LogOriginalId { get; set; } // Referência ao log original

        [Required]
        [MaxLength(int.MaxValue)]
        public string Conteudo { get; set; } // Armazena o log no formato "Agora"
    }
    public class LogRequest
    {
        private string _conteudo;

        // Armazena o log no formato "MINHA CDN"
        [Required]
        [MaxLength(int.MaxValue)]
        public string Conteudo
        {
            get => _conteudo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("O conteúdo do log é obrigatório.");
                }
                _conteudo = value;
            }
        } 

        [Required]
        public bool SalvarNoServidor { get; set; } // iforma se tenta salvar no baco.

    }
}