
//*************************************************************************************
//   !!! Generated by the fmp-cli 1.70.0.  DO NOT EDIT!
//*************************************************************************************

using System.Threading;
using XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.Vendor.LIB.Proto;

namespace XTC.FMP.MOD.Vendor.LIB.MVCS
{
    /// <summary>
    /// Blazor数据层基类
    /// </summary>
    public class BlazorModelBase : Model
    {
        /// <summary>
        /// 带uid参数的构造函数
        /// </summary>
        /// <param name="_uid">实例化后的唯一识别码</param>
        /// <param name="_gid">直系的组的ID</param>
        public BlazorModelBase(string _uid, string _gid) : base(_uid)
        {
            gid_ = _gid;
        }


        /// <summary>
        /// 更新Create的数据
        /// </summary>
        /// <param name="_response">Create的回复</param>
        public virtual void UpdateProtoCreate(UuidResponse _response, object? _context)
        {
            getController()?.UpdateProtoCreate(status_ as BlazorModel.BlazorStatus, _response, _context);
        }

        /// <summary>
        /// 更新Update的数据
        /// </summary>
        /// <param name="_response">Update的回复</param>
        public virtual void UpdateProtoUpdate(UuidResponse _response, object? _context)
        {
            getController()?.UpdateProtoUpdate(status_ as BlazorModel.BlazorStatus, _response, _context);
        }

        /// <summary>
        /// 更新Retrieve的数据
        /// </summary>
        /// <param name="_response">Retrieve的回复</param>
        public virtual void UpdateProtoRetrieve(BlazorRetrieveResponse _response, object? _context)
        {
            getController()?.UpdateProtoRetrieve(status_ as BlazorModel.BlazorStatus, _response, _context);
        }

        /// <summary>
        /// 更新Delete的数据
        /// </summary>
        /// <param name="_response">Delete的回复</param>
        public virtual void UpdateProtoDelete(UuidResponse _response, object? _context)
        {
            getController()?.UpdateProtoDelete(status_ as BlazorModel.BlazorStatus, _response, _context);
        }

        /// <summary>
        /// 更新List的数据
        /// </summary>
        /// <param name="_response">List的回复</param>
        public virtual void UpdateProtoList(BlazorListResponse _response, object? _context)
        {
            getController()?.UpdateProtoList(status_ as BlazorModel.BlazorStatus, _response, _context);
        }

        /// <summary>
        /// 更新Search的数据
        /// </summary>
        /// <param name="_response">Search的回复</param>
        public virtual void UpdateProtoSearch(BlazorListResponse _response, object? _context)
        {
            getController()?.UpdateProtoSearch(status_ as BlazorModel.BlazorStatus, _response, _context);
        }


        /// <summary>
        /// 获取直系控制层
        /// </summary>
        /// <returns>控制层</returns>
        protected BlazorController? getController()
        {
            if(null == controller_)
                controller_ = findController(BlazorController.NAME + "." + gid_) as BlazorController;
            return controller_;
        }

        /// <summary>
        /// 直系的MVCS的四个组件的组的ID
        /// </summary>
        protected string gid_ = "";

        /// <summary>
        /// 直系控制层
        /// </summary>
        private BlazorController? controller_;
    }
}


