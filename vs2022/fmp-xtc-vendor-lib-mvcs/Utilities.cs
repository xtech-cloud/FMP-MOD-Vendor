using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Xml.Serialization;

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
            entity.DependencyConfig = _entity.DependencyConfig;
            entity.BootloaderConfig = _entity.BootloaderConfig;
            entity.UpdateConfig = _entity.UpdateConfig;
            entity.ModuleCatalogs.Clear();
            foreach(var pair in _entity.ModuleCatalogs)
                entity.ModuleCatalogs[pair.Key] = pair.Value;
            entity.ModuleConfigs.Clear();
            foreach(var pair in _entity.ModuleConfigs)
                entity.ModuleConfigs[pair.Key] = pair.Value;
            return entity;
        }

        public static LIB.Proto.BlazorEntity DeepCloneProtoBlazorEntity(LIB.Proto.BlazorEntity _blazor)
        {
            var entity = new LIB.Proto.BlazorEntity();
            entity.Uuid = _blazor.Uuid;
            entity.Name = _blazor.Name;
            entity.Display = _blazor.Display;
            entity.Logo = _blazor.Logo;
            entity.SkinConfig = _blazor.SkinConfig;
            entity.MenuTitle = _blazor.MenuTitle;
            entity.MenuConfig = _blazor.MenuConfig;
            entity.ModulesConfig = _blazor.ModulesConfig;
            return entity;
        }

        public static string ToBase64XML<T>(T? _xml) where T : class, new()
        {
            if (null == _xml)
                return "";
            string result = "";
            var xs = new XmlSerializer(typeof(T));
            using (MemoryStream writer = new MemoryStream())
            {
                xs.Serialize(writer, _xml);
                result = System.Convert.ToBase64String(writer.ToArray());
            }
            return result;
        }

        public static T? FromBase64XML<T>(string? _xml) where T : class, new()
        {
            if (null == _xml)
                return null;
            if (string.IsNullOrEmpty(_xml))
                return null;
            byte[] bytes = Convert.FromBase64String(_xml);
            object result;
            var xs = new XmlSerializer(typeof(T));
            using (MemoryStream reader = new MemoryStream(bytes))
            {
                result = xs.Deserialize(reader);
            }
            return result as T;
        }

        public static string ToBase64JSON<T>(T? _json) where T : class, new()
        {
            if (null == _json)
                return "";
            string result = "";
            string json = JsonConvert.SerializeObject(_json);
            result = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            return result;
        }

        public static T? FromBase64JSON<T>(string? _json) where T : class, new()
        {
            if (null == _json)
                return null;
            if (string.IsNullOrEmpty(_json))
                return null;
            byte[] bytes = Convert.FromBase64String(_json);
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
        }

        public static string? DependencyReferenceToString(UnityModel.DependencyConfig.Reference? _reference)
        {
            if (null == _reference)
                return "";
            return string.Format("{0}_{1}@{2}", _reference.org, _reference.module, _reference.version);
        }

        public static UnityModel.DependencyConfig.Reference? DependencyReferenceFromString(string? _str)
        {
            if (null == _str)
                return null;
            string[] val1 = _str.Trim().Split("@");
            if (val1.Length != 2)
                return null;

            var reference = new UnityModel.DependencyConfig.Reference();
            reference.version = val1[1];
            string[] val2 = val1[0].Split("_");
            if (val2.Length != 2)
                return null;
            reference.org = val2[0];
            reference.module = val2[1];
            return reference;
        }

        public static string? DependencyPluginToString(UnityModel.DependencyConfig.Plugin? _plugin)
        {
            if (null == _plugin)
                return "";
            return string.Format("{0}@{1}", _plugin.name, _plugin.version);
        }

        public static UnityModel.DependencyConfig.Plugin? DependencyPluginFromString(string? _str)
        {
            if (null == _str)
                return null;
            string[] val = _str.Trim().Split("@");
            if (val.Length != 2)
                return null;

            var plugin = new UnityModel.DependencyConfig.Plugin();
            plugin.name = val[0];
            plugin.version = val[1];
            return plugin;
        }

        public static string? BootloaderStepToString(UnityModel.BootloaderConfig.BootStep? _step)
        {
            if (null == _step)
                return "";
            return string.Format("{0}_{1}", _step.org, _step.module);
        }

        public static UnityModel.BootloaderConfig.BootStep? BootloaderStepFromString(string? _str)
        {
            if (null == _str)
                return null;
            string[] val = _str.Trim().Split("_");
            if (val.Length != 2)
                return null;

            var step = new UnityModel.BootloaderConfig.BootStep();
            step.org = val[0];
            step.module = val[1];
            step.length = 1;
            return step;
        }
    }
}
