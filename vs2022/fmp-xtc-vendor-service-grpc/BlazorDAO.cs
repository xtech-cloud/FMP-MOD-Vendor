
//*************************************************************************************
//   !!! Generated by the fmp-cli 1.39.0.  DO NOT EDIT!
//*************************************************************************************

using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace XTC.FMP.MOD.Vendor.App.Service
{
    public class BlazorDAO : MongoDAO<BlazorEntity>
    {
        public BlazorDAO(IMongoDatabase _mongoDatabase)
        : base(_mongoDatabase, "Blazor")
        {
        }

        public BlazorEntity NewDefaultBlazor()
        {
            var blazor = new BlazorEntity();
            blazor.Uuid = Guid.NewGuid();
            blazor.Name = "";
            blazor.Display = "";
            blazor.Logo = "";
            blazor.SkinConfig = "";
            blazor.MenuTitle = "";
            blazor.MenuConfig = "";
            blazor.ModulesConfig = "";
            return blazor;
        }

        public LIB.Proto.BlazorEntity ToProtoEntity(BlazorEntity _blazor)
        {
            var blazor = new LIB.Proto.BlazorEntity();
            blazor.Uuid = _blazor.Uuid?.ToString() ?? "";
            blazor.Name = _blazor.Name;
            blazor.Display = _blazor.Display;
            blazor.Logo = _blazor.Logo;
            blazor.SkinConfig = _blazor.SkinConfig;
            blazor.MenuTitle = _blazor.MenuTitle;
            blazor.MenuConfig = _blazor.MenuConfig;
            blazor.ModulesConfig = _blazor.ModulesConfig;
            return blazor;
        }

        public virtual async Task<BlazorEntity?> GetByNameAsync(string _name) =>
            await collection_.Find(x => x.Name.Equals(_name)).FirstOrDefaultAsync();

        public virtual async Task<KeyValuePair<long, List<BlazorEntity>>> SearchAsync(int _offset, int _count, string _name, string _display)
        {
            var filter = Builders<BlazorEntity>.Filter.Where(x =>
                            (string.IsNullOrWhiteSpace(_name) || (null != x.Name && x.Name.ToLower().Contains(_name.ToLower()))) &&
                            (string.IsNullOrWhiteSpace(_display) || (null != x.Display && x.Display.ToLower().Contains(_display.ToLower())))
                        );

            var found = collection_.Find(filter);

            var total = await found.CountDocumentsAsync();
            var blazorS = await found.Skip((int)_offset).Limit((int)_count).ToListAsync();

            return new KeyValuePair<long, List<BlazorEntity>>(total, blazorS);
        }
    }
}
