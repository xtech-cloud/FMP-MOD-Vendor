using Microsoft.AspNetCore.Components;
using XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.Vendor.LIB.Proto;
using XTC.FMP.MOD.Vendor.LIB.Bridge;
using XTC.FMP.MOD.Vendor.LIB.MVCS;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using AntDesign;
using Microsoft.JSInterop;
using System.Text;
using Newtonsoft.Json;

namespace XTC.FMP.MOD.Vendor.LIB.Razor
{
    public partial class UnityComponent
    {
        public class UnityUiBridge : IUnityUiBridge
        {

            public UnityUiBridge(UnityComponent _razor)
            {
                razor_ = _razor;
            }

            public void Alert(string _code, string _message, object? _context)
            {
                if (null == razor_.messageService_)
                    return;
                Task.Run(async () =>
                {
                    await razor_.messageService_.Error(_message);
                    razor_.createLoading = false;
                    razor_.StateHasChanged();
                });
            }


            public void RefreshCreate(IDTO _dto, object? _context)
            {
                razor_.createLoading = false;
                razor_.visibleCreateModal = false;
                razor_.StateHasChanged();

                Task.Run(async () =>
                {
                    await razor_.listAll();
                });
            }

            public void RefreshUpdate(IDTO _dto, object? _context)
            {
                razor_.updateLoading = false;
                razor_.visibleUpdateModal = false;
                razor_.StateHasChanged();

                Task.Run(async () =>
                {
                    await razor_.listAll();
                });
            }


            public void RefreshRetrieve(IDTO _dto, object? _context)
            {
                var dto = _dto as UnityRetrieveResponseDTO;
            }

            public void RefreshDelete(IDTO _dto, object? _context)
            {
                var dto = _dto as UuidResponseDTO;
                if (null == dto)
                    return;
                razor_.tableModel.RemoveAll((_item) =>
                {
                    return _item.Entity?.Uuid?.Equals(dto.Value.Uuid) ?? false;
                });
                razor_.selectedModel = null;
            }

            public void RefreshList(IDTO _dto, object? _context)
            {
                var dto = _dto as UnityListResponseDTO;
                if (null == dto)
                    return;

                razor_.tableTotal = (int)dto.Value.Total;
                razor_.tableModel.Clear();
                foreach (var unity in dto.Value.UnityS)
                {
                    var item = new TableModel
                    {
                        Entity = unity,
                    };

                    try
                    {
                        item._dependencyConfig = Utilities.FromBase64XML<UnityModel.DependencyConfig>(item.Entity.DependencyConfig) ?? new UnityModel.DependencyConfig();
                    }
                    catch (System.Exception ex)
                    {
                        razor_.logger_?.Exception(ex);
                    }
                    try
                    {
                        item._bootloaderConfig = Utilities.FromBase64XML<UnityModel.BootloaderConfig>(item.Entity.BootloaderConfig) ?? new UnityModel.BootloaderConfig();
                    }
                    catch (System.Exception ex)
                    {
                        razor_.logger_?.Exception(ex);
                    }
                    try
                    {
                        item._updateConfig = Utilities.FromBase64XML<UnityModel.UpdateConfig>(item.Entity.UpdateConfig) ?? new UnityModel.UpdateConfig();
                    }
                    catch (System.Exception ex)
                    {
                        razor_.logger_?.Exception(ex);
                    }
                    razor_.tableModel.Add(item);
                }
                razor_.selectedModel = null;
                razor_.StateHasChanged();
            }

            public void RefreshSearch(IDTO _dto, object? _context)
            {
                razor_.searchLoading = false;
                RefreshList(_dto, _context);
            }

            public void RefreshPrepareUploadTheme(IDTO _dto, object? _context)
            {
                var dto = _dto as PrepareUploadResponseDTO;
                if (null == dto)
                    return;

                // 使用预签名的地址上传数据
                Task.Run(async () => await razor_.uploadTheme(dto.Value.Filepath, dto.Value.Url));
            }

