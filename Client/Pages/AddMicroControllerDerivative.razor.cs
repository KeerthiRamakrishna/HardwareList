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
    public partial class AddMicroControllerDerivative
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

        protected override async Task OnInitializedAsync()
        {
            microControllerDerivative = new HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative();
        }
        protected bool errorVisible;
        protected HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative microControllerDerivative;

        protected IEnumerable<HardwareManagement.Server.Models.FocusDB.SiliconVendor> siliconVendorsForSiliconVendorID;


        protected int siliconVendorsForSiliconVendorIDCount;
        protected HardwareManagement.Server.Models.FocusDB.SiliconVendor siliconVendorsForSiliconVendorIDValue;
        protected async Task siliconVendorsForSiliconVendorIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await FocusDBService.GetSiliconVendors(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(SiliconVendorName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                siliconVendorsForSiliconVendorID = result.Value.AsODataEnumerable();
                siliconVendorsForSiliconVendorIDCount = result.Count;

                if (!object.Equals(microControllerDerivative.SiliconVendorID, null))
                {
                    var valueResult = await FocusDBService.GetSiliconVendors(filter: $"SiliconVendorID eq {microControllerDerivative.SiliconVendorID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        siliconVendorsForSiliconVendorIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load SiliconVendor" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await FocusDBService.CreateMicroControllerDerivative(microControllerDerivative);
                DialogService.Close(microControllerDerivative);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;
    }
}