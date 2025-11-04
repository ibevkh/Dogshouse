using Dogshouse.DTOs;
using Dogshouse.Models;

namespace Dogshouse.Services.Contracts;

public interface IDogService
{
    Task<BaseResponseDto<IEnumerable<DogDto>>> GetAllDogsAsync(QueryParameters parameters);
    Task<BaseResponseDto<DogDto>> AddDogAsync(CreateDogDto dto);
}