            public void RefreshFlushUploadTheme(IDTO _dto, object? _context)
            {
                var dto = _dto as FlushUploadResponseDTO;
                var file = razor_.uploadThemeFiles_.Find((_item) =>
                {
                    return _item.browserFile.Name.Equals(dto.Value.Filepath);
                });
                if (null == file)
                    return;
                file.percentage = 100;
                razor_.uploadThemeFiles_.Remove(file);
                razor_.StateHasChanged();
                //Task.Run(async () => await razor_.fetchAttachments());
            }

            private UnityComponent razor_;
        }

        [Inject] IJSRuntime? jsRuntime_ { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            searchFormData[SearchField.Name.GetHashCode()] = new FormValue { Text = "名称", Value = "" };
            searchFormData[SearchField.Display.GetHashCode()] = new FormValue { Text = "显示名", Value = "" };
            await listAll();
        }

        #region Search
        private class FormValue
        {
            public string? Text { get; set; }
            public string? Value { get; set; }
        }

        private bool searchLoading = false;
        private AntDesign.Internal.IForm? searchForm;
        private Dictionary<int, FormValue> searchFormData = new();
        private bool searchExpand = false;

        private enum SearchField
        {
            Name,
            Display,
        }

        private async void onSearchSubmit(EditContext _context)
        {
            searchLoading = true;
            var bridge = (getFacade()?.getViewBridge() as IUnityViewBridge);
            if (null == bridge)
            {
                logger_?.Error("bridge is null");
                return;
            }
            var req = new UnitySearchRequest();
            req.Offset = (tablePageIndex - 1) * tablePageSize;
            req.Count = tablePageSize;
            req.Name = searchFormData[SearchField.Name.GetHashCode()].Value ?? "";
            var dto = new UnitySearchRequestDTO(req);
            Error err = await bridge.OnSearchSubmit(dto, null);
            if (!Error.IsOK(err))
            {
                logger_?.Error(err.getMessage());
            }

        }

        private async void onSearchResetClick()
        {
            searchForm?.Reset();
            await listAll();
        }
        #endregion

        #region Create Modal
        private class CreateModel
        {
            [Required]
            public string? Name { get; set; }

            public string? Display { get; set; }
        }

        private bool visibleCreateModal = false;
        private bool createLoading = false;
        private AntDesign.Internal.IForm? createForm;
        private CreateModel createModel = new();

        private void onCreateClick()
        {
            visibleCreateModal = true;
        }

        private void onCreateModalOk()
        {
            createForm?.Submit();
        }

        private void onCreateModalCancel()
        {
            visibleCreateModal = false;
        }

        private async void onCreateSubmit(EditContext _context)
        {
            createLoading = true;
            var bridge = (getFacade()?.getViewBridge() as IUnityViewBridge);
            if (null == bridge)
            {
                logger_?.Error("bridge is null");
                return;
            }
            var model = _context.Model as CreateModel;
            if (null == model)
            {
                logger_?.Error("model is null");
                return;
            }
            var req = new UnityCreateRequest();
            req.Name = model.Name;
            req.Display = model.Display ?? "";
            var dto = new UnityCreateRequestDTO(req);
            Error err = await bridge.OnCreateSubmit(dto, null);
            if (null != err)
            {
                logger_?.Error(err.getMessage());
            }
        }


        #endregion

        #region Update Modal
        private class UpdateModel
        {
            public class StringPair
            {
                public string Key = "";
                public string Value = "";
            }

            public UnityEntity Entity { get; set; } = new UnityEntity();
            public UnityModel.DependencyConfig _dependencyConfig { get; set; } = new UnityModel.DependencyConfig();
            public UnityModel.BootloaderConfig _bootloaderConfig { get; set; } = new UnityModel.BootloaderConfig();
            public UnityModel.UpdateConfig _updateConfig { get; set; } = new UnityModel.UpdateConfig();

            public string _dependencyReferencesInput { get; set; } = "";
            public string _dependencyPluginsInput { get; set; } = "";
            public string _bootloaderStepsInput { get; set; } = "";

            public List<StringPair> _rawModuleCatalogS = new List<StringPair>();
            public List<StringPair> _rawModuleConfigS = new List<StringPair>();

