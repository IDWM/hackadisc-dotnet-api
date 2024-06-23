using System.ComponentModel.DataAnnotations;

namespace hackadisc_dotnet_api.DTOs;

/// <summary>
/// DTO para la creación de un integrante.
/// </summary>
public class CreateMemberDto
{
    /// <summary>
    /// Nombre del integrante.
    /// </summary>
    [Required(ErrorMessage = "El nombre es requerido.")]
    [StringLength(
        20,
        MinimumLength = 3,
        ErrorMessage = "El nombre debe tener entre 3 y 20 caracteres."
    )]
    public required string Name { get; set; }

    /// <summary>
    /// Correo electrónico del integrante.
    /// </summary>
    [Required(ErrorMessage = "El correo electrónico es requerido.")]
    [EmailAddress(ErrorMessage = "Correo electrónico inválido.")]
    public required string Email { get; set; }

    /// <summary>
    /// Número de semestre del integrante.
    /// </summary>
    [Required(ErrorMessage = "El semestre es requerido.")]
    [Range(1, 10, ErrorMessage = "El semestre debe estar entre 1 y 10.")]
    public required int Semester { get; set; }

    /// <summary>
    /// Carrera del integrante.
    /// </summary>
    [Required(ErrorMessage = "La carrera es requerida.")]
    [StringLength(
        50,
        MinimumLength = 3,
        ErrorMessage = "La carrera debe tener entre 3 y 50 caracteres."
    )]
    public required string Career { get; set; }
}
