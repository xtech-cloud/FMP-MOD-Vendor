
//*************************************************************************************
//   !!! Generated by the fmp-cli 1.39.0.  DO NOT EDIT!
//*************************************************************************************

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace XTC.FMP.MOD.Vendor.App.Service
{
    /// <summary>
    /// 数据实体
    /// </summary>
    /// <example>
    /// public class YourDAO : DAO<YourEntity>
    /// {
    ///     public YourDAO(IOptions<DBSettings> _settings) : base(_settings)
    ///     {
    ///     }
    /// }
    /// </example>
    public class Entity
    {
        /// <summary>
        /// MongoDB的ObjectId
        /// </summary>
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        private string? _id{ get; set; }

        /// <summary>
        /// 实体的唯一识别ID
        /// </summary>
        public Guid? Uuid { get; set; }
    }
}
