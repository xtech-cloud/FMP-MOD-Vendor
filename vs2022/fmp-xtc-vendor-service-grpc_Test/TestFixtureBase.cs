
//*************************************************************************************
//   !!! Generated by the fmp-cli 1.70.0.  DO NOT EDIT!
//*************************************************************************************

using XTC.FMP.MOD.Vendor.App.Service;

public abstract class TestFixtureBase : IDisposable
{
    public TestServerCallContext context { get; set; }

    public TestFixtureBase()
    {
        context = TestServerCallContext.Create();
    }

    public virtual void Dispose()
    {

        var options = new DatabaseOptions();
        var mongoClient = new MongoDB.Driver.MongoClient(options.Value.ConnectionString);
        mongoClient.DropDatabase(options.Value.DatabaseName);

    }


    protected BlazorService? serviceBlazor_ { get; set; }

    public BlazorService getServiceBlazor()
    {
        if(null == serviceBlazor_)
        {
            newBlazorService();
        }
        return serviceBlazor_!;
    }

    /// <summary>
    /// 实例化服务
    /// </summary>
    /// <example>
    /// serviceBlazor_ = new BlazorService(new BlazorDAO(new DatabaseOptions()));
    /// </example>
    protected abstract void newBlazorService();

    protected HealthyService? serviceHealthy_ { get; set; }

    public HealthyService getServiceHealthy()
    {
        if(null == serviceHealthy_)
        {
            newHealthyService();
        }
        return serviceHealthy_!;
    }

    /// <summary>
    /// 实例化服务
    /// </summary>
    /// <example>
    /// serviceHealthy_ = new HealthyService(new HealthyDAO(new DatabaseOptions()));
    /// </example>
    protected abstract void newHealthyService();

    protected UnityService? serviceUnity_ { get; set; }

    public UnityService getServiceUnity()
    {
        if(null == serviceUnity_)
        {
            newUnityService();
        }
        return serviceUnity_!;
    }

    /// <summary>
    /// 实例化服务
    /// </summary>
    /// <example>
    /// serviceUnity_ = new UnityService(new UnityDAO(new DatabaseOptions()));
    /// </example>
    protected abstract void newUnityService();

}

