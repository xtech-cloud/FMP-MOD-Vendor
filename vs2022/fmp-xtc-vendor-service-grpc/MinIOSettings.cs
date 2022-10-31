namespace XTC.FMP.MOD.Vendor.App.Service
{
    /// <summary>
    /// MinIO设置
    /// </summary>
    public class MinIOSettings
    {
         /// <summary>
        /// 外部访问的SSL
        /// </summary>
        public bool AddressSSL{ get; set; } = false;
        /// <summary>
        /// 外部访问的地址
        /// </summary>
        public string AddressUrl { get; set; } = null!;

        /// <summary>
        /// 连接地址
        /// </summary>
        public string Endpoint { get; set; } = null!;

        /// <summary>
        /// 存储桶
        /// </summary>
        public string Bucket { get; set; } = null!;

        public string AccessKey { get; set; } = null!;

        public string SecretKey { get; set; } = null!;
    }
}
