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
    public partial class EditTresosAcg
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

        [Parameter]
        public int TresosACGID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            tresosAcg = await FocusDBService.GetTresosAcgByTresosAcgid(tresosAcgid:TresosACGID);
        }
        protected bool errorVisible;
        protected HardwareManagement.Server.Models.FocusDB.TresosAcg tresosAcg;

        protected IEnumerable<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> microControllerSubDerivativesForMicroControllerSubDerivativesID;

        protected IEnumerable<HardwareManagement.Server.Models.FocusDB.Architecture> architecturesForArchitectureID;

        protected IEnumerable<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> availabilityStatusesForAvailabilityStatusID;

        protected IEnumerable<HardwareManagement.Server.Models.FocusDB.AutosarVersion> autosarVersionsForAUTOSARVersionID;

        protected IEnumerable<HardwareManagement.Server.Models.FocusDB.SiliconVendor> siliconVendorsForSiliconVendorID;

        protected IEnumerable<HardwareManagement.Server.Models.FocusDB.CompilerVendor> compilerVendorsForCompilerVendorID;

        protected IEnumerable<HardwareManagement.Server.Models.FocusDB.CompilerVersion> compilerVersionsForCompilerVersionID;


        protected int microControllerSubDerivativesForMicroControllerSubDerivativesIDCount;
        protected HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative microControllerSubDerivativesForMicroControllerSubDerivativesIDValue;
        protected async Task microControllerSubDerivativesForMicroControllerSubDerivativesIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await FocusDBService.GetMicroControllerSubDerivatives(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(MicroControllerSubDerivativesName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                microControllerSubDerivativesForMicroControllerSubDerivativesID = result.Value.AsODataEnumerable();
                microControllerSubDerivativesForMicroControllerSubDerivativesIDCount = result.Count;

                if (!object.Equals(tresosAcg.MicroControllerSubDerivativesID, null))
                {
                    var valueResult = await FocusDBService.GetMicroControllerSubDerivatives(filter: $"MicroControllerSubDerivativesID eq {tresosAcg.MicroControllerSubDerivativesID}");
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

        protected int architecturesForArchitectureIDCount;
        protected HardwareManagement.Server.Models.FocusDB.Architecture architecturesForArchitectureIDValue;
        protected async Task architecturesForArchitectureIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await FocusDBService.GetArchitectures(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Architecture1, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                architecturesForArchitectureID = result.Value.AsODataEnumerable();
                architecturesForArchitectureIDCount = result.Count;

                if (!object.Equals(tresosAcg.ArchitectureID, null))
                {
                    var valueResult = await FocusDBService.GetArchitectures(filter: $"ArchitectureID eq {tresosAcg.ArchitectureID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        architecturesForArchitectureIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Architecture" });
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

                if (!object.Equals(tresosAcg.AvailabilityStatusID, null))
                {
                    var valueResult = await FocusDBService.GetAvailabilityStatuses(filter: $"AvailabilityStatusID eq {tresosAcg.AvailabilityStatusID}");
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

        protected int autosarVersionsForAUTOSARVersionIDCount;
        protected HardwareManagement.Server.Models.FocusDB.AutosarVersion autosarVersionsForAUTOSARVersionIDValue;
        protected async Task autosarVersionsForAUTOSARVersionIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await FocusDBService.GetAutosarVersions(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(AUTOSARVersion1, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                autosarVersionsForAUTOSARVersionID = result.Value.AsODataEnumerable();
                autosarVersionsForAUTOSARVersionIDCount = result.Count;

                if (!object.Equals(tresosAcg.AUTOSARVersionID, null))
                {
                    var valueResult = await FocusDBService.GetAutosarVersions(filter: $"AUTOSARVersionID eq {tresosAcg.AUTOSARVersionID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        autosarVersionsForAUTOSARVersionIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Autosarversion" });
            }
        }

        protected int siliconVendorsForSiliconVendorIDCount;
        protected HardwareManagement.Server.Models.FocusDB.SiliconVendor siliconVendorsForSiliconVendorIDValue;
        protected async Task siliconVendorsForSiliconVendorIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await FocusDBService.GetSiliconVendors(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(SiliconVendorName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                siliconVendorsForSiliconVendorID = result.Value.AsODataEnumerable();
                siliconVendorsForSiliconVendorIDCount = result.Count;

                if (!object.Equals(tresosAcg.SiliconVendorID, null))
                {
                    var valueResult = await FocusDBService.GetSiliconVendors(filter: $"SiliconVendorID eq {tresosAcg.SiliconVendorID}");
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

        protected int compilerVendorsForCompilerVendorIDCount;
        protected HardwareManagement.Server.Models.FocusDB.CompilerVendor compilerVendorsForCompilerVendorIDValue;
        protected async Task compilerVendorsForCompilerVendorIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await FocusDBService.GetCompilerVendors(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(CompilerVendorName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                compilerVendorsForCompilerVendorID = result.Value.AsODataEnumerable();
                compilerVendorsForCompilerVendorIDCount = result.Count;

                if (!object.Equals(tresosAcg.CompilerVendorID, null))
                {
                    var valueResult = await FocusDBService.GetCompilerVendors(filter: $"CompilerVendorID eq {tresosAcg.CompilerVendorID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        compilerVendorsForCompilerVendorIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load CompilerVendor" });
            }
        }

        protected int compilerVersionsForCompilerVersionIDCount;
        protected HardwareManagement.Server.Models.FocusDB.CompilerVersion compilerVersionsForCompilerVersionIDValue;
        protected async Task compilerVersionsForCompilerVersionIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await FocusDBService.GetCompilerVersions(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(CompilerVersionName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                compilerVersionsForCompilerVersionID = result.Value.AsODataEnumerable();
                compilerVersionsForCompilerVersionIDCount = result.Count;

                if (!object.Equals(tresosAcg.CompilerVersionID, null))
                {
                    var valueResult = await FocusDBService.GetCompilerVersions(filter: $"CompilerVersionID eq {tresosAcg.CompilerVersionID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        compilerVersionsForCompilerVersionIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load CompilerVersion" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await FocusDBService.UpdateTresosAcg(tresosAcgid:TresosACGID, tresosAcg);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(tresosAcg);
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


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
            hasChanges = false;
            canEdit = true;

            tresosAcg = await FocusDBService.GetTresosAcgByTresosAcgid(tresosAcgid:TresosACGID);
        }
    }
}