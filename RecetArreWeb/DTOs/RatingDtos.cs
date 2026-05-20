using System.ComponentModel.DataAnnotations;

namespace RecetArreWeb.DTOs
{
    public class RatingDTO
    {
        public int Id { get; set; }
        public float Estrellas { get; set; } = 0;
        public int RecetaId { get; set; }
        public string UsuarioId { get; set; } = default!;
    }

    public class RatingCreacionDTO
    {
        [Required]
        [Range(0, 5)]
        public float Estrellas { get; set; } = 0;
        [Required]
        public int RecetaId { get; set; }
        [Required]
        public string UsuarioId { get; set; } = default!;
    }
}
