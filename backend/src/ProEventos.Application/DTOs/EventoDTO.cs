using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.DTOs
{
    public class EventoDTO
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string Data { get; set; }

        [Required(ErrorMessage = "O campo {0} � obrigat�rio."),
         MinLength(4, ErrorMessage = "{0} deve ter no m�nimo 4 caracteres."),
         MaxLength(50, ErrorMessage = "{0} deve ter no m�ximo 50 caracteres.")]
        public string Tema { get; set; }
        public int QtdPessoas { get; set; }
        public string ImagemURL { get; set; }
        public string Telefone { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "O campo {0} � obrigat�rio")]
        [EmailAddress(ErrorMessage = "O campo {0} precisa ser um e-mail v�lido.")]
        public string Email { get; set; }
        public IEnumerable<LoteDTO> Lotes { get; set; }
        public IEnumerable<RedeSocialDTO> RedesSociais { get; set; }
        public IEnumerable<PalestranteDTO> Palestrantes { get; set; }
    }
}