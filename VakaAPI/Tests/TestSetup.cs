using NUnit.Framework;

[SetUpFixture]
public class TestSetup
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", null);
    }
}
