using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;

using BusinessLogic.Options;
using BusinessLogic.Services;
using Data.Data.EF.Context;
using Vi.Service.Exception.Models;

namespace BusinessLogicTest;
//from https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.testhost.testserver?view=aspnetcore-9.0

// <snippet1>
public class BasicTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Theory]
    [InlineData(5)]
    public async Task TestThrowErrors(int data)
    {
        var mockMessageHandler = new Mock<HttpMessageHandler>();
        var mockContext = new Mock<NorthwindContext>();
        var options = new Mock<IOptions<BusinessOptions>>();
        var service = new BusinessServices(mockContext.Object, options.Object);
        var apiRestCall = await service.ThrowErrors(data);
        Assert.Throws<System.Exception>(() => { });
    }

    [Theory]
    [InlineData("Api/v1/GetData")]
    public async Task GetDataHttpResponseOK(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }

    [Theory]
    [InlineData("Api/v1/ThrowErrors/5")]
    public async Task ThrowErrorsResponseKO(string url)
    {
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var typedContent = JsonConvert.DeserializeObject<DomainDataError>(content);

        Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
    }

    [Theory]
    [InlineData("Api/v1/ThrowErrors/5")]
    public async Task ThrowsErrorsData(string url)
    {
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var typedContent = JsonConvert.DeserializeObject<DomainDataError>(content);

        var kv1 = new KeyValuePair<string, object?>(
            BusinessLogic.DomainDataError.BusinessError.BusinessMatErrors.CategoryAlreadyExists.Item1.ToString(),
            BusinessLogic.DomainDataError.BusinessError.BusinessMatErrors.CategoryAlreadyExists.Item2);
        Assert.True(typedContent.Details.Contains(kv1));
        Assert.True(typedContent.Details.Contains(new KeyValuePair<string, object?>(
            BusinessLogic.DomainDataError.BusinessError.BusinessMatErrors.CategoryAlreadyExists2.Item1.ToString(),
            BusinessLogic.DomainDataError.BusinessError.BusinessMatErrors.CategoryAlreadyExists2.Item2)));
    }


}  
