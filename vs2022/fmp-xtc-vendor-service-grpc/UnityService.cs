
using Grpc.Core;
using Newtonsoft.Json;
using System.Threading.Tasks;
using XTC.FMP.MOD.Vendor.LIB.Proto;

namespace XTC.FMP.MOD.Vendor.App.Service
{
    public class UnityService : UnityServiceBase
    {
        private readonly SingletonServices singletonServices_;

        public UnityService(SingletonServices _singletonServices)
        {
            singletonServices_ = _singletonServices;
        }

        protected override async Task<UuidResponse> safeCreate(UnityCreateRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Name, "Name");

            var unity = await singletonServices_.getUnityDAO().GetByNameAsync(_request.Name);
            if (null != unity)
            {
                return new UuidResponse
                {
                    Status = new LIB.Proto.Status { Code = 1, Message = "Exists" },
                };
            }

            unity = singletonServices_.getUnityDAO().NewDefaultUnity();
            unity.Name = _request.Name;
            unity.Display = _request.Display;

            await singletonServices_.getUnityDAO().CreateAsync(unity);
            return new UuidResponse
            {
                Status = new LIB.Proto.Status(),
                Uuid = unity.Uuid.ToString(),
            };
        }

        protected override async Task<UnityRetrieveResponse> safeRetrieve(UuidRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Uuid, "Uuid");

            var unity = await singletonServices_.getUnityDAO().GetAsync(_request.Uuid);
            if (null == unity)
            {
                return new UnityRetrieveResponse
                {
                    Status = new LIB.Proto.Status { Code = 1, Message = "NotFound" },
                };
            }

            return new UnityRetrieveResponse
            {
                Status = new LIB.Proto.Status(),
                Unity = singletonServices_.getUnityDAO().ToProtoEntity(unity),
            };
        }

        protected override async Task<UnityListResponse> safeList(UnityListRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredNumber((int)_request.Count, "Count");

            var response = new UnityListResponse
            {
                Status = new LIB.Proto.Status(),
            };

            response.Total = await singletonServices_.getUnityDAO().CountAsync();
            var unityS = await singletonServices_.getUnityDAO().ListAsync((int)_request.Offset, (int)_request.Count);
            foreach (var unity in unityS)
            {
                response.UnityS.Add(singletonServices_.getUnityDAO().ToProtoEntity(unity));
            }
            return response;
        }

        protected override async Task<UuidResponse> safeDelete(UuidRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Uuid, "Uuid");

            await singletonServices_.getUnityDAO().RemoveAsync(_request.Uuid);
            return new UuidResponse
            {
                Status = new LIB.Proto.Status(),
                Uuid = _request.Uuid,
            };
        }

        protected override async Task<UuidResponse> safeUpdate(UnityUpdateRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Uuid, "Uuid");

            var unity = await singletonServices_.getUnityDAO().GetAsync(_request.Uuid);
            if (null == unity)
            {
                return new UuidResponse
                {
                    Status = new LIB.Proto.Status { Code = 1, Message = "NotFound" },
                };
            }

            unity.Name = _request.Name;
            unity.Display = _request.Display;
            unity.SkinSplashBackground = _request.SkinSplashBackground;
            unity.SkinSplashSlogan = _request.SkinSplashSlogan;
            unity.GraphicsFPS = _request.GraphicsFps;
            unity.GraphicsQuality = _request.GraphicsQuality;
            unity.GraphicsPixelResolution = _request.GraphicsPixelResolution;
            unity.GraphicsReferenceResolutionWidth = _request.GraphicsReferenceResolutionWidth;
            unity.GraphicsReferenceResolutionHeight = _request.GraphicsReferenceResolutionHeight;
            unity.GraphicsReferenceResolutionMatch = _request.GraphicsReferenceResolutionMatch;
            unity.Application = _request.Application;
            unity.DependencyConfig = _request.DependencyConfig;
            unity.BootloaderConfig = _request.BootloaderConfig;
            unity.UpdateConfig = _request.UpdateConfig;
            unity.ModuleConfigS.Clear();
            foreach(var pair in _request.ModuleConfigs)
                unity.ModuleConfigS[pair.Key] = pair.Value;
            unity.ModuleCatalogS.Clear();
            foreach(var pair in _request.ModuleCatalogs)
                unity.ModuleCatalogS[pair.Key] = pair.Value;

            await singletonServices_.getUnityDAO().UpdateAsync(_request.Uuid, unity);

            // 保存到存储中
            string filepath = string.Format("unity/{0}.json", unity.Uuid.ToString());
            byte[] bytesBlazor = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(unity));
            await singletonServices_.getMinIOClient().PutObject(filepath, new MemoryStream(bytesBlazor));

            return new UuidResponse
            {
                Status = new LIB.Proto.Status(),
                Uuid = _request.Uuid,
            };
        }

        protected override async Task<UnityListResponse> safeSearch(UnitySearchRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredNumber((int)_request.Count, "Count");

            var response = new UnityListResponse
            {
                Status = new LIB.Proto.Status(),
            };

            if (string.IsNullOrWhiteSpace(_request.Name) && string.IsNullOrWhiteSpace(_request.Display))
            {
                return response;
            }

            var result = await singletonServices_.getUnityDAO().SearchAsync((int)_request.Offset, (int)_request.Count, _request.Name, _request.Display);
            response.Total = result.Key;
            foreach (var unity in result.Value)
            {
                response.UnityS.Add(singletonServices_.getUnityDAO().ToProtoEntity(unity));
            }
            return response;
        }
    }
}
