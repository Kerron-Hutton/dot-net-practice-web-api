namespace SampleAppTests.Integration;

[CollectionDefinition(nameof(SharedTestCollection))]
public class SharedTestCollection : ICollectionFixture<CustomWebApplicationFactory>
{
}