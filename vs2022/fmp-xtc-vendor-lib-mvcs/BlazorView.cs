
using XTC.FMP.LIB.MVCS;

namespace XTC.FMP.MOD.Vendor.LIB.MVCS
{
    /// <summary>
    /// Blazor视图层
    /// </summary>
    public class BlazorView : BlazorViewBase
    {
        /// <summary>
        /// 完整名称
        /// </summary>
        public const string NAME = "XTC.FMP.MOD.Vendor.LIB.MVCS.BlazorView";

        /// <summary>
        /// 带uid参数的构造函数
        /// </summary>
        /// <param name="_uid">实例化后的唯一识别码</param>
        /// <param name="_gid">直系的组的ID</param>
        public BlazorView(string _uid, string _gid) : base(_uid, _gid) 
        {
        }
    }
}


