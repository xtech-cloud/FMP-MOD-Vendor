
using XTC.FMP.MOD.Vendor.App.Service;

/// <summary>
/// 测试上下文，用于共享测试资源
/// </summary>
public class TestFixture : TestFixtureBase
{
    private SingletonServices singletonServices_;
    public TestFixture()
        : base()
    {
        singletonServices_ = new SingletonServices(new DatabaseOptions(), new MinIOOptions());
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    protected override void newBlazorService()
    {
        serviceBlazor_ = new BlazorService(singletonServices_);
    }

    protected override void newHealthyService()
    {
        serviceHealthy_ = new HealthyService();
    }

    protected override void newUnityService()
    {
        serviceUnity_ = new UnityService(singletonServices_);
    }
}
