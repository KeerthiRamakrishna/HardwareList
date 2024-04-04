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
    public partial class AddMicroController
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
            microController = new HardwareManagement.Server.Models.FocusDB.MicroController();
        }
        protected bool errorVisible;
        protected HardwareManagement.Server.Models.FocusDB.MicroController microController;

        protected IEnumerable<HardwareManagement.Server.Models.FocusDB.SiliconVendor> siliconVendorsForSiliconVendorID;

        protected IEnumerable<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> microControllerDerivativesForMicroControllerDerivativesID;

        protected IEnumerable<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> microControllerSubDerivativesForMicroControllerSubDerivativesID;

        protected IEnumerable<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> availabilityStatusesForAvailabilityStatusID;


        protected int siliconVendorsForSiliconVendorIDCount;
        protected HardwareManagement.Server.Models.FocusDB.SiliconVendor siliconVendorsForSiliconVendorIDValue;
        protected async Task siliconVendorsForSiliconVendorIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await FocusDBService.GetSiliconVendors(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(SiliconVendorName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                siliconVendorsForSiliconVendorID = result.Value.AsODataEnumerable();
                siliconVendorsForSiliconVendorIDCount = result.Count;

                if (!object.Equals(microController.SiliconVendorID, null))
                {
                    var valueResult = await FocusDBService.GetSiliconVendors(filter: $"SiliconVendorID eq {microController.SiliconVendorID}");
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

        protected int microControllerDerivativesForMicroControllerDerivativesIDCount;
        protected HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative microControllerDerivativesForMicroControllerDerivativesIDValue;
        protected async Task microControllerDerivativesForMicroControllerDerivativesIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await FocusDBService.GetMicroControllerDerivatives(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(MicroControllerDerivativesName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                microControllerDerivativesForMicroControllerDerivativesID = result.Value.AsODataEnumerable();
                microControllerDerivativesForMicroControllerDerivativesIDCount = result.Count;

                if (!object.Equals(microController.MicroControllerDerivativesID, null))
                {
                    var valueResult = await FocusDBService.GetMicroControllerDerivatives(filter: $"MicroControllerDerivativesID eq {microController.MicroControllerDerivativesID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        microControllerDerivativesForMicroControllerDerivativesIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load MicroControllerDerivative" });
            }
        }

        protected int microControllerSubDerivativesForMicroControllerSubDerivativesIDCount;
        protected HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative microControllerSubDerivativesForMicroControllerSubDerivativesIDValue;
        protected async Task microControllerSubDerivativesForMicroControllerSubDerivativesIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await FocusDBService.GetMicroControllerSubDerivatives(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(MicroControllerSubDerivativesName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                microControllerSubDerivativesForMicroControllerSubDerivativesID = result.Value.AsODataEnumerable();
                microControllerSubDerivativesForMicroControllerSubDerivativesIDCount = result.Count;

                if (!object.Equals(microController.MicroControllerSubDerivativesID, null))
                {
                    var valueResult = await FocusDBService.GetMicroControllerSubDerivatives(filter: $"MicroControllerSubDerivativesID eq {microController.MicroControllerSubDerivativesID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        microControllerSubDerivativesForMicroControllerSubDerivativesIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load MicroControllerSubDerivative" });
            }
        }

        protected int availabilityStatusesForAvailabilityStatusIDCount;
        protected HardwareManagement.Server.Models.FocusDB.AvailabilityStatus availabilityStatusesForAvailabilityStatusIDValue;
        protected async Task availabilityStatusesForAvailabilityStatusIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await FocusDBService.GetAvailabilityStatuses(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(AvailabilityStatusName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                availabilityStatusesForAvailabilityStatusID = result.Value.AsODataEnumerable();
                availabilityStatusesForAvailabilityStatusIDCount = result.Count;

                if (!object.Equals(microController.AvailabilityStatusID, null))
                {
                    var valueResult = await FocusDBService.GetAvailabilityStatuses(filter: $"AvailabilityStatusID eq {microController.AvailabilityStatusID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        availabilityStatusesForAvailabilityStatusIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load AvailabilityStatus" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await FocusDBService.CreateMicroController(microController);
                DialogService.Close(microController);
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