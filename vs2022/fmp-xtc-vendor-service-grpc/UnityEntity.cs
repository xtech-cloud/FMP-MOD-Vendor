namespace XTC.FMP.MOD.Vendor.App.Service
{
    public class UnityEntity : Entity
    {
        public string Name { get; set; } = "";
        public string Display { get; set; } = "";
        public string SkinSplashBackground { get; set; } = "";
        public string SkinSplashSlogan { get; set; } = "";
        public int GraphicsFPS { get; set; }
        public int GraphicsQuality { get; set; }
        public string GraphicsPixelResolution { get; set; } = "";
        public int GraphicsReferenceResolutionWidth { get; set; }
        public int GraphicsReferenceResolutionHeight { get; set; }
        public float GraphicsReferenceResolutionMatch { get; set; }
        public string Application { get; set; } = "";
        public string DependencyConfig { get; set; } = "";
        public string BootloaderConfig { get; set; } = "";
        public string UpdateConfig { get; set; } = "";
        public Dictionary<string, string> ModuleConfigS { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ModuleCatalogS { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, FileSubEntityS> ModuleThemeS { get; set; } = new Dictionary<string, FileSubEntityS>();
    }
}
