using AutoMapper;
using Dogshouse.AppUnitOfWork;
using Dogshouse.AppUnitOfWork.Contracts;
using Dogshouse.DTOs;
using Dogshouse.Entities;
using Dogshouse.Exceptions;
using Dogshouse.Models;
using Dogshouse.Repositories;
using Dogshouse.Repositories.Contracts;
using Dogshouse.Services;
using Moq;
using URF.Core.Abstractions;
using URF.Core.EF;

namespace DogsHouse.Tests;

public class DogServiceTests
{
    private readonly Mock<IDogUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DogService _service;

    public DogServiceTests()
    {
        _mockUnitOfWork = new Mock<IDogUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _service = new DogService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllDogsAsync_ShouldReturnSuccess_WhenDogsExist()
    {
        // Arrange
        var queryParameters = new QueryParameters();

        var dogs = new List<Dog>
        {
            new Dog { Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 },
            new Dog { Name = "Jessy", Color = "black&white", TailLength = 7, Weight = 14 }
        };

        var dogDtos = new List<DogDto>
        {
            new DogDto { Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 },
            new DogDto { Name = "Jessy", Color = "black&white", TailLength = 7, Weight = 14 }
        };

        var mockDogRepo = new Mock<IDogRepository>();
        mockDogRepo.Setup(r => r.GetAllAsync(queryParameters)).ReturnsAsync(dogs);

        _mockUnitOfWork.Setup(u => u.Dogs).Returns(mockDogRepo.Object);

        _mockMapper.Setup(m => m.Map<IEnumerable<DogDto>>(dogs)).Returns(dogDtos);

        // Act
        var result = await _service.GetAllDogsAsync(queryParameters);

        // Assert
        Assert.True(result.IsSucces);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count());
        Assert.Equal("Neo", result.Data.First().Name);

        mockDogRepo.Verify(r => r.GetAllAsync(queryParameters), Times.Once);
    }

    [Fact]
    public async Task AddDogAsync_ShouldReturnSuccess_WhenNameIsUnique()
    {
        // Arrange
        var createDto = new CreateDogDto { Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 };
        var dogEntity = new Dog { Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 };
        var dogDto = new DogDto { Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 };

        _mockUnitOfWork.Setup(u => u.Dogs.ExistsByNameAsync("Neo")).ReturnsAsync(false);
        _mockMapper.Setup(m => m.Map<Dog>(createDto)).Returns(dogEntity);
        _mockMapper.Setup(m => m.Map<DogDto>(dogEntity)).Returns(dogDto);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        var service = new DogService(_mockUnitOfWork.Object, _mockMapper.Object);

        // Act
        var result = await service.AddDogAsync(createDto);

        // Assert
        Assert.True(result.IsSucces);
        Assert.Equal("Neo", result.Data.Name);
    }

    [Fact]
    public async Task AddDogAsync_ShouldThrowException_WhenNameAlreadyExists()
    {
        // Arrange
        var createDto = new CreateDogDto { Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 };
        _mockUnitOfWork.Setup(u => u.Dogs.ExistsByNameAsync("Neo")).ReturnsAsync(true);

        var service = new DogService(_mockUnitOfWork.Object, _mockMapper.Object);

        // Act + Assert
        await Assert.ThrowsAsync<DuplicateDogNameException>(() => service.AddDogAsync(createDto));
    }

    [Fact]
    public async Task AddDogAsync_ShouldThrowValidationException_WhenInvalidDataProvided()
    {
        // Arrange
        var invalidDto = new CreateDogDto
        {
            Name = "",             
            Color = "red&amber",
            TailLength = -3,       
            Weight = 10
        };

        var service = new DogService(_mockUnitOfWork.Object, _mockMapper.Object);

        // Act + Assert
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
            service.AddDogAsync(invalidDto));
    }
}