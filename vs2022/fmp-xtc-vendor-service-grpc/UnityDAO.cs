
//*************************************************************************************
//   !!! Generated by the fmp-cli 1.39.0.  DO NOT EDIT!
//*************************************************************************************

using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace XTC.FMP.MOD.Vendor.App.Service
{
    public class UnityDAO : DAO<UnityEntity>
    {
        public UnityDAO(IOptions<DatabaseSettings> _settings)
        : base(_settings, "Unity")
        {
        }

        public UnityEntity NewDefaultUnity()
        {
            var unity = new UnityEntity();
            unity.Uuid = Guid.NewGuid();
            unity.Name = "";
            unity.Display = "";
            unity.SkinSplashBackground = "";
            unity.SkinSplashSlogan = "";
            unity.GraphicsFPS = 60;
            unity.GraphicsQuality = 3;
            unity.GraphicsPixelResolution = "auto";
            unity.GraphicsReferenceResolutionWidth = 1920;
            unity.GraphicsReferenceResolutionHeight = 1080;
            unity.GraphicsReferenceResolutionMatch = 1;
            unity.Application = "";
            unity.DependencyConfig = "";
            unity.BootloaderConfig = "";
            unity.UpgradeConfig = "";
            return unity;
        }

        public LIB.Proto.UnityEntity ToProtoEntity(UnityEntity _unity)
        {
            var unity = new LIB.Proto.UnityEntity();
            unity.Uuid = _unity.Uuid?.ToString() ?? "";
            unity.Name = _unity.Name;
            unity.Display = _unity.Display;
            unity.SkinSplashBackground = _unity.SkinSplashBackground;
            unity.SkinSplashSlogan = _unity.SkinSplashSlogan;
            unity.GraphicsFps = _unity.GraphicsFPS;
            unity.GraphicsQuality = _unity.GraphicsQuality;
            unity.GraphicsPixelResolution = _unity.GraphicsPixelResolution;
            unity.GraphicsReferenceResolutionWidth = _unity.GraphicsReferenceResolutionWidth;
            unity.GraphicsReferenceResolutionHeight = _unity.GraphicsReferenceResolutionHeight;
            unity.GraphicsReferenceResolutionMatch = _unity.GraphicsReferenceResolutionMatch;
            unity.Application = _unity.Application;
            unity.DependencyConfig = _unity.DependencyConfig;
            unity.BootloaderConfig = _unity.BootloaderConfig;
            unity.UpgradeConfig = _unity.UpgradeConfig;
            return unity;
        }

        public virtual async Task<UnityEntity?> GetByNameAsync(string _name) =>
            await collection_.Find(x => x.Name.Equals(_name)).FirstOrDefaultAsync();

        public virtual async Task<KeyValuePair<long, List<UnityEntity>>> SearchAsync(int _offset, int _count, string _name, string _display)
        {
            var filter = Builders<UnityEntity>.Filter.Where(x =>
                            (string.IsNullOrWhiteSpace(_name) || (null != x.Name && x.Name.ToLower().Contains(_name.ToLower()))) &&
                            (string.IsNullOrWhiteSpace(_display) || (null != x.Display && x.Display.ToLower().Contains(_display.ToLower())))
                        );

            var found = collection_.Find(filter);

            var total = await found.CountDocumentsAsync();
            var unityS = await found.Skip((int)_offset).Limit((int)_count).ToListAsync();

            return new KeyValuePair<long, List<UnityEntity>>(total, unityS);
        }
    }
}