            public string _activeThemeKey { get; set; } = "";
        }

        private bool visibleUpdateModal = false;
        private bool updateLoading = false;
        private AntDesign.Internal.IForm? updateForm;
        private UpdateModel updateModel = new();

        private void onUpdateClick(string? _uuid)
        {
            if (string.IsNullOrEmpty(_uuid))
                return;

            var unity = tableModel.Find((x) =>
            {
                if (string.IsNullOrEmpty(x.Entity?.Uuid))
                    return false;
                return x.Entity.Uuid.Equals(_uuid);
            });
            if (null == unity)
                return;

            visibleUpdateModal = true;
            updateModel.Entity = Utilities.DeepCloneProtoUnityEntity(unity.Entity);
            // 解析模块
            try
            {
                updateModel._dependencyConfig = Utilities.FromBase64XML<UnityModel.DependencyConfig>(updateModel.Entity.DependencyConfig) ?? new UnityModel.DependencyConfig();
            }
            catch (System.Exception ex)
            {
                logger_?.Exception(ex);
            }
            StringBuilder sbReference = new StringBuilder();
            foreach (var reference in updateModel._dependencyConfig.schema.body.references)
            {
                string? referenceStr = Utilities.DependencyReferenceToString(reference) ?? null;
                if (string.IsNullOrEmpty(referenceStr))
                    continue;
                sbReference.AppendLine(referenceStr);
            }
            updateModel._dependencyReferencesInput = sbReference.ToString();
            StringBuilder sbPlugin = new StringBuilder();
            foreach (var plugin in updateModel._dependencyConfig.schema.body.plugins)
            {
                string? pluginStr = Utilities.DependencyPluginToString(plugin) ?? null;
                if (string.IsNullOrEmpty(pluginStr))
                    continue;
                sbPlugin.AppendLine(pluginStr);
            }
            updateModel._dependencyPluginsInput = sbPlugin.ToString();
            // 解析加载
            try
            {
                updateModel._bootloaderConfig = Utilities.FromBase64XML<UnityModel.BootloaderConfig>(updateModel.Entity.BootloaderConfig) ?? new UnityModel.BootloaderConfig();
            }
            catch (System.Exception ex)
            {
                logger_?.Exception(ex);
            }
            // 解析更新
            try
            {
                updateModel._updateConfig = Utilities.FromBase64XML<UnityModel.UpdateConfig>(updateModel.Entity.UpdateConfig) ?? new UnityModel.UpdateConfig();
            }
            catch (System.Exception ex)
            {
                logger_?.Exception(ex);
            }
            StringBuilder sbBootSteps = new StringBuilder();
            foreach (var step in updateModel._bootloaderConfig.schema.steps)
            {
                string? stepStr = Utilities.BootloaderStepToString(step) ?? null;
                if (string.IsNullOrEmpty(stepStr))
                    continue;
                sbBootSteps.AppendLine(stepStr);
            }
            // 解析模块编目
            updateModel._rawModuleCatalogS.Clear();
            foreach (var pair in updateModel.Entity.ModuleCatalogs)
            {
                var catalog = new UpdateModel.StringPair
                {
                    Key = pair.Key,
                    Value = Encoding.UTF8.GetString(Convert.FromBase64String(pair.Value)),
                };
                updateModel._rawModuleCatalogS.Add(catalog);
            }
            // 解析模块配置
            updateModel._rawModuleConfigS.Clear();
            foreach (var pair in updateModel.Entity.ModuleConfigs)
            {
                var config = new UpdateModel.StringPair
                {
                    Key = pair.Key,
                    Value = Encoding.UTF8.GetString(Convert.FromBase64String(pair.Value)),
                };
                updateModel._rawModuleConfigS.Add(config);
            }
            updateModel._bootloaderStepsInput = sbBootSteps.ToString();
        }

        private void onUpdateModalOk()
        {
            updateForm?.Submit();
        }

        private void onUpdateModalCancel()
        {
            visibleUpdateModal = false;
        }

