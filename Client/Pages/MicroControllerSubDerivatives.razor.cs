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
    public partial class MicroControllerSubDerivatives
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

        protected IEnumerable<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> microControllerSubDerivatives;

        protected RadzenDataGrid<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> grid0;
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
                var result = await FocusDBService.GetMicroControllerSubDerivatives(filter: $@"(contains(MicroControllerSubDerivativesName,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                microControllerSubDerivatives = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load MicroControllerSubDerivatives" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddMicroControllerSubDerivative>("Add MicroControllerSubDerivative", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> args)
        {
            await DialogService.OpenAsync<EditMicroControllerSubDerivative>("Edit MicroControllerSubDerivative", new Dictionary<string, object> { {"MicroControllerSubDerivativesID", args.Data.MicroControllerSubDerivativesID} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative microControllerSubDerivative)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await FocusDBService.DeleteMicroControllerSubDerivative(microControllerSubDerivativesId:microControllerSubDerivative.MicroControllerSubDerivativesID);

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
                    Detail = $"Unable to delete MicroControllerSubDerivative"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await FocusDBService.ExportMicroControllerSubDerivativesToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "MicroControllerSubDerivatives");
            }

            if (args == null || args.Value == "xlsx")
            {
                await FocusDBService.ExportMicroControllerSubDerivativesToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "MicroControllerSubDerivatives");
            }
        }
    }
}