using AutoMapper;
using Dogshouse.DTOs;
using Dogshouse.Entities;

namespace Dogshouse.Mapping;

public class DogMapping : Profile
{
    public DogMapping()
    {
        CreateMap<CreateDogDto, Dog>();

        CreateMap<Dog, DogDto>();
    }
}
