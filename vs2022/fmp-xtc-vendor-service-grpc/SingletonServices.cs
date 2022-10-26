using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace XTC.FMP.MOD.Vendor.App.Service
{
    public class SingletonServices
    {
        private MongoClient mongoClient_;
        private IMongoDatabase mongoDatabase_;
        private BlazorDAO daoBlazor_;
        private UnityDAO daoUnity_;
        private MinIOClient minioClient_;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>
        /// 参数为自动注入，支持多个参数，DatabaseSettings的注入点在Program.cs中，自定义设置可在MyProgram.PreBuild中注入
        /// </remarks>
        public SingletonServices(IOptions<DatabaseSettings> _databaseSettings, IOptions<MinIOSettings> _minioSettings)
        {
            minioClient_ = new MinIOClient(_minioSettings);

            mongoClient_ = new MongoClient(_databaseSettings.Value.ConnectionString);
            mongoDatabase_ = mongoClient_.GetDatabase(_databaseSettings.Value.DatabaseName);

            daoBlazor_ = new BlazorDAO(mongoDatabase_);
            daoUnity_ = new UnityDAO(mongoDatabase_);
        }

        public BlazorDAO getBlazorDAO()
        {
            return daoBlazor_;
        }

        public UnityDAO getUnityDAO()
        {
            return daoUnity_;
        }

        public MinIOClient getMinIOClient()
        {
            return minioClient_;
        }
    }
}
