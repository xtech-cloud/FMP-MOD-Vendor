
//*************************************************************************************
//   !!! Generated by the fmp-cli 1.70.0.  DO NOT EDIT!
//*************************************************************************************

using System.Threading;
using XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.Vendor.LIB.Proto;

namespace XTC.FMP.MOD.Vendor.LIB.MVCS
{
    /// <summary>
    /// Blazor控制层基类
    /// </summary>
    public class BlazorControllerBase : Controller
    {
        /// <summary>
        /// 带uid参数的构造函数
        /// </summary>
        /// <param name="_uid">实例化后的唯一识别码</param>
        /// <param name="_gid">直系的组的ID</param>
        public BlazorControllerBase(string _uid, string _gid) : base(_uid)
        {
            gid_ = _gid;
        }


        /// <summary>
        /// 更新Create的数据
        /// </summary>
        /// <param name="_status">直系状态</param>
        /// <param name="_response">Create的回复</param>
        public virtual void UpdateProtoCreate(BlazorModel.BlazorStatus? _status, UuidResponse _response, object? _context)
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
        public virtual void UpdateProtoUpdate(BlazorModel.BlazorStatus? _status, UuidResponse _response, object? _context)
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
        public virtual void UpdateProtoRetrieve(BlazorModel.BlazorStatus? _status, BlazorRetrieveResponse _response, object? _context)
        {
            Error err = new Error(_response.Status.Code, _response.Status.Message);
            BlazorRetrieveResponseDTO? dto = new BlazorRetrieveResponseDTO(_response);
            getView()?.RefreshProtoRetrieve(err, dto, _context);
        }

        /// <summary>
        /// 更新Delete的数据
        /// </summary>
        /// <param name="_status">直系状态</param>
        /// <param name="_response">Delete的回复</param>
        public virtual void UpdateProtoDelete(BlazorModel.BlazorStatus? _status, UuidResponse _response, object? _context)
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
        public virtual void UpdateProtoList(BlazorModel.BlazorStatus? _status, BlazorListResponse _response, object? _context)
        {
            Error err = new Error(_response.Status.Code, _response.Status.Message);
            BlazorListResponseDTO? dto = new BlazorListResponseDTO(_response);
            getView()?.RefreshProtoList(err, dto, _context);
        }

        /// <summary>
        /// 更新Search的数据
        /// </summary>
        /// <param name="_status">直系状态</param>
        /// <param name="_response">Search的回复</param>
        public virtual void UpdateProtoSearch(BlazorModel.BlazorStatus? _status, BlazorListResponse _response, object? _context)
        {
            Error err = new Error(_response.Status.Code, _response.Status.Message);
            BlazorListResponseDTO? dto = new BlazorListResponseDTO(_response);
            getView()?.RefreshProtoSearch(err, dto, _context);
        }


        /// <summary>
        /// 获取直系视图层
        /// </summary>
        /// <returns>视图层</returns>
        protected BlazorView? getView()
        {
            if(null == view_)
                view_ = findView(BlazorView.NAME + "." + gid_) as BlazorView;
            return view_;
        }

        /// <summary>
        /// 直系的MVCS的四个组件的组的ID
        /// </summary>
        protected string gid_ = "";

        /// <summary>
        /// 直系视图层
        /// </summary>
        private BlazorView? view_;
    }
}
