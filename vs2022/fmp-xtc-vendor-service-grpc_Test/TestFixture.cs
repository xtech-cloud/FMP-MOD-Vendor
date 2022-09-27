
using XTC.FMP.MOD.Vendor.App.Service;

/// <summary>
/// 测试上下文，用于共享测试资源
/// </summary>
public class TestFixture : TestFixtureBase
{
    public TestFixture()
        : base()
    {
    }

    public override void Dispose()
    {
        base.Dispose();
    }


    protected override void newHealthyService()
    {
        serviceHealthy_ = new HealthyService();
    }

    protected override void newUnityService()
    {
        serviceUnity_ = new UnityService(new UnityDAO(new DatabaseOptions()));
    }
}
