
using XTC.FMP.MOD.Vendor.App.Service;

public static class MyProgram
{
    public static void PreBuild(WebApplicationBuilder? _builder)
    {
        _builder?.Services.Configure<MinIOSettings>(_builder.Configuration.GetSection("MinIO"));
        _builder?.Services.AddSingleton<UnityDAO>();
        _builder?.Services.AddSingleton<BlazorDAO>();
        _builder?.Services.AddSingleton<MinIOClient>();
    }

    public static void PreRun(WebApplication? _app)
    {
    }
}
