using Grpc.Core;
using Newtonsoft.Json;
using System.Threading.Tasks;
using XTC.FMP.MOD.Vendor.LIB.Proto;

namespace XTC.FMP.MOD.Vendor.App.Service
{
    public class BlazorService : BlazorServiceBase
    {
        private readonly SingletonServices singletonServices_;

        public BlazorService(SingletonServices _singletonServices)
        {
            singletonServices_ = _singletonServices;
        }

        protected override async Task<UuidResponse> safeCreate(BlazorCreateRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Name, "Name");

            var blazor = await singletonServices_.getBlazorDAO().GetByNameAsync(_request.Name);
            if (null != blazor)
            {
                return new UuidResponse
                {
                    Status = new LIB.Proto.Status { Code = 1, Message = "Exists" },
                };
            }

            blazor = singletonServices_.getBlazorDAO().NewDefaultBlazor();
            blazor.Name = _request.Name;
            blazor.Display = _request.Display;

            await singletonServices_.getBlazorDAO().CreateAsync(blazor);
            return new UuidResponse
            {
                Status = new LIB.Proto.Status(),
                Uuid = blazor.Uuid.ToString(),
            };
        }

        protected override async Task<BlazorRetrieveResponse> safeRetrieve(UuidRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Uuid, "Uuid");

            var blazor = await singletonServices_.getBlazorDAO().GetAsync(_request.Uuid);
            if (null == blazor)
            {
                return new BlazorRetrieveResponse
                {
                    Status = new LIB.Proto.Status { Code = 1, Message = "NotFound" },
                };
            }

            return new BlazorRetrieveResponse
            {
                Status = new LIB.Proto.Status(),
                Blazor = singletonServices_.getBlazorDAO().ToProtoEntity(blazor),
            };
        }

        protected override async Task<BlazorListResponse> safeList(BlazorListRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredNumber((int)_request.Count, "Count");

            var response = new BlazorListResponse
            {
                Status = new LIB.Proto.Status(),
            };

            response.Total = await singletonServices_.getBlazorDAO().CountAsync();
            var blazorS = await singletonServices_.getBlazorDAO().ListAsync((int)_request.Offset, (int)_request.Count);
            foreach (var blazor in blazorS)
            {
                response.BlazorS.Add(singletonServices_.getBlazorDAO().ToProtoEntity(blazor));
            }
            return response;
        }

        protected override async Task<UuidResponse> safeDelete(UuidRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Uuid, "Uuid");

            await singletonServices_.getBlazorDAO().RemoveAsync(_request.Uuid);
            return new UuidResponse
            {
                Status = new LIB.Proto.Status(),
                Uuid = _request.Uuid,
            };
        }

        protected override async Task<UuidResponse> safeUpdate(BlazorUpdateRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Uuid, "Uuid");

            var blazor = await singletonServices_.getBlazorDAO().GetAsync(_request.Uuid);
            if (null == blazor)
            {
                return new UuidResponse
                {
                    Status = new LIB.Proto.Status { Code = 1, Message = "NotFound" },
                };
            }

            blazor.Name = _request.Name;
            blazor.Display = _request.Display;
            blazor.Logo = _request.Logo;
            blazor.SkinConfig = _request.SkinConfig;
            blazor.MenuTitle = _request.MenuTitle;
            blazor.MenuConfig = _request.MenuConfig;
            blazor.ModulesConfig = _request.ModulesConfig;

            await singletonServices_.getBlazorDAO().UpdateAsync(_request.Uuid, blazor);

            // 保存到存储中
            string filepath = string.Format("blazor/{0}.json", blazor.Uuid.ToString());
            byte[] bytesBlazor = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(blazor));
            await singletonServices_.getMinIOClient().PutObject(filepath, new MemoryStream(bytesBlazor));

            return new UuidResponse
            {
                Status = new LIB.Proto.Status(),
                Uuid = _request.Uuid,
            };
        }

        protected override async Task<BlazorListResponse> safeSearch(BlazorSearchRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredNumber((int)_request.Count, "Count");

            var response = new BlazorListResponse
            {
                Status = new LIB.Proto.Status(),
            };

            if (string.IsNullOrWhiteSpace(_request.Name) && string.IsNullOrWhiteSpace(_request.Display))
            {
                return response;
            }

            var result = await singletonServices_.getBlazorDAO().SearchAsync((int)_request.Offset, (int)_request.Count, _request.Name, _request.Display);
            response.Total = result.Key;
            foreach (var blazor in result.Value)
            {
                response.BlazorS.Add(singletonServices_.getBlazorDAO().ToProtoEntity(blazor));
            }
            return response;
        }
    }
}
