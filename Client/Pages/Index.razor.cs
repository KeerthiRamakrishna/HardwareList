using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using HardwareManagement.Server.Models.FocusDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace HardwareManagement.Client.Pages
{
    public partial class Index
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

        protected int MicrocontrollerCount;
        protected int SiliconVendorCount;
        protected int MicrocontrollerDerivativeCount;
        protected int MicrocontrollerSubDerivativeCount;

        protected IEnumerable<HardwareManagement.Server.Models.FocusDB.MicroController> microControllers;
        protected string search;

        bool showDataLabels = false;

        IEnumerable<ArchitectureGroupForTresosAutoCore> architectureGroupForTresosAutoCore;
        class ArchitectureGroupForTresosAutoCore
        {
            public int CountOfSubDerevatives { get; set; }
            public string ArchitectureName { get; set; }

        }
        IEnumerable<ArchitectureGroupForTresosAutoCoreVendor> architectureGroupForTresosAutoCoreVendor;
        class ArchitectureGroupForTresosAutoCoreVendor
        {
            public int CountOfAvailability { get; set; }
            public string VendorName { get; set; }

        }
        IEnumerable<ArchitectureGroupForTresosAutoCoreAvailability> architectureGroupForTresosAutoCoreAvailability;
        class ArchitectureGroupForTresosAutoCoreAvailability
        {
            public int CountOfAvailability { get; set; }
            public string TresosAutocore { get; set; }
            public string TresosAutocoreColour { get; set; }


        }

        protected override async Task OnInitializedAsync()
        {
            await Grid0LoadData();
        }

        protected async Task Grid0LoadData()
        {
            try
            {
                var result = await FocusDBService.GetMicroControllers();//.GetMicroControllers(filter: $@"({(string.IsNullOrEmpty(args.Filter) ? "true" : args.Filter)}", expand: "SiliconVendor,MicroControllerDerivative,MicroControllerSubDerivative,AvailabilityStatus", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null);
                MicrocontrollerCount = result.Value.Count();

                var result2 = await FocusDBService.GetSiliconVendors();
                SiliconVendorCount = result2.Value.Count();

                var result3 = await FocusDBService.GetMicroControllerDerivatives();
                MicrocontrollerDerivativeCount = result3.Value.Count();
                
                var result4 = await FocusDBService.GetMicroControllerSubDerivatives();
                MicrocontrollerSubDerivativeCount = result4.Value.Count();

                var result5 = await FocusDBService.GetTresosAutoCores(filter: $@"(contains(MultiCore,""{search}"") or contains(DevDrop,""{search}"") or contains(RFM,""{search}"")) ", expand: "MicroControllerSubDerivative,Architecture,AvailabilityStatus,CompilerVendor,CompilerVersion");

                architectureGroupForTresosAutoCore = result5.Value.GroupBy(issue => issue.Architecture.ArchitectureName)
                                   .Select(group => new ArchitectureGroupForTresosAutoCore { ArchitectureName = group.Key, CountOfSubDerevatives = group.Count() })
                                   .OrderByDescending(group => group.CountOfSubDerevatives);


                var result6 = await FocusDBService.GetTresosAutoCores(filter: $@"(contains(MultiCore,""{search}"") or contains(DevDrop,""{search}"") or contains(RFM,""{search}"")) ", expand: "MicroControllerSubDerivative,Architecture,AvailabilityStatus,CompilerVendor,CompilerVersion");


                architectureGroupForTresosAutoCoreVendor = result6.Value.GroupBy(issue => issue.CompilerVendor.CompilerVendorName)
                                   .Select(group => new ArchitectureGroupForTresosAutoCoreVendor { VendorName = group.Key, CountOfAvailability = group.Count() })
                                   .OrderByDescending(group => group.CountOfAvailability);


                var result7 = await FocusDBService.GetTresosAutoCores(filter: $@"(contains(MultiCore,""{search}"") or contains(DevDrop,""{search}"") or contains(RFM,""{search}"")) ", expand: "MicroControllerSubDerivative,Architecture,AvailabilityStatus,CompilerVendor,CompilerVersion");


                //architectureGroupForTresosAutoCoreAvailability = result7.Value.GroupBy(issue => issue.AvailabilityStatus.AvailabilityStatusName)
                //                   .Select(group => new ArchitectureGroupForTresosAutoCoreAvailability { TresosAutocore = group.Key, CountOfAvailability = group.Count() })
                //                   .OrderByDescending(group => group.CountOfAvailability);

                architectureGroupForTresosAutoCoreAvailability = result7.Value
                                .GroupBy(issue => new
                                {
                                    issue.AvailabilityStatus.AvailabilityStatusName,
                                    issue.CompilerVendor.CompilerVendorName // Add additional columns here
                                })
                                .Select(group => new ArchitectureGroupForTresosAutoCoreAvailability
                                {
                                    //TresosAutocore = $"{group.Key.AvailabilityStatusName} - {group.Key.CompilerVendorName}",
                                    TresosAutocoreColour = group.Key.AvailabilityStatusName,
                                    TresosAutocore = group.Key.CompilerVendorName,
                                    CountOfAvailability = group.Count()
                                })
                                .OrderByDescending(group => group.CountOfAvailability)
                                .ToList();


                architectureGroupForTresosAutoCoreAvailability = architectureGroupForTresosAutoCoreAvailability.GroupBy(isu => isu.TresosAutocore).Select(group => new ArchitectureGroupForTresosAutoCoreAvailability
                {
                    //TresosAutocore = $"{group.Key.AvailabilityStatusName} - {group.Key.CompilerVendorName}",
                    TresosAutocoreColour = group.Key,
                    //TresosAutocore = group.Key,
                    CountOfAvailability = group.Count()
                }).ToList(); 


            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load MicroControllers" });
            }
        }

    }
}