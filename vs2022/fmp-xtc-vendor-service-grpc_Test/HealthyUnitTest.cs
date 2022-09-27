
using XTC.FMP.MOD.Vendor.LIB.Proto;

public class HealthyTest : HealthyUnitTestBase
{
    public HealthyTest(TestFixture _testFixture)
        : base(_testFixture)
    {
    }


    public override async Task EchoTest()
    {
        var request = new HealthyEchoRequest();
        request.Msg = "hello";
        var response = await fixture_.getServiceHealthy().Echo(request, fixture_.context);
        Assert.Equal(0, response.Status.Code);
        Assert.Equal("hello", response.Msg);
    }

}
