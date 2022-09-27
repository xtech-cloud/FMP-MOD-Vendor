
//*************************************************************************************
//   !!! Generated by the fmp-cli 1.50.0.  DO NOT EDIT!
//*************************************************************************************

using System.Threading;
using XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.Vendor.LIB.Proto;

namespace XTC.FMP.MOD.Vendor.LIB.MVCS
{
    /// <summary>
    /// Unity控制层基类
    /// </summary>
    public class UnityControllerBase : Controller
    {
        /// <summary>
        /// 带uid参数的构造函数
        /// </summary>
        /// <param name="_uid">实例化后的唯一识别码</param>
        /// <param name="_gid">直系的组的ID</param>
        public UnityControllerBase(string _uid, string _gid) : base(_uid)
        {
            gid_ = _gid;
        }


        /// <summary>
        /// 更新Create的数据
        /// </summary>
        /// <param name="_status">直系状态</param>
        /// <param name="_response">Create的回复</param>
        public virtual void UpdateProtoCreate(UnityModel.UnityStatus? _status, UuidResponse _response, object? _context)
        {
            Error err = new Error(_response.Status.Code, _response.Status.Message);
            UuidResponseDTO? dto = new UuidResponseDTO(_response);
            getView()?.RefreshProtoCreate(err, dto, _context);
        }

        /// <summary>
        /// 更新Update的数据
        /// </summary>
        /// <param name="_status">直系状态</param>
        /// <param name="_response">Update的回复</param>
        public virtual void UpdateProtoUpdate(UnityModel.UnityStatus? _status, UuidResponse _response, object? _context)
        {
            Error err = new Error(_response.Status.Code, _response.Status.Message);
            UuidResponseDTO? dto = new UuidResponseDTO(_response);
            getView()?.RefreshProtoUpdate(err, dto, _context);
        }

        /// <summary>
        /// 更新Retrieve的数据
        /// </summary>
        /// <param name="_status">直系状态</param>
        /// <param name="_response">Retrieve的回复</param>
        public virtual void UpdateProtoRetrieve(UnityModel.UnityStatus? _status, UnityRetrieveResponse _response, object? _context)
        {
            Error err = new Error(_response.Status.Code, _response.Status.Message);
            UnityRetrieveResponseDTO? dto = new UnityRetrieveResponseDTO(_response);
            getView()?.RefreshProtoRetrieve(err, dto, _context);
        }

        /// <summary>
        /// 更新Delete的数据
        /// </summary>
        /// <param name="_status">直系状态</param>
        /// <param name="_response">Delete的回复</param>
        public virtual void UpdateProtoDelete(UnityModel.UnityStatus? _status, UuidResponse _response, object? _context)
        {
            Error err = new Error(_response.Status.Code, _response.Status.Message);
            UuidResponseDTO? dto = new UuidResponseDTO(_response);
            getView()?.RefreshProtoDelete(err, dto, _context);
        }

        /// <summary>
        /// 更新List的数据
        /// </summary>
        /// <param name="_status">直系状态</param>
        /// <param name="_response">List的回复</param>
        public virtual void UpdateProtoList(UnityModel.UnityStatus? _status, UnityListResponse _response, object? _context)
        {
            Error err = new Error(_response.Status.Code, _response.Status.Message);
            UnityListResponseDTO? dto = new UnityListResponseDTO(_response);
            getView()?.RefreshProtoList(err, dto, _context);
        }

        /// <summary>
        /// 更新Search的数据
        /// </summary>
        /// <param name="_status">直系状态</param>
        /// <param name="_response">Search的回复</param>
        public virtual void UpdateProtoSearch(UnityModel.UnityStatus? _status, UnityListResponse _response, object? _context)
        {
            Error err = new Error(_response.Status.Code, _response.Status.Message);
            UnityListResponseDTO? dto = new UnityListResponseDTO(_response);
            getView()?.RefreshProtoSearch(err, dto, _context);
        }


        /// <summary>
        /// 获取直系视图层
        /// </summary>
        /// <returns>视图层</returns>
        protected UnityView? getView()
        {
            if(null == view_)
                view_ = findView(UnityView.NAME + "." + gid_) as UnityView;
            return view_;
        }

        /// <summary>
        /// 直系的MVCS的四个组件的组的ID
        /// </summary>
        protected string gid_ = "";

        /// <summary>
        /// 直系视图层
        /// </summary>
        private UnityView? view_;
    }
}
