using System;
using System.Collections.Generic;
using System.Text;

namespace XTC.FMP.MOD.Vendor.LIB.MVCS
{
    public class Utilities
    {
        public static string QualityToString(int _quality)
        {
            switch (_quality)
            {
                case 0:
                    return "Very Low";
                case 1:
                    return "Low";
                case 2:
                    return "Medium";
                case 3:
                    return "High";
                case 4:
                    return "Very High";
                case 5:
                    return "Ultra";
                default:
                    return "Unknown";
            }
        }

        public static LIB.Proto.UnityEntity DeepCloneProtoUnityEntity(LIB.Proto.UnityEntity _entity)
        {
            var entity = new LIB.Proto.UnityEntity();
            entity.Uuid = _entity.Uuid;
            entity.Name = _entity.Name;
            entity.Display = _entity.Display;
            entity.SkinSplashBackground = _entity.SkinSplashBackground;
            entity.SkinSplashSlogan = _entity.SkinSplashSlogan;
            entity.GraphicsFps = _entity.GraphicsFps;
            entity.GraphicsQuality = _entity.GraphicsQuality;
            entity.GraphicsPixelResolution = _entity.GraphicsPixelResolution;
            entity.GraphicsReferenceResolutionWidth = _entity.GraphicsReferenceResolutionWidth;
            entity.GraphicsReferenceResolutionHeight = _entity.GraphicsReferenceResolutionHeight;
            entity.GraphicsReferenceResolutionMatch = _entity.GraphicsReferenceResolutionMatch;
            entity.Application = _entity.Application;
            return entity;
        }
    }
}
