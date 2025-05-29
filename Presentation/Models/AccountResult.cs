namespace Presentation.Models;

public class AccountResult
{
    public bool Succeeded { get; set; }
    public int StatusCode { get; set; }
    public string? Error { get; set; }
}

public class AccountResult<T> : AccountResult
{
    public T? Result { get; set; }
}

