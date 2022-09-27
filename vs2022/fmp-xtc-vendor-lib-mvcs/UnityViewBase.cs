
//*************************************************************************************
//   !!! Generated by the fmp-cli 1.50.0.  DO NOT EDIT!
//*************************************************************************************

using System.Threading;
using XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.Vendor.LIB.Bridge;

namespace XTC.FMP.MOD.Vendor.LIB.MVCS
{
    /// <summary>
    /// Unity视图层基类
    /// </summary>
    public class UnityViewBase : View
    {
        /// <summary>
        /// 带uid参数的构造函数
        /// </summary>
        /// <param name="_uid">实例化后的唯一识别码</param>
        /// <param name="_gid">直系的组的ID</param>
        public UnityViewBase(string _uid, string _gid) : base(_uid)
        {
            gid_ = _gid;
        }


        /// <summary>
        /// 刷新Create的数据
        /// </summary>
        /// <param name="_err">错误</param>
        /// <param name="_dto">UuidResponse的数据传输对象</param>
        public virtual void RefreshProtoCreate(Error _err, UuidResponseDTO _dto, object? _context)
        {
            var bridge = getFacade()?.getUiBridge() as IUnityUiBridge; 
            if (!Error.IsOK(_err))
            {
                bridge?.Alert(string.Format("errcode_Create_{0}", _err.getCode()), _err.getMessage(), _context);
                return;
            }
            bridge?.RefreshCreate(_dto, _context);
        }

        /// <summary>
        /// 刷新Update的数据
        /// </summary>
        /// <param name="_err">错误</param>
        /// <param name="_dto">UuidResponse的数据传输对象</param>
        public virtual void RefreshProtoUpdate(Error _err, UuidResponseDTO _dto, object? _context)
        {
            var bridge = getFacade()?.getUiBridge() as IUnityUiBridge; 
            if (!Error.IsOK(_err))
            {
                bridge?.Alert(string.Format("errcode_Update_{0}", _err.getCode()), _err.getMessage(), _context);
                return;
            }
            bridge?.RefreshUpdate(_dto, _context);
        }

        /// <summary>
        /// 刷新Retrieve的数据
        /// </summary>
        /// <param name="_err">错误</param>
        /// <param name="_dto">UnityRetrieveResponse的数据传输对象</param>
        public virtual void RefreshProtoRetrieve(Error _err, UnityRetrieveResponseDTO _dto, object? _context)
        {
            var bridge = getFacade()?.getUiBridge() as IUnityUiBridge; 
            if (!Error.IsOK(_err))
            {
                bridge?.Alert(string.Format("errcode_Retrieve_{0}", _err.getCode()), _err.getMessage(), _context);
                return;
            }
            bridge?.RefreshRetrieve(_dto, _context);
        }

        /// <summary>
        /// 刷新Delete的数据
        /// </summary>
        /// <param name="_err">错误</param>
        /// <param name="_dto">UuidResponse的数据传输对象</param>
        public virtual void RefreshProtoDelete(Error _err, UuidResponseDTO _dto, object? _context)
        {
            var bridge = getFacade()?.getUiBridge() as IUnityUiBridge; 
            if (!Error.IsOK(_err))
            {
                bridge?.Alert(string.Format("errcode_Delete_{0}", _err.getCode()), _err.getMessage(), _context);
                return;
            }
            bridge?.RefreshDelete(_dto, _context);
        }

        /// <summary>
        /// 刷新List的数据
        /// </summary>
        /// <param name="_err">错误</param>
        /// <param name="_dto">UnityListResponse的数据传输对象</param>
        public virtual void RefreshProtoList(Error _err, UnityListResponseDTO _dto, object? _context)
        {
            var bridge = getFacade()?.getUiBridge() as IUnityUiBridge; 
            if (!Error.IsOK(_err))
            {
                bridge?.Alert(string.Format("errcode_List_{0}", _err.getCode()), _err.getMessage(), _context);
                return;
            }
            bridge?.RefreshList(_dto, _context);
        }

        /// <summary>
        /// 刷新Search的数据
        /// </summary>
        /// <param name="_err">错误</param>
        /// <param name="_dto">UnityListResponse的数据传输对象</param>
        public virtual void RefreshProtoSearch(Error _err, UnityListResponseDTO _dto, object? _context)
        {
            var bridge = getFacade()?.getUiBridge() as IUnityUiBridge; 
            if (!Error.IsOK(_err))
            {
                bridge?.Alert(string.Format("errcode_Search_{0}", _err.getCode()), _err.getMessage(), _context);
                return;
            }
            bridge?.RefreshSearch(_dto, _context);
        }


        /// <summary>
        /// 获取直系数据层
        /// </summary>
        /// <returns>数据层</returns>
        protected UnityModel? getModel()
        {
            if(null == model_)
                model_ = findModel(UnityModel.NAME + "." + gid_) as UnityModel;
            return model_;
        }

        /// <summary>
        /// 获取直系服务层
        /// </summary>
        /// <returns>服务层</returns>
        protected UnityService? getService()
        {
            if(null == service_)
                service_ = findService(UnityService.NAME + "." + gid_) as UnityService;
            return service_;
        }

        /// <summary>
        /// 获取直系UI装饰层
        /// </summary>
        /// <returns>UI装饰层</returns>
        protected UnityFacade? getFacade()
        {
            if(null == facade_)
                facade_ = findFacade(UnityFacade.NAME + "." + gid_) as UnityFacade;
            return facade_;
        }

        /// <summary>
        /// 直系的MVCS的四个组件的组的ID
        /// </summary>
        protected string gid_ = "";

        /// <summary>
        /// 直系数据层
        /// </summary>
        private UnityModel? model_;

        /// <summary>
        /// 直系服务层
        /// </summary>
        private UnityService? service_;

        /// <summary>
        /// 直系UI装饰层
        /// </summary>
        private UnityFacade? facade_;
    }
}

