namespace WeatherForecast.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserService> _service = new();
    private readonly UserController _sut;

    public UserControllerTests()
    {
        _sut = new UserController(_service.Object);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsOkWithList()
    {
        var users = new List<UserResponseDto>
        {
            new() { Id = 1, Name = "Alice", Email = "alice@test.com" },
            new() { Id = 2, Name = "Bob", Email = "bob@test.com" }
        };
        _service.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

        var result = await _sut.GetAllUsers();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var body = Assert.IsType<List<UserResponseDto>>(ok.Value);
        Assert.Equal(2, body.Count);
    }

    [Fact]
    public async Task GetUser_WhenFound_ReturnsOk()
    {
        var dto = new UserResponseDto { Id = 1, Name = "Alice", Email = "alice@test.com" };
        _service.Setup(s => s.GetUserByEmailAsync("alice@test.com")).ReturnsAsync(dto);

        var result = await _sut.GetUser("alice@test.com");

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var body = Assert.IsType<UserResponseDto>(ok.Value);
        Assert.Equal("alice@test.com", body.Email);
    }

    [Fact]
    public async Task GetUser_WhenNotFound_ReturnsNotFound()
    {
        _service.Setup(s => s.GetUserByEmailAsync("missing@test.com")).ReturnsAsync((UserResponseDto?)null);

        var result = await _sut.GetUser("missing@test.com");

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateUser_ReturnsCreated_WithLocationHeader()
    {
        var dto = new UserCreateDto { Name = "Alice", Email = "alice@test.com", Password = "pass" };
        var responseDto = new UserResponseDto { Id = 1, Name = "Alice", Email = "alice@test.com" };
        _service.Setup(s => s.CreateUserAsync(dto)).ReturnsAsync(responseDto);

        var result = await _sut.CreateUser(dto);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(UserController.GetUser), created.ActionName);
        Assert.Equal("alice@test.com", created.RouteValues!["email"]);
        Assert.Equal(responseDto, created.Value);
    }

    [Fact]
    public async Task CreateUser_WhenEmailTaken_ReturnsConflict()
    {
        var dto = new UserCreateDto { Name = "Alice", Email = "alice@test.com", Password = "pass" };
        _service.Setup(s => s.CreateUserAsync(dto))
            .ThrowsAsync(new InvalidOperationException("User with email alice@test.com already exists."));

        var result = await _sut.CreateUser(dto);

        var conflict = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.Contains("alice@test.com", conflict.Value!.ToString());
    }
}
