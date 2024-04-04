using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace HardwareManagement.Client.Pages
{
    public partial class TresosAutoCores
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public FocusDBService FocusDBService { get; set; }

        protected IEnumerable<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> tresosAutoCores;

        protected RadzenDataGrid<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> grid0;
        protected int count;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            await grid0.Reload();
        }

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await FocusDBService.GetTresosAutoCores(filter: $@"(contains(MultiCore,""{search}"") or contains(DevDrop,""{search}"") or contains(RFM,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "MicroControllerSubDerivative,Architecture,AvailabilityStatus,CompilerVendor,CompilerVersion", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                tresosAutoCores = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load TresosAutoCores" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddTresosAutoCore>("Add TresosAutoCore", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> args)
        {
            await DialogService.OpenAsync<EditTresosAutoCore>("Edit TresosAutoCore", new Dictionary<string, object> { {"TresosAutoCoreID", args.Data.TresosAutoCoreID} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, HardwareManagement.Server.Models.FocusDB.TresosAutoCore tresosAutoCore)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await FocusDBService.DeleteTresosAutoCore(tresosAutoCoreId:tresosAutoCore.TresosAutoCoreID);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete TresosAutoCore"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await FocusDBService.ExportTresosAutoCoresToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "MicroControllerSubDerivative,Architecture,AvailabilityStatus,CompilerVendor,CompilerVersion",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "TresosAutoCores");
            }

            if (args == null || args.Value == "xlsx")
            {
                await FocusDBService.ExportTresosAutoCoresToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "MicroControllerSubDerivative,Architecture,AvailabilityStatus,CompilerVendor,CompilerVersion",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "TresosAutoCores");
            }
        }
    }
}