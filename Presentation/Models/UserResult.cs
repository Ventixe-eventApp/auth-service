namespace Presentation.Models;

public class UserResult<T>
{
    public bool Succeeded { get; set; }
    public T? Result { get; set; }
    public string? Error { get; set; }
}
