using System.Net.Http;
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



        bool showDataLabels = false;

        class DataItem
        {
            public string Quarter { get; set; }
            public double Revenue { get; set; }
        }

        DataItem[] revenue = new DataItem[] {
        new DataItem
        {
        Quarter = "Q1",
        Revenue = 30000
        },
        new DataItem
        {
        Quarter = "Q2",
        Revenue = 40000
        },
        new DataItem
        {
        Quarter = "Q3",
        Revenue = 50000
        },
        new DataItem
        {
        Quarter = "Q4",
        Revenue = 80000
        },
        };

    }
}