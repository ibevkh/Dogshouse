using Microsoft.AspNetCore.Mvc;

namespace Dogshouse.DTOs;

public class ProblemDetailsDto : ProblemDetails
{
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}