        private async void onUpdateSubmit(EditContext _context)
        {
            updateLoading = true;
            var bridge = (getFacade()?.getViewBridge() as IUnityViewBridge);
            if (null == bridge)
            {
                logger_?.Error("bridge is null");
                return;
            }
            var model = _context.Model as UpdateModel;
            if (null == model)
            {
                logger_?.Error("model is null");
                return;
            }
            // 依赖配置的base64编码
            var referenceS = new List<UnityModel.DependencyConfig.Reference>();
            foreach (var referenceInput in model._dependencyReferencesInput.Split("\n"))
            {
                var reference = Utilities.DependencyReferenceFromString(referenceInput);
                if (null == reference)
                    continue;
                referenceS.Add(reference);
            }
            model._dependencyConfig.schema.body.references = referenceS.ToArray();
            var pluginS = new List<UnityModel.DependencyConfig.Plugin>();
            foreach (var pluginInput in model._dependencyPluginsInput.Split("\n"))
            {
                var plugin = Utilities.DependencyPluginFromString(pluginInput);
                if (null == plugin)
                    continue;
                pluginS.Add(plugin);
            }
            model._dependencyConfig.schema.body.plugins = pluginS.ToArray();
            string dependencyConfigBase64 = Utilities.ToBase64XML(model._dependencyConfig);
            // 加载配置的base64编码
            var bootStepS = new List<UnityModel.BootloaderConfig.BootStep>();
            foreach (var bootStepInput in model._bootloaderStepsInput.Split("\n"))
            {
                var step = Utilities.BootloaderStepFromString(bootStepInput);
                if (null == step)
                    continue;
                bootStepS.Add(step);
            }
            model._bootloaderConfig.schema.steps = bootStepS.ToArray();
            string bootloaderConfigBase64 = Utilities.ToBase64XML(model._bootloaderConfig);
            // 更新配置的base64编码
            string updateConfigBase64 = Utilities.ToBase64XML(model._updateConfig);
            var req = new UnityUpdateRequest();
            req.Uuid = model.Entity.Uuid;
            req.Name = model.Entity.Name;
            req.Display = model.Entity.Display;
            req.SkinSplashBackground = model.Entity.SkinSplashBackground;
            req.SkinSplashSlogan = model.Entity.SkinSplashSlogan;
            req.GraphicsFps = model.Entity.GraphicsFps;
            req.GraphicsQuality = model.Entity.GraphicsQuality;
            req.GraphicsPixelResolution = model.Entity.GraphicsPixelResolution;
            req.GraphicsReferenceResolutionWidth = model.Entity.GraphicsReferenceResolutionWidth;
            req.GraphicsReferenceResolutionHeight = model.Entity.GraphicsReferenceResolutionHeight;
            req.GraphicsReferenceResolutionMatch = model.Entity.GraphicsReferenceResolutionMatch;
            req.DependencyConfig = dependencyConfigBase64;
            req.BootloaderConfig = bootloaderConfigBase64;
            req.UpdateConfig = updateConfigBase64;
            foreach (var pair in model._rawModuleCatalogS)
            {
                req.ModuleCatalogs.Add(pair.Key, Convert.ToBase64String(Encoding.UTF8.GetBytes(pair.Value)));
            }
            foreach (var pair in model._rawModuleConfigS)
            {
                req.ModuleConfigs.Add(pair.Key, Convert.ToBase64String(Encoding.UTF8.GetBytes(pair.Value)));
            }
            foreach (var pair in model.Entity.ModuleThemes)
            {
                req.ModuleThemes[pair.Key] = new FileSubEntityS();
                foreach (var file in pair.Value.EntityS)
                {
                    req.ModuleThemes[pair.Key].EntityS.Add(new FileSubEntity()
                    {
                        Hash = file.Hash,
                        Size = file.Size,
                        Path = file.Path,
                        Url = file.Url,
                    });
                }
            }
            req.Application = model.Entity.Application;
            var dto = new UnityUpdateRequestDTO(req);
            Error err = await bridge.OnUpdateSubmit(dto, null);
            if (null != err)
            {
                logger_?.Error(err.getMessage());
            }
        }
        #endregion


