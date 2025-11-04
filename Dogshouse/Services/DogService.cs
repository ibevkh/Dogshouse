using AutoMapper;
using Dogshouse.AppUnitOfWork;
using Dogshouse.AppUnitOfWork.Contracts;
using Dogshouse.DTOs;
using Dogshouse.Entities;
using Dogshouse.Exceptions;
using Dogshouse.Models;
using Dogshouse.Services.Contracts;
using Dogshouse.Validation;

namespace Dogshouse.Services;

public class DogService : IDogService
{
    private readonly IDogUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DogService(IDogUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponseDto<IEnumerable<DogDto>>> GetAllDogsAsync(QueryParameters parameters)
    {
        var dogs = await _unitOfWork.Dogs.GetAllAsync(parameters);

        if (dogs == null || !dogs.Any())
            throw new KeyNotFoundException("Собак не знайдено");

        return new BaseResponseDto<IEnumerable<DogDto>>
        {
            IsSucces = true,
            Data = _mapper.Map<IEnumerable<DogDto>>(dogs)
        };
    }

    public async Task<BaseResponseDto<DogDto>> AddDogAsync(CreateDogDto dto)
    {
        var validator = new CreateDogDtoValidator(); 
        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        if (await _unitOfWork.Dogs.ExistsByNameAsync(dto.Name))
        {
            throw new DuplicateDogNameException(dto.Name);
        }

        var dog = _mapper.Map<Dog>(dto);

        _unitOfWork.Dogs.Insert(dog);
        await _unitOfWork.SaveChangesAsync();

        return new BaseResponseDto<DogDto>
        {
            IsSucces = true,
            Data = _mapper.Map<DogDto>(dog)
        };
    }
}