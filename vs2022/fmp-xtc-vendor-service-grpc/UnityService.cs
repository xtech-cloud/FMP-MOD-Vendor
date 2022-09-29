
using Grpc.Core;
using Newtonsoft.Json;
using System.Threading.Tasks;
using XTC.FMP.MOD.Vendor.LIB.Proto;

namespace XTC.FMP.MOD.Vendor.App.Service
{
    public class UnityService : UnityServiceBase
    {
        private readonly MinIOClient minioClient_;
        // 解开以下代码的注释，可支持数据库操作
        private readonly UnityDAO unityDAO_;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>
        /// 支持多个参数，均为自动注入，注入点位于MyProgram.PreBuild
        /// </remarks>
        /// <param name="_unityDAO">自动注入的数据操作对象</param>
        /// <param name="_minioClient">自动注入的MinIO客户端</param>
        public UnityService(UnityDAO _unityDAO, MinIOClient _minioClient)
        {
            unityDAO_ = _unityDAO;
            minioClient_ = _minioClient;
        }

        protected override async Task<UuidResponse> safeCreate(UnityCreateRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Name, "Name");

            var unity = await unityDAO_.GetByNameAsync(_request.Name);
            if (null != unity)
            {
                return new UuidResponse
                {
                    Status = new LIB.Proto.Status { Code = 1, Message = "Exists" },
                };
            }

            unity = unityDAO_.NewDefaultUnity();
            unity.Name = _request.Name;
            unity.Display = _request.Display;

            await unityDAO_.CreateAsync(unity);
            return new UuidResponse
            {
                Status = new LIB.Proto.Status(),
                Uuid = unity.Uuid.ToString(),
            };
        }

        protected override async Task<UnityRetrieveResponse> safeRetrieve(UuidRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Uuid, "Uuid");

            var unity = await unityDAO_.GetAsync(_request.Uuid);
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
                Unity = unityDAO_.ToProtoEntity(unity),
            };
        }

        protected override async Task<UnityListResponse> safeList(UnityListRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredNumber((int)_request.Count, "Count");

            var response = new UnityListResponse
            {
                Status = new LIB.Proto.Status(),
            };

            response.Total = await unityDAO_.CountAsync();
            var unityS = await unityDAO_.ListAsync((int)_request.Offset, (int)_request.Count);
            foreach (var unity in unityS)
            {
                response.UnityS.Add(unityDAO_.ToProtoEntity(unity));
            }
            return response;
        }

        protected override async Task<UuidResponse> safeDelete(UuidRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Uuid, "Uuid");

            await unityDAO_.RemoveAsync(_request.Uuid);
            return new UuidResponse
            {
                Status = new LIB.Proto.Status(),
                Uuid = _request.Uuid,
            };
        }

        protected override async Task<UuidResponse> safeUpdate(UnityUpdateRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Uuid, "Uuid");

            var unity = await unityDAO_.GetAsync(_request.Uuid);
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

            await unityDAO_.UpdateAsync(_request.Uuid, unity);

            // 保存到存储中
            string filepath = string.Format("unity/{0}.json", unity.Uuid.ToString());
            byte[] bytesBlazor = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(unity));
            await minioClient_.PutObject(filepath, new MemoryStream(bytesBlazor));

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

            var result = await unityDAO_.SearchAsync((int)_request.Offset, (int)_request.Count, _request.Name, _request.Display);
            response.Total = result.Key;
            foreach (var unity in result.Value)
            {
                response.UnityS.Add(unityDAO_.ToProtoEntity(unity));
            }
            return response;
        }
    }
}