        #region Table
        private class TableModel
        {
            public UnityEntity Entity { get; set; } = new UnityEntity();
            public UnityModel.DependencyConfig _dependencyConfig { get; set; } = new UnityModel.DependencyConfig();
            public UnityModel.BootloaderConfig _bootloaderConfig { get; set; } = new UnityModel.BootloaderConfig();
            public UnityModel.UpdateConfig _updateConfig { get; set; } = new UnityModel.UpdateConfig();

            public string _GraphicsReferenceResolution
            {
                get
                {
                    return string.Format("{0}x{1}", Entity.GraphicsReferenceResolutionWidth, Entity.GraphicsReferenceResolutionHeight);
                }
            }
        }


        private List<TableModel> tableModel = new();
        private TableModel? selectedModel = null;
        private int tableTotal = 0;
        private int tablePageIndex = 1;
        private int tablePageSize = 50;

        private async Task listAll()
        {
            var bridge = (getFacade()?.getViewBridge() as IUnityViewBridge);
            if (null == bridge)
            {
                logger_?.Error("bridge is null");
                return;
            }
            var req = new UnityListRequest();
            req.Offset = (tablePageIndex - 1) * tablePageSize;
            req.Count = tablePageSize;
            var dto = new UnityListRequestDTO(req);
            Error err = await bridge.OnListSubmit(dto, null);
            if (!Error.IsOK(err))
            {
                logger_?.Error(err.getMessage());
            }
        }

        private async Task onConfirmDelete(string? _uuid)
        {
            if (string.IsNullOrEmpty(_uuid))
                return;

            var bridge = (getFacade()?.getViewBridge() as IUnityViewBridge);
            if (null == bridge)
            {
                logger_?.Error("bridge is null");
                return;
            }
            var req = new UuidRequest();
            req.Uuid = _uuid;
            var dto = new UuidRequestDTO(req);
            Error err = await bridge.OnDeleteSubmit(dto, null);
            if (!Error.IsOK(err))
            {
                logger_?.Error(err.getMessage());
            }
        }

        private void onCancelDelete()
        {
            //Nothing to do
        }

        private async void onPageIndexChanged(PaginationEventArgs args)
        {
            tablePageIndex = args.Page;
            await listAll();
        }
        #endregion

        #region Browse
        private bool visibleBrowseModal = false;
        private void onBrowseClick(string? _uuid)
        {
            if (string.IsNullOrEmpty(_uuid))
                return;

            var unity = tableModel.Find((x) =>
            {
                if (string.IsNullOrEmpty(x.Entity?.Uuid))
                    return false;
                return x.Entity.Uuid.Equals(_uuid);
            });
            if (null == unity)
                return;

            visibleBrowseModal = true;
            updateModel.Entity = Utilities.DeepCloneProtoUnityEntity(unity.Entity);
            // 解析模块
            try
            {
                updateModel._dependencyConfig = Utilities.FromBase64XML<UnityModel.DependencyConfig>(updateModel.Entity.DependencyConfig) ?? new UnityModel.DependencyConfig();
            }
            catch (System.Exception ex)
            {
                logger_?.Exception(ex);
            }
            StringBuilder sbReference = new StringBuilder();
            foreach (var reference in updateModel._dependencyConfig.schema.body.references)
            {
                string? referenceStr = Utilities.DependencyReferenceToString(reference) ?? null;
                if (string.IsNullOrEmpty(referenceStr))
                    continue;
                sbReference.AppendLine(referenceStr);
            }
            updateModel._dependencyReferencesInput = sbReference.ToString();
            StringBuilder sbPlugin = new StringBuilder();
            foreach (var plugin in updateModel._dependencyConfig.schema.body.plugins)
            {
                string? pluginStr = Utilities.DependencyPluginToString(plugin) ?? null;
                if (string.IsNullOrEmpty(pluginStr))
                    continue;
                sbPlugin.AppendLine(pluginStr);
            }
            updateModel._dependencyPluginsInput = sbPlugin.ToString();
            // 解析加载
            try
            {
                updateModel._bootloaderConfig = Utilities.FromBase64XML<UnityModel.BootloaderConfig>(updateModel.Entity.BootloaderConfig) ?? new UnityModel.BootloaderConfig();
            }
            catch (System.Exception ex)
            {
                logger_?.Exception(ex);
            }
            // 解析更新
            try
            {
                updateModel._updateConfig = Utilities.FromBase64XML<UnityModel.UpdateConfig>(updateModel.Entity.UpdateConfig) ?? new UnityModel.UpdateConfig();
            }
            catch (System.Exception ex)
            {
                logger_?.Exception(ex);
            }
            StringBuilder sbBootSteps = new StringBuilder();
            foreach (var step in updateModel._bootloaderConfig.schema.steps)
            {
                string? stepStr = Utilities.BootloaderStepToString(step) ?? null;
                if (string.IsNullOrEmpty(stepStr))
                    continue;
                sbBootSteps.AppendLine(stepStr);
            }
            // 解析模块编目
            updateModel._rawModuleCatalogS.Clear();
            foreach (var pair in updateModel.Entity.ModuleCatalogs)
            {
                var catalog = new UpdateModel.StringPair
                {
                    Key = pair.Key,
                    Value = Encoding.UTF8.GetString(Convert.FromBase64String(pair.Value)),
                };
                updateModel._rawModuleCatalogS.Add(catalog);
            }
            // 解析模块配置
            updateModel._rawModuleConfigS.Clear();
            foreach (var pair in updateModel.Entity.ModuleConfigs)
            {
                var config = new UpdateModel.StringPair
                {
                    Key = pair.Key,
                    Value = Encoding.UTF8.GetString(Convert.FromBase64String(pair.Value)),
                };
                updateModel._rawModuleConfigS.Add(config);
            }
            updateModel._bootloaderStepsInput = sbBootSteps.ToString();
        }

