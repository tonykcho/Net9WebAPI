namespace Net9WebAPI.Tests.FunctionalTest;

[Collection(nameof(WebApplicationFixtureCollection))]
public class IntegrationTestBase
{
    public IntegrationTestBase(WebApplicationFixture webApplicationFixture)
    {
        WebApplicationFixture = webApplicationFixture;
    }

    public WebApplicationFixture WebApplicationFixture { get; set; }

    public HttpClient Client => WebApplicationFixture.Client;
}