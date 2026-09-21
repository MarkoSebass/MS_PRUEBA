using System.ComponentModel.DataAnnotations;

namespace MsPrueba.Models;

public class Tarea
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(120, ErrorMessage = "El título no puede superar los 120 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
    public string? Descripcion { get; set; }

    public bool Completada { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}