        private void onBrowseModalCancel()
        {
            visibleBrowseModal = false;
        }
        #endregion

        private async Task onRunClick(string? _uuid)
        {
            if (string.IsNullOrEmpty(_uuid))
                return;

            var unity = tableModel.Find((x) =>
            {
                if (string.IsNullOrEmpty(x.Entity?.Uuid))
                    return false;
                return x.Entity.Uuid.Equals(_uuid);
            });
            if (null == unity)
                return;

            if (null != jsRuntime_)
            {
                string uri = string.Format("{0}?vendor={1}", unity.Entity.Application, unity.Entity.Uuid);
                await jsRuntime_.InvokeAsync<object>("open", uri, "_blank");
            }
        }

        private async Task onDownloadClick(string? _uuid)
        {
            if (null != jsRuntime_)
            {
                string uri = string.Format("http://minio.xtech.cloud/fmp.vendor/unity/{0}.json", _uuid);
                await jsRuntime_.InvokeAsync<object>("open", uri, "_blank");
            }
        }

        private void onDependencyBlur()
        {
            List<UpdateModel.StringPair> catalogS = new();
            List<UpdateModel.StringPair> configS = new();
            Dictionary<string, FileSubEntityS> themeS = new();
            catalogS.AddRange(updateModel._rawModuleCatalogS);
            configS.AddRange(updateModel._rawModuleConfigS);
            updateModel._rawModuleCatalogS.Clear();
            updateModel._rawModuleConfigS.Clear();
            foreach (var pair in updateModel.Entity.ModuleThemes)
            {
                themeS.Add(pair.Key, pair.Value);
            }
            foreach (var referenceInput in updateModel._dependencyReferencesInput.Split("\n"))
            {
                var reference = Utilities.DependencyReferenceFromString(referenceInput);
                if (null == reference)
                    continue;
                string module = string.Format("{0}_{1}", reference.org, reference.module);

                // 更新大纲
                var catalog = catalogS.Find((_item) =>
                {
                    return _item.Key.Equals(module);
                });
                var catalogPair = new UpdateModel.StringPair
                {
                    Key = module,
                };
                catalogPair.Value = null == catalog ? "" : catalog.Value;
                updateModel._rawModuleCatalogS.Add(catalogPair);

                // 更新配置
                var config = configS.Find((_item) =>
                {
                    return _item.Key.Equals(module);
                });
                var configPair = new UpdateModel.StringPair
                {
                    Key = module,
                };
                configPair.Value = null == config ? "" : config.Value;
                updateModel._rawModuleConfigS.Add(configPair);

                // 更新主题
                FileSubEntityS? files;
                if (!themeS.TryGetValue(module, out files))
                {
                    files = new FileSubEntityS();
                }
                updateModel.Entity.ModuleThemes[module] = files;

            }
        }

