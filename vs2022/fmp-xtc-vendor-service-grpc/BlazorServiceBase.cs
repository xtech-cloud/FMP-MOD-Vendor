
//*************************************************************************************
//   !!! Generated by the fmp-cli 1.70.0.  DO NOT EDIT!
//*************************************************************************************

using System.Net;
using Grpc.Core;
using System.Threading.Tasks;
using XTC.FMP.MOD.Vendor.LIB.Proto;

namespace XTC.FMP.MOD.Vendor.App.Service
{
    /// <summary>
    /// Blazor基类
    /// </summary>
    public class BlazorServiceBase : LIB.Proto.Blazor.BlazorBase
    {
    

        public override async Task<UuidResponse> Create(BlazorCreateRequest _request, ServerCallContext _context)
        {
            try
            {
                return await safeCreate(_request, _context);
            }
            catch (ArgumentRequiredException ex)
            {
                return await Task.Run(() => new UuidResponse
                {
                    Status = new LIB.Proto.Status() { Code = -HttpStatusCode.BadRequest.GetHashCode(), Message = ex.Message },
                });
            }
            catch (Exception ex)
            {
                return await Task.Run(() => new UuidResponse
                {
                    Status = new LIB.Proto.Status() { Code = -HttpStatusCode.InternalServerError.GetHashCode(), Message = ex.Message },
                });
            }
        }

        public override async Task<UuidResponse> Update(BlazorUpdateRequest _request, ServerCallContext _context)
        {
            try
            {
                return await safeUpdate(_request, _context);
            }
            catch (ArgumentRequiredException ex)
            {
                return await Task.Run(() => new UuidResponse
                {
                    Status = new LIB.Proto.Status() { Code = -HttpStatusCode.BadRequest.GetHashCode(), Message = ex.Message },
                });
            }
            catch (Exception ex)
            {
                return await Task.Run(() => new UuidResponse
                {
                    Status = new LIB.Proto.Status() { Code = -HttpStatusCode.InternalServerError.GetHashCode(), Message = ex.Message },
                });
            }
        }

        public override async Task<BlazorRetrieveResponse> Retrieve(UuidRequest _request, ServerCallContext _context)
        {
            try
            {
                return await safeRetrieve(_request, _context);
            }
            catch (ArgumentRequiredException ex)
            {
                return await Task.Run(() => new BlazorRetrieveResponse
                {
                    Status = new LIB.Proto.Status() { Code = -HttpStatusCode.BadRequest.GetHashCode(), Message = ex.Message },
                });
            }
            catch (Exception ex)
            {
                return await Task.Run(() => new BlazorRetrieveResponse
                {
                    Status = new LIB.Proto.Status() { Code = -HttpStatusCode.InternalServerError.GetHashCode(), Message = ex.Message },
                });
            }
        }

        public override async Task<UuidResponse> Delete(UuidRequest _request, ServerCallContext _context)
        {
            try
            {
                return await safeDelete(_request, _context);
            }
            catch (ArgumentRequiredException ex)
            {
                return await Task.Run(() => new UuidResponse
                {
                    Status = new LIB.Proto.Status() { Code = -HttpStatusCode.BadRequest.GetHashCode(), Message = ex.Message },
                });
            }
            catch (Exception ex)
            {
                return await Task.Run(() => new UuidResponse
                {
                    Status = new LIB.Proto.Status() { Code = -HttpStatusCode.InternalServerError.GetHashCode(), Message = ex.Message },
                });
            }
        }

        public override async Task<BlazorListResponse> List(BlazorListRequest _request, ServerCallContext _context)
        {
            try
            {
                return await safeList(_request, _context);
            }
            catch (ArgumentRequiredException ex)
            {
                return await Task.Run(() => new BlazorListResponse
                {
                    Status = new LIB.Proto.Status() { Code = -HttpStatusCode.BadRequest.GetHashCode(), Message = ex.Message },
                });
            }
            catch (Exception ex)
            {
                return await Task.Run(() => new BlazorListResponse
                {
                    Status = new LIB.Proto.Status() { Code = -HttpStatusCode.InternalServerError.GetHashCode(), Message = ex.Message },
                });
            }
        }

        public override async Task<BlazorListResponse> Search(BlazorSearchRequest _request, ServerCallContext _context)
        {
            try
            {
                return await safeSearch(_request, _context);
            }
            catch (ArgumentRequiredException ex)
            {
                return await Task.Run(() => new BlazorListResponse
                {
                    Status = new LIB.Proto.Status() { Code = -HttpStatusCode.BadRequest.GetHashCode(), Message = ex.Message },
                });
            }
            catch (Exception ex)
            {
                return await Task.Run(() => new BlazorListResponse
                {
                    Status = new LIB.Proto.Status() { Code = -HttpStatusCode.InternalServerError.GetHashCode(), Message = ex.Message },
                });
            }
        }



        protected virtual async Task<UuidResponse> safeCreate(BlazorCreateRequest _request, ServerCallContext _context)
        {
            return await Task.Run(() => new UuidResponse {
                    Status = new LIB.Proto.Status() { Code = -1, Message = "Not Implemented" },
            });
        }

        protected virtual async Task<UuidResponse> safeUpdate(BlazorUpdateRequest _request, ServerCallContext _context)
        {
            return await Task.Run(() => new UuidResponse {
                    Status = new LIB.Proto.Status() { Code = -1, Message = "Not Implemented" },
            });
        }

        protected virtual async Task<BlazorRetrieveResponse> safeRetrieve(UuidRequest _request, ServerCallContext _context)
        {
            return await Task.Run(() => new BlazorRetrieveResponse {
                    Status = new LIB.Proto.Status() { Code = -1, Message = "Not Implemented" },
            });
        }

        protected virtual async Task<UuidResponse> safeDelete(UuidRequest _request, ServerCallContext _context)
        {
            return await Task.Run(() => new UuidResponse {
                    Status = new LIB.Proto.Status() { Code = -1, Message = "Not Implemented" },
            });
        }

        protected virtual async Task<BlazorListResponse> safeList(BlazorListRequest _request, ServerCallContext _context)
        {
            return await Task.Run(() => new BlazorListResponse {
                    Status = new LIB.Proto.Status() { Code = -1, Message = "Not Implemented" },
            });
        }

        protected virtual async Task<BlazorListResponse> safeSearch(BlazorSearchRequest _request, ServerCallContext _context)
        {
            return await Task.Run(() => new BlazorListResponse {
                    Status = new LIB.Proto.Status() { Code = -1, Message = "Not Implemented" },
            });
        }

    }
}

