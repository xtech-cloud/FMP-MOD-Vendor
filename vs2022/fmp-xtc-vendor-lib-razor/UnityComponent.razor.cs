using Microsoft.AspNetCore.Components;
using XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.Vendor.LIB.Proto;
using XTC.FMP.MOD.Vendor.LIB.Bridge;
using XTC.FMP.MOD.Vendor.LIB.MVCS;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel;
using AntDesign;
using AntDesign.TableModels;
using Microsoft.JSInterop;

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

            private UnityComponent razor_;
        }

        [Inject] IJSRuntime? jsRuntime_ { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            searchFormData[SearchField.Name.GetHashCode()] = new FormValue { Text = "����", Value = "" };
            searchFormData[SearchField.Display.GetHashCode()] = new FormValue { Text = "��ʾ��", Value = "" };
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
            public UnityEntity Entity { get; set; } = new UnityEntity();
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

            public string _GraphicsReferenceResolution
            {
                get
                {
                    return string.Format("{0}x{1}", Entity.GraphicsReferenceResolutionWidth, Entity.GraphicsReferenceResolutionHeight);
                }
            }

            public string _GraphicsQuality
            {
                get
                {
                    return Utilities.QualityToString(Entity.GraphicsQuality);
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
    }
}
