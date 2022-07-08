namespace Dogstagram.WebApi.IntegrationTests
{
    using Dogstagram.WebApi.Features;
    using Dogstagram.WebApi.Features.Identity.Models;
    using Newtonsoft.Json;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;

    public class IdentityControllerTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory factory;

        public IdentityControllerTest(CustomWebApplicationFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task RegisterMethodShouldRegisterUserSuccessfully()
        {
            var client = this.factory.CreateClient();
            var registerRequestModel = new RegisterRequestModel
            {
                Email = "end13n@ndn.com",
                Password = "123456",
                UserName = "end13n",
                WithRole = false,
            };

            var stringContent = new StringContent(
                JsonConvert.SerializeObject(registerRequestModel),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/identity/register", stringContent);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task RegisterMethodShouldReturnBadRequestIfUserExist()
        {
            var client = this.factory.CreateClient();
            var registerRequestModel = new RegisterRequestModel
            {
                Email = "nikolay@ndn.com",
                Password = "123456",
                UserName = "nikolay",
                WithRole = false,
            };

            var stringContent = new StringContent(
                JsonConvert.SerializeObject(registerRequestModel),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/identity/register", stringContent);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("nikolay", "123456", HttpStatusCode.OK)]
        [InlineData("ivan", "123456", HttpStatusCode.NotFound)]
        [InlineData("nikolay", "654321", HttpStatusCode.Unauthorized)]
        [InlineData("nikolay1", "n1k0lay", HttpStatusCode.OK)]
        public async Task ReturnsExpectedResultByGivenCredentials(
            string username,
            string password,
            HttpStatusCode expectedStatusCode)
        {
            var reinitFactory = new CustomWebApplicationFactory();
            var client = reinitFactory.CreateClient();

            var loginRequestModel = new LoginRequestModel { UserName = username, Password = password };

            var stringContent = new StringContent(
                JsonConvert.SerializeObject(loginRequestModel),
                Encoding.UTF8,
                "application/json");

            await client.PostAsync("/identity/login", stringContent);
            var response = await client.PostAsync("/identity/login", stringContent);
            var content = await response.Content.ReadAsStringAsync();

            var authObj = JsonConvert.DeserializeObject<LoginResponseModel>(content);

            Assert.Equal(expectedStatusCode, response.StatusCode);
            if (expectedStatusCode == HttpStatusCode.OK)
            {
                Assert.NotNull(authObj.Token);
            }
        }

        [Theory]
        [InlineData("nikolay", HttpStatusCode.OK)]
        public async Task DeleteUserShouldReturnIsUserSuccessfullyDeleted(string username, HttpStatusCode expectedResult)
        {
            var client = this.factory.CreateClient();

            var loginRequestModel = new LoginRequestModel { UserName = username, Password = "123456" };

            var stringContent = new StringContent(
                JsonConvert.SerializeObject(loginRequestModel),
                Encoding.UTF8,
                "application/json");

            var responseLogin = await client.PostAsync("/identity/login", stringContent);
            var content = await responseLogin.Content.ReadAsStringAsync();
            var authObj = JsonConvert.DeserializeObject<LoginResponseModel>(content);

            stringContent = new StringContent(
                JsonConvert.SerializeObject(""),
                Encoding.UTF8,
                "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{authObj.Token}");
            var response = await client
                .SendAsync(
                    new HttpRequestMessage(HttpMethod.Delete, "/profile/delete")
                    {
                        Content = stringContent
                    });
            var contentResult = await response.Content.ReadAsStringAsync();
            var resultObj = JsonConvert.DeserializeObject<CommonResponseModel>(contentResult);

            Assert.Equal(expectedResult, response.StatusCode);
            Assert.Equal($"{username} Deleted!", resultObj.Message);
        }

        [Theory]
        [InlineData("nikolay1", "n1k0lay", HttpStatusCode.OK)]
        public async Task UndeletedEndPointShouldRecoverUser(string username, string password, HttpStatusCode expectedStatusCode)
        {
            var reinitFactory = new CustomWebApplicationFactory();
            var client = reinitFactory.CreateClient();

            var loginRequestModel = new LoginRequestModel
            {
                UserName = username,
                Password = password,
            };

            var stringContent = new StringContent(
                JsonConvert.SerializeObject(loginRequestModel),
                Encoding.UTF8,
                "application/json");
            
            var response = await client.PostAsync("/identity/login", stringContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultObj = JsonConvert.DeserializeObject<CommonResponseModel>(responseContent);
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal("Account has been restored", resultObj.Message);
        }
    }
}
