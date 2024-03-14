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
    public partial class EditAutosarVersion
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
        public int AUTOSARVersionID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            autosarVersion = await FocusDBService.GetAutosarVersionByAutosarversionId(autosarversionId:AUTOSARVersionID);
        }
        protected bool errorVisible;
        protected HardwareManagement.Server.Models.FocusDB.AutosarVersion autosarVersion;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await FocusDBService.UpdateAutosarVersion(autosarversionId:AUTOSARVersionID, autosarVersion);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(autosarVersion);
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

            autosarVersion = await FocusDBService.GetAutosarVersionByAutosarversionId(autosarversionId:AUTOSARVersionID);
        }
    }
}