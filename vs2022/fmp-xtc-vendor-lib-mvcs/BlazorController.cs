
namespace XTC.FMP.MOD.Vendor.LIB.MVCS
{
    /// <summary>
    /// Blazor控制层
    /// </summary>
    public class BlazorController : BlazorControllerBase
    {
        /// <summary>
        /// 完整名称
        /// </summary>
        public const string NAME = "XTC.FMP.MOD.Vendor.LIB.MVCS.BlazorController";

        /// <summary>
        /// 带uid参数的构造函数
        /// </summary>
        /// <param name="_uid">实例化后的唯一识别码</param>
        /// <param name="_gid">直系的组的ID</param>
        public BlazorController(string _uid, string _gid) : base(_uid, _gid) 
        {
        }
    }
}

