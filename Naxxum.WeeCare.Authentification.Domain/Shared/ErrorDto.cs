using Humanizer;
using Naxxum.WeeCare.Authentification.Domain.Enums;

namespace Naxxum.WeeCare.Authentification.Domain.Shared;

public class ErrorDto
{
    public int Code { get; init; }
    public string Message { get; init; }

    public ErrorDto(int code, string message)
    {
        Code = code;
        Message = message;
    }

    public static implicit operator ErrorDto(Enum error) => new(Convert.ToInt32(error), error.ToString().Humanize());
    public static implicit operator DomainErrors(ErrorDto error) => (DomainErrors)error.Code;
}