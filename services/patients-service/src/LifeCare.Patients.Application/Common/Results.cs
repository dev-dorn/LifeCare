namespace LifeCare.Modules.Shared.Application.common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T Data { get; }
    public string Error { get; }

    private Result(bool isSuccess, T data, string error)
    {
        this.IsSuccess = isSuccess;
        this.Data = data;
        this.Error = error;
    }

    public static Result<T> Success(T data) => new(true, data, null);
    public static Result<T> Failure (string error) => new(false, default, error);

}

public class Result
{
    public bool IsSuccess { get; }
    public string Error { get; }

    private Result(bool isSuccess, string error)
    {
        this.IsSuccess = isSuccess;
        this.Error = error;
    }
    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);
}