        #region UploadTheme
        public class UploadThemeFile
        {
            public UploadThemeFile(IBrowserFile _file)
            {
                browserFile = _file;
            }
            public string vendorUuid = "";
            public string moduleKey = "";
            public IBrowserFile browserFile { get; private set; }
            public string uploadUrl = "";
            public int percentage = 0;
        }

        private List<UploadThemeFile> uploadThemeFiles_ = new List<UploadThemeFile>();

        /// <summary>
        /// 对需要上传的主题文件进行预签名
        /// </summary>
        /// <remarks>
        /// 将文件名提交给服务端，获取签名的上传地址，获取到上传地址后再上传原文件
        /// </remarks>
        /// <param name="_e"></param>
        /// <returns></returns>
        private async Task onUploadThemeFilesClick(InputFileChangeEventArgs _e)
        {
            uploadThemeFiles_.Clear();
            if (null == updateModel)
                return;
            var bridge = (getFacade()?.getViewBridge() as IUnityViewBridge);
            if (null == bridge)
            {
                logger_?.Error("bridge is null");
                return;
            }

            int maxAllowedFiles = 100;
            foreach (var file in _e.GetMultipleFiles(maxAllowedFiles))
            {
                var req = new PrepareUploadRequest();
                req.Uuid = updateModel.Entity.Uuid ?? "";
                req.Filepath = string.Format("{0}", file.Name);
                req.Module = updateModel._activeThemeKey;
                var dto = new PrepareUploadRequestDTO(req);
                Error err = await bridge.OnPrepareUploadThemeSubmit(dto, null);
                if (!Error.IsOK(err))
                {
                    logger_?.Error(err.getMessage());
                }
                var uploadFile = new UploadThemeFile(file);
                uploadFile.vendorUuid = updateModel.Entity.Uuid ?? "";
                uploadFile.moduleKey = updateModel._activeThemeKey;
                uploadThemeFiles_.Add(uploadFile);
            }
        }

        /// <summary>
        /// 上传文件数据
        /// </summary>
        /// <param name="_filepath"></param>
        /// <param name="_url"></param>
        /// <returns></returns>
        private async Task uploadTheme(string _filepath, string _url)
        {
            var uploadfile = uploadThemeFiles_.Find((_item) =>
            {
                return _item.browserFile.Name.Equals(_filepath);
            });
            if (null == uploadfile)
                return;
            uploadfile.uploadUrl = _url;

            var httpClient = new HttpClient();
            bool success = false;
            try
            {
                long maxFileSize = long.MaxValue;
                var fileContent = new StreamContent(uploadfile.browserFile.OpenReadStream(maxFileSize));
                var response = await httpClient.PutAsync(new Uri(uploadfile.uploadUrl), fileContent);
                success = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger_?.Error(ex.Message);
            }

            if (!success)
                return;

            var bridge = (getFacade()?.getViewBridge() as IUnityViewBridge);
            if (null == bridge)
            {
                logger_?.Error("bridge is null");
                return;
            }
            var req = new FlushUploadRequest();
            req.Uuid = uploadfile.vendorUuid;
            req.Filepath = uploadfile.browserFile.Name;
            req.Module = uploadfile.moduleKey;
            var dto = new FlushUploadRequestDTO(req);
            Error err = await bridge.OnFlushUploadThemeSubmit(dto, null);
            if (!Error.IsOK(err))
            {
                logger_?.Error(err.getMessage());
            }
        }

        private void onThemeActiveKeyChanged(string _e)
        {
            updateModel._activeThemeKey = _e;
        }


        #endregion
    }
}
