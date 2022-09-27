
using XTC.FMP.MOD.Vendor.App.Service;

public static class MyProgram
{
    public static void PreBuild(WebApplicationBuilder? _builder)
    {
        _builder?.Services.AddSingleton<UnityDAO>();
    }

    public static void PreRun(WebApplication? _app)
    {
    }
}
