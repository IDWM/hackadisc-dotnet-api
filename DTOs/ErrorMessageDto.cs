namespace hackadisc_dotnet_api.DTOs;

/// <summary>
/// DTO para mensajes de error.
/// </summary>
public class ErrorMessageDto
{
    /// <summary>
    /// Mensaje de error.
    /// </summary>
    public required string Error { get; set; }
}
