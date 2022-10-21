namespace RealEstate.Admin.Models;

public class ApiResponse
{
    public bool Success => Error == null;

    public ApiError Error { get; set; }

    public void SetError(string status)
    {
        SetError(status, status);
    }

    public void SetError(Exception exception)
    {
        SetError("Unhandled", exception.Message);
    }

    public void SetError(string status, string message)
    {
        Error = new ApiError(status, message);
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T Data { get; set; }
}

public class ApiError    
{
    public string Status { get; set; }
    public string Message { get; set; } 
    public List<string>? ErrorMessages { get; set; }
        
    public ApiError(string status, string message)
    {
        Status = status;
        Message = message;
    }
        
    public override string ToString()
    {
        return !string.IsNullOrEmpty(Message) ? Message : Status;
    }
}