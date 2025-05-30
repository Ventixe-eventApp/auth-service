namespace Presentation.Models;

public class AuthResult
{
    public bool Succeeded { get; set; }
    public int StatusCode { get; set; }
    public string? Error { get; set; }
}

public class AuthResult<T> : AuthResult
{
    public T? Result { get; set; }
 
}
