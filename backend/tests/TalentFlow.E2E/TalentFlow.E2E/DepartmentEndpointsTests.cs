using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using TalentFlow.API.Controllers.Department.Request;
using TalentFlow.Domain.DTO.Department;
using Xunit;

namespace TalentFlow.E2E;

public class DepartmentEndpointsTests : IClassFixture<DatabaseFixture>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly ApplicationFactory _factory;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
    
    public DepartmentEndpointsTests(DatabaseFixture fixture)
    {
        _factory = new ApplicationFactory(fixture.DbContainer);
        _client = _factory.CreateClient();
    }
    
    public async Task InitializeAsync()
    {
        await _factory.InitializeDatabaseAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task CreateDepartment_ReturnsSuccessResult()
    {
        // Arrange
        var request = new CreateDepartmentRequest(name: "New Department", description: "ND");
        var content = CreateJsonContent(request);
        // Act
        var response =
            await _client.PostAsync("api/v1/endpoints/departments/", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK,
            $"Ожидается статус код 200 Ok но получили {response.StatusCode}. Response: {await response.Content.ReadAsStringAsync()}");
        
        var apiResponse =
            await DeserializeResponse<Guid>(response);
        
        apiResponse.Should().NotBeNull();
        apiResponse.Result.Should().NotBeEmpty();
        apiResponse.Result.Should().As<Guid>();
        apiResponse.Errors!.Should().BeNull();
    }

    [Fact]
    public async Task GetDepartments_ReturnsSuccessResult()
    {
        // Act
        var response =
            await _client.GetAsync("api/v1/endpoints/departments/");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK,
            $"Ожидается статус код 200 Ok, но получили {response.StatusCode}. Response: {await response.Content.ReadAsStringAsync()}");

        var apiResponse =
            await DeserializeResponse<IReadOnlyList<DepartmentGetDto>>(response);

        apiResponse.Should().NotBeNull();
        apiResponse.Result.Should().NotBeNullOrEmpty();
        apiResponse.Result.Should().AllBeOfType<DepartmentGetDto>();
        apiResponse.Errors.Should().BeNull();
    }
    
    private StringContent CreateJsonContent<T>(T data, JsonSerializerOptions? options = null)
    {
        return new StringContent(
            JsonSerializer.Serialize(data, options ?? new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
            Encoding.UTF8,
            "application/json");
    }

    private async Task<ApiResponse<T>> DeserializeResponse<T>(HttpResponseMessage response)
    {
        return JsonSerializer.Deserialize<ApiResponse<T>>(
            await response.Content.ReadAsStringAsync(),
            _jsonOptions) ?? throw new InvalidOperationException("Failed to deserialize response");
    }
}