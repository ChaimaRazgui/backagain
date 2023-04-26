using System.Text.Json.Serialization;

namespace Naxxum.WeeCare.Authentification.Application.DTOs;

public record ErrorResultDto(int StatusCode, string Message,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? StackTrace = null);