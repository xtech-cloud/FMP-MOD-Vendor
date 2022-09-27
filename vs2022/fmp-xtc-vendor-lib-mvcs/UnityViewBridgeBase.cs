
//*************************************************************************************
//   !!! Generated by the fmp-cli 1.53.0.  DO NOT EDIT!
//*************************************************************************************

using System.Threading;
using System.Threading.Tasks;
using XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.Vendor.LIB.Bridge;

namespace XTC.FMP.MOD.Vendor.LIB.MVCS
{
    /// <summary>
    /// Unity的视图桥接层基类（协议部分）
    /// 处理UI的事件
    /// </summary>
    public class UnityViewBridgeBase : IUnityViewBridge
    {

        /// <summary>
        /// 直系服务层
        /// </summary>
        public UnityService? service { get; set; }


        /// <summary>
        /// 处理Create的提交
        /// </summary>
        /// <param name="_dto">UnityCreateRequest的数据传输对象</param>
        /// <returns>错误</returns>
        public virtual async Task<Error> OnCreateSubmit(IDTO _dto, object? _context)
        {
            UnityCreateRequestDTO? dto = _dto as UnityCreateRequestDTO;
            if(null == service)
            {
                return Error.NewNullErr("service is null");
            }
            return await service.CallCreate(dto?.Value, _context);
        }

        /// <summary>
        /// 处理Update的提交
        /// </summary>
        /// <param name="_dto">UnityUpdateRequest的数据传输对象</param>
        /// <returns>错误</returns>
        public virtual async Task<Error> OnUpdateSubmit(IDTO _dto, object? _context)
        {
            UnityUpdateRequestDTO? dto = _dto as UnityUpdateRequestDTO;
            if(null == service)
            {
                return Error.NewNullErr("service is null");
            }
            return await service.CallUpdate(dto?.Value, _context);
        }

        /// <summary>
        /// 处理Retrieve的提交
        /// </summary>
        /// <param name="_dto">UuidRequest的数据传输对象</param>
        /// <returns>错误</returns>
        public virtual async Task<Error> OnRetrieveSubmit(IDTO _dto, object? _context)
        {
            UuidRequestDTO? dto = _dto as UuidRequestDTO;
            if(null == service)
            {
                return Error.NewNullErr("service is null");
            }
            return await service.CallRetrieve(dto?.Value, _context);
        }

        /// <summary>
        /// 处理Delete的提交
        /// </summary>
        /// <param name="_dto">UuidRequest的数据传输对象</param>
        /// <returns>错误</returns>
        public virtual async Task<Error> OnDeleteSubmit(IDTO _dto, object? _context)
        {
            UuidRequestDTO? dto = _dto as UuidRequestDTO;
            if(null == service)
            {
                return Error.NewNullErr("service is null");
            }
            return await service.CallDelete(dto?.Value, _context);
        }

        /// <summary>
        /// 处理List的提交
        /// </summary>
        /// <param name="_dto">UnityListRequest的数据传输对象</param>
        /// <returns>错误</returns>
        public virtual async Task<Error> OnListSubmit(IDTO _dto, object? _context)
        {
            UnityListRequestDTO? dto = _dto as UnityListRequestDTO;
            if(null == service)
            {
                return Error.NewNullErr("service is null");
            }
            return await service.CallList(dto?.Value, _context);
        }

        /// <summary>
        /// 处理Search的提交
        /// </summary>
        /// <param name="_dto">UnitySearchRequest的数据传输对象</param>
        /// <returns>错误</returns>
        public virtual async Task<Error> OnSearchSubmit(IDTO _dto, object? _context)
        {
            UnitySearchRequestDTO? dto = _dto as UnitySearchRequestDTO;
            if(null == service)
            {
                return Error.NewNullErr("service is null");
            }
            return await service.CallSearch(dto?.Value, _context);
        }


    }
}
