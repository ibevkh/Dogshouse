namespace Dogshouse.DTOs;

public class BaseResponseDto<T>
{
    public bool IsSucces { get; set; }
    public T Data { get; set; }
    public ProblemDetailsDto ProblemDetails { get; set; }
}
