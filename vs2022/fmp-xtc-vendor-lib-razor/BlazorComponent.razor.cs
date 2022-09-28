using Microsoft.AspNetCore.Components;
using XTC.FMP.LIB.MVCS;
using XTC.FMP.MOD.Vendor.LIB.Proto;
using XTC.FMP.MOD.Vendor.LIB.Bridge;
using XTC.FMP.MOD.Vendor.LIB.MVCS;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using AntDesign;
using Microsoft.JSInterop;

namespace XTC.FMP.MOD.Vendor.LIB.Razor
{
    public partial class BlazorComponent
    {
        public class BlazorUiBridge : IBlazorUiBridge
        {

            public BlazorUiBridge(BlazorComponent _razor)
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
                var dto = _dto as BlazorRetrieveResponseDTO;
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
                var dto = _dto as BlazorListResponseDTO;
                if (null == dto)
                    return;

                razor_.tableTotal = (int)dto.Value.Total;
                razor_.tableModel.Clear();
                foreach (var blazor in dto.Value.BlazorS)
                {
                    var item = new TableModel
                    {
                        Entity = blazor,
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

            private BlazorComponent razor_;
        }

        [Inject] IJSRuntime? jsRuntime_ { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            searchFormData[SearchField.Name.GetHashCode()] = new FormValue { Text = "Ãû³Æ", Value = "" };
            searchFormData[SearchField.Display.GetHashCode()] = new FormValue { Text = "ÏÔÊ¾Ãû", Value = "" };
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
            var bridge = (getFacade()?.getViewBridge() as IBlazorViewBridge);
            if (null == bridge)
            {
                logger_?.Error("bridge is null");
                return;
            }
            var req = new BlazorSearchRequest();
            req.Offset = (tablePageIndex - 1) * tablePageSize;
            req.Count = tablePageSize;
            req.Name = searchFormData[SearchField.Name.GetHashCode()].Value ?? "";
            var dto = new BlazorSearchRequestDTO(req);
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
            var bridge = (getFacade()?.getViewBridge() as IBlazorViewBridge);
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
            var req = new BlazorCreateRequest();
            req.Name = model.Name;
            req.Display = model.Display ?? "";
            var dto = new BlazorCreateRequestDTO(req);
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
            public BlazorEntity Entity { get; set; } = new BlazorEntity();
            public string MenuConfig { get; set; } = "";
            public string ModulesConfig { get; set; } = "";
            public string SkinConfig { get; set; } = "";
        }

        private bool visibleUpdateModal = false;
        private bool updateLoading = false;
        private AntDesign.Internal.IForm? updateForm;
        private UpdateModel updateModel = new();

        private void onUpdateClick(string? _uuid)
        {
            if (string.IsNullOrEmpty(_uuid))
                return;

            var blazor = tableModel.Find((x) =>
            {
                if (string.IsNullOrEmpty(x.Entity?.Uuid))
                    return false;
                return x.Entity.Uuid.Equals(_uuid);
            });
            if (null == blazor)
                return;

            visibleUpdateModal = true;
            updateModel.Entity = Utilities.DeepCloneProtoBlazorEntity(blazor.Entity);
            updateModel.MenuConfig = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(blazor.Entity.MenuConfig));
            updateModel.ModulesConfig = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(blazor.Entity.ModulesConfig));
            updateModel.SkinConfig = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(blazor.Entity.SkinConfig));
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
            var bridge = (getFacade()?.getViewBridge() as IBlazorViewBridge);
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
            var req = new BlazorUpdateRequest();
            req.Uuid = model.Entity.Uuid;
            req.Name = model.Entity.Name;
            req.Display = model.Entity.Display;
            req.Logo = model.Entity.Logo;
            req.MenuTitle = model.Entity.MenuTitle;
            req.MenuConfig = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(model.MenuConfig));
            req.ModulesConfig = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(model.ModulesConfig));
            req.SkinConfig = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(model.SkinConfig));
            var dto = new BlazorUpdateRequestDTO(req);
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
            public BlazorEntity Entity { get; set; } = new BlazorEntity();
        }


        private List<TableModel> tableModel = new();
        private TableModel? selectedModel = null;
        private int tableTotal = 0;
        private int tablePageIndex = 1;
        private int tablePageSize = 50;

        private async Task listAll()
        {
            var bridge = (getFacade()?.getViewBridge() as IBlazorViewBridge);
            if (null == bridge)
            {
                logger_?.Error("bridge is null");
                return;
            }
            var req = new BlazorListRequest();
            req.Offset = (tablePageIndex - 1) * tablePageSize;
            req.Count = tablePageSize;
            var dto = new BlazorListRequestDTO(req);
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

            var bridge = (getFacade()?.getViewBridge() as IBlazorViewBridge);
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

            var blazor = tableModel.Find((x) =>
            {
                if (string.IsNullOrEmpty(x.Entity?.Uuid))
                    return false;
                return x.Entity.Uuid.Equals(_uuid);
            });
            if (null == blazor)
                return;

            if (null != jsRuntime_)
            {
                string uri = string.Format("?vendor={0}", blazor.Entity.Uuid);
                await jsRuntime_.InvokeAsync<object>("open", uri, "_blank");
            }
        }
    }
}
