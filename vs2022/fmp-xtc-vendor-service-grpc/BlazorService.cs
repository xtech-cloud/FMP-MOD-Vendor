using Grpc.Core;
using System.Threading.Tasks;
using XTC.FMP.MOD.Vendor.LIB.Proto;

namespace XTC.FMP.MOD.Vendor.App.Service
{
    public class BlazorService : BlazorServiceBase
    {
        // 解开以下代码的注释，可支持数据库操作
        private readonly BlazorDAO blazorDAO_;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>
        /// 支持多个参数，均为自动注入，注入点位于MyProgram.PreBuild
        /// </remarks>
        /// <param name="_yourDAO">自动注入的数据操作对象</param>
        public BlazorService(BlazorDAO _blazorDAO)
        {
            blazorDAO_ = _blazorDAO;
        }

        protected override async Task<UuidResponse> safeCreate(BlazorCreateRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Name, "Name");

            var blazor = await blazorDAO_.GetByNameAsync(_request.Name);
            if (null != blazor)
            {
                return new UuidResponse
                {
                    Status = new LIB.Proto.Status { Code = 1, Message = "Exists" },
                };
            }

            blazor = blazorDAO_.NewDefaultBlazor();
            blazor.Name = _request.Name;
            blazor.Display = _request.Display;

            await blazorDAO_.CreateAsync(blazor);
            return new UuidResponse
            {
                Status = new LIB.Proto.Status(),
                Uuid = blazor.Uuid.ToString(),
            };
        }

        protected override async Task<BlazorRetrieveResponse> safeRetrieve(UuidRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Uuid, "Uuid");

            var blazor = await blazorDAO_.GetAsync(_request.Uuid);
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
                Blazor = blazorDAO_.ToProtoEntity(blazor),
            };
        }

        protected override async Task<BlazorListResponse> safeList(BlazorListRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredNumber((int)_request.Count, "Count");

            var response = new BlazorListResponse
            {
                Status = new LIB.Proto.Status(),
            };

            response.Total = await blazorDAO_.CountAsync();
            var blazorS = await blazorDAO_.ListAsync((int)_request.Offset, (int)_request.Count);
            foreach (var blazor in blazorS)
            {
                response.BlazorS.Add(blazorDAO_.ToProtoEntity(blazor));
            }
            return response;
        }

        protected override async Task<UuidResponse> safeDelete(UuidRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Uuid, "Uuid");

            await blazorDAO_.RemoveAsync(_request.Uuid);
            return new UuidResponse
            {
                Status = new LIB.Proto.Status(),
                Uuid = _request.Uuid,
            };
        }

        protected override async Task<UuidResponse> safeUpdate(BlazorUpdateRequest _request, ServerCallContext _context)
        {
            ArgumentChecker.CheckRequiredString(_request.Uuid, "Uuid");

            var blazor = await blazorDAO_.GetAsync(_request.Uuid);
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

            await blazorDAO_.UpdateAsync(_request.Uuid, blazor);
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

            var result = await blazorDAO_.SearchAsync((int)_request.Offset, (int)_request.Count, _request.Name, _request.Display);
            response.Total = result.Key;
            foreach (var blazor in result.Value)
            {
                response.BlazorS.Add(blazorDAO_.ToProtoEntity(blazor));
            }
            return response;
        }
    }
}
