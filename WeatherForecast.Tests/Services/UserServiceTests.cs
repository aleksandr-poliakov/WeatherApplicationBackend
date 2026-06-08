using AutoMapper;

namespace WeatherForecast.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly IUserService _sut;

    public UserServiceTests()
    {
        _sut = new UserService(_repository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsAllMappedUsers()
    {
        var users = new List<User>
        {
            new() { Id = 1, Name = "Alice", Email = "alice@test.com", PasswordHash = "hash" },
            new() { Id = 2, Name = "Bob", Email = "bob@test.com", PasswordHash = "hash" }
        };
        var dtos = new List<UserResponseDto>
        {
            new() { Id = 1, Name = "Alice", Email = "alice@test.com" },
            new() { Id = 2, Name = "Bob", Email = "bob@test.com" }
        };

        _repository.Setup(r => r.GetAllUsersAsync()).ReturnsAsync(users);
        _mapper.Setup(m => m.Map<List<UserResponseDto>>(users)).Returns(dtos);

        var result = await _sut.GetAllUsersAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal("alice@test.com", result[0].Email);
        Assert.Equal("bob@test.com", result[1].Email);
    }

    [Fact]
    public async Task GetUserByEmailAsync_WhenUserExists_ReturnsMappedDto()
    {
        var user = new User { Id = 1, Name = "Alice", Email = "alice@test.com", PasswordHash = "hash" };
        var dto = new UserResponseDto { Id = 1, Name = "Alice", Email = "alice@test.com" };

        _repository.Setup(r => r.GetUserByEmailAsync("alice@test.com")).ReturnsAsync(user);
        _mapper.Setup(m => m.Map<UserResponseDto>(user)).Returns(dto);

        var result = await _sut.GetUserByEmailAsync("alice@test.com");

        Assert.NotNull(result);
        Assert.Equal("alice@test.com", result.Email);
    }

    [Fact]
    public async Task GetUserByEmailAsync_WhenUserNotFound_ReturnsNull()
    {
        _repository.Setup(r => r.GetUserByEmailAsync("missing@test.com")).ReturnsAsync((User?)null);

        var result = await _sut.GetUserByEmailAsync("missing@test.com");

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateUserAsync_WhenEmailAlreadyExists_ThrowsInvalidOperationException()
    {
        var existing = new User { Id = 1, Name = "Alice", Email = "alice@test.com", PasswordHash = "hash" };
        _repository.Setup(r => r.GetUserByEmailAsync("alice@test.com")).ReturnsAsync(existing);

        var dto = new UserCreateDto { Name = "Alice", Email = "alice@test.com", Password = "pass" };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateUserAsync(dto));
    }

    [Fact]
    public async Task CreateUserAsync_HashesPassword_BeforeSaving()
    {
        _repository.Setup(r => r.GetUserByEmailAsync("new@test.com")).ReturnsAsync((User?)null);

        var mappedUser = new User { Name = "New", Email = "new@test.com", PasswordHash = string.Empty };
        _mapper.Setup(m => m.Map<User>(It.IsAny<UserCreateDto>())).Returns(mappedUser);
        _mapper.Setup(m => m.Map<UserResponseDto>(mappedUser))
            .Returns(new UserResponseDto { Id = 0, Name = "New", Email = "new@test.com" });

        var dto = new UserCreateDto { Name = "New", Email = "new@test.com", Password = "secret" };

        await _sut.CreateUserAsync(dto);

        Assert.True(BCrypt.Net.BCrypt.Verify("secret", mappedUser.PasswordHash),
            "Password must be stored as BCrypt hash.");
    }

    [Fact]
    public async Task CreateUserAsync_CallsAddUserAsync_AndReturnsMappedDto()
    {
        _repository.Setup(r => r.GetUserByEmailAsync("new@test.com")).ReturnsAsync((User?)null);

        var mappedUser = new User { Name = "New", Email = "new@test.com", PasswordHash = string.Empty };
        var responseDto = new UserResponseDto { Id = 1, Name = "New", Email = "new@test.com" };

        _mapper.Setup(m => m.Map<User>(It.IsAny<UserCreateDto>())).Returns(mappedUser);
        _mapper.Setup(m => m.Map<UserResponseDto>(mappedUser)).Returns(responseDto);

        var dto = new UserCreateDto { Name = "New", Email = "new@test.com", Password = "secret" };

        var result = await _sut.CreateUserAsync(dto);

        _repository.Verify(r => r.AddUserAsync(mappedUser), Times.Once);
        Assert.Equal("new@test.com", result.Email);
    }
}
