
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace HardwareManagement.Client
{
    public partial class FocusDBService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public FocusDBService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/FocusDB/");
        }


        public async System.Threading.Tasks.Task ExportArchitecturesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/architectures/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/architectures/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportArchitecturesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/architectures/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/architectures/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetArchitectures(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.Architecture>> GetArchitectures(Query query)
        {
            return await GetArchitectures(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.Architecture>> GetArchitectures(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Architectures");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetArchitectures(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.Architecture>>(response);
        }

        partial void OnCreateArchitecture(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.Architecture> CreateArchitecture(HardwareManagement.Server.Models.FocusDB.Architecture architecture = default(HardwareManagement.Server.Models.FocusDB.Architecture))
        {
            var uri = new Uri(baseUri, $"Architectures");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(architecture), Encoding.UTF8, "application/json");

            OnCreateArchitecture(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.Architecture>(response);
        }

        partial void OnDeleteArchitecture(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteArchitecture(int architectureId = default(int))
        {
            var uri = new Uri(baseUri, $"Architectures({architectureId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteArchitecture(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetArchitectureByArchitectureId(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.Architecture> GetArchitectureByArchitectureId(string expand = default(string), int architectureId = default(int))
        {
            var uri = new Uri(baseUri, $"Architectures({architectureId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetArchitectureByArchitectureId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.Architecture>(response);
        }

        partial void OnUpdateArchitecture(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateArchitecture(int architectureId = default(int), HardwareManagement.Server.Models.FocusDB.Architecture architecture = default(HardwareManagement.Server.Models.FocusDB.Architecture))
        {
            var uri = new Uri(baseUri, $"Architectures({architectureId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", architecture.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(architecture), Encoding.UTF8, "application/json");

            OnUpdateArchitecture(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAvailabilityStatusesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/availabilitystatuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/availabilitystatuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAvailabilityStatusesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/availabilitystatuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/availabilitystatuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAvailabilityStatuses(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus>> GetAvailabilityStatuses(Query query)
        {
            return await GetAvailabilityStatuses(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus>> GetAvailabilityStatuses(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AvailabilityStatuses");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAvailabilityStatuses(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus>>(response);
        }

        partial void OnCreateAvailabilityStatus(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> CreateAvailabilityStatus(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus availabilityStatus = default(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus))
        {
            var uri = new Uri(baseUri, $"AvailabilityStatuses");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(availabilityStatus), Encoding.UTF8, "application/json");

            OnCreateAvailabilityStatus(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus>(response);
        }

        partial void OnDeleteAvailabilityStatus(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAvailabilityStatus(int availabilityStatusId = default(int))
        {
            var uri = new Uri(baseUri, $"AvailabilityStatuses({availabilityStatusId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAvailabilityStatus(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAvailabilityStatusByAvailabilityStatusId(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> GetAvailabilityStatusByAvailabilityStatusId(string expand = default(string), int availabilityStatusId = default(int))
        {
            var uri = new Uri(baseUri, $"AvailabilityStatuses({availabilityStatusId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAvailabilityStatusByAvailabilityStatusId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus>(response);
        }

        partial void OnUpdateAvailabilityStatus(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAvailabilityStatus(int availabilityStatusId = default(int), HardwareManagement.Server.Models.FocusDB.AvailabilityStatus availabilityStatus = default(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus))
        {
            var uri = new Uri(baseUri, $"AvailabilityStatuses({availabilityStatusId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", availabilityStatus.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(availabilityStatus), Encoding.UTF8, "application/json");

            OnUpdateAvailabilityStatus(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportMicroControllerDerivativesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/microcontrollerderivatives/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/microcontrollerderivatives/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportMicroControllerDerivativesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/microcontrollerderivatives/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/microcontrollerderivatives/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetMicroControllerDerivatives(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative>> GetMicroControllerDerivatives(Query query)
        {
            return await GetMicroControllerDerivatives(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative>> GetMicroControllerDerivatives(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"MicroControllerDerivatives");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMicroControllerDerivatives(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative>>(response);
        }

        partial void OnCreateMicroControllerDerivative(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> CreateMicroControllerDerivative(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative microControllerDerivative = default(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative))
        {
            var uri = new Uri(baseUri, $"MicroControllerDerivatives");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(microControllerDerivative), Encoding.UTF8, "application/json");

            OnCreateMicroControllerDerivative(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative>(response);
        }

        partial void OnDeleteMicroControllerDerivative(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteMicroControllerDerivative(int microControllerDerivativesId = default(int))
        {
            var uri = new Uri(baseUri, $"MicroControllerDerivatives({microControllerDerivativesId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteMicroControllerDerivative(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetMicroControllerDerivativeByMicroControllerDerivativesId(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> GetMicroControllerDerivativeByMicroControllerDerivativesId(string expand = default(string), int microControllerDerivativesId = default(int))
        {
            var uri = new Uri(baseUri, $"MicroControllerDerivatives({microControllerDerivativesId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMicroControllerDerivativeByMicroControllerDerivativesId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative>(response);
        }

        partial void OnUpdateMicroControllerDerivative(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateMicroControllerDerivative(int microControllerDerivativesId = default(int), HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative microControllerDerivative = default(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative))
        {
            var uri = new Uri(baseUri, $"MicroControllerDerivatives({microControllerDerivativesId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", microControllerDerivative.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(microControllerDerivative), Encoding.UTF8, "application/json");

            OnUpdateMicroControllerDerivative(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportMicroControllersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/microcontrollers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/microcontrollers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportMicroControllersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/microcontrollers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/microcontrollers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetMicroControllers(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.MicroController>> GetMicroControllers(Query query)
        {
            return await GetMicroControllers(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.MicroController>> GetMicroControllers(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"MicroControllers");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMicroControllers(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.MicroController>>(response);
        }

        partial void OnCreateMicroController(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroController> CreateMicroController(HardwareManagement.Server.Models.FocusDB.MicroController microController = default(HardwareManagement.Server.Models.FocusDB.MicroController))
        {
            var uri = new Uri(baseUri, $"MicroControllers");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(microController), Encoding.UTF8, "application/json");

            OnCreateMicroController(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.MicroController>(response);
        }

        partial void OnDeleteMicroController(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteMicroController(int hardwareId = default(int))
        {
            var uri = new Uri(baseUri, $"MicroControllers({hardwareId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteMicroController(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetMicroControllerByHardwareId(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroController> GetMicroControllerByHardwareId(string expand = default(string), int hardwareId = default(int))
        {
            var uri = new Uri(baseUri, $"MicroControllers({hardwareId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMicroControllerByHardwareId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.MicroController>(response);
        }

        partial void OnUpdateMicroController(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateMicroController(int hardwareId = default(int), HardwareManagement.Server.Models.FocusDB.MicroController microController = default(HardwareManagement.Server.Models.FocusDB.MicroController))
        {
            var uri = new Uri(baseUri, $"MicroControllers({hardwareId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", microController.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(microController), Encoding.UTF8, "application/json");

            OnUpdateMicroController(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportMicroControllerSubDerivativesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/microcontrollersubderivatives/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/microcontrollersubderivatives/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportMicroControllerSubDerivativesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/microcontrollersubderivatives/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/microcontrollersubderivatives/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetMicroControllerSubDerivatives(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative>> GetMicroControllerSubDerivatives(Query query)
        {
            return await GetMicroControllerSubDerivatives(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative>> GetMicroControllerSubDerivatives(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"MicroControllerSubDerivatives");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMicroControllerSubDerivatives(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative>>(response);
        }

        partial void OnCreateMicroControllerSubDerivative(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> CreateMicroControllerSubDerivative(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative microControllerSubDerivative = default(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative))
        {
            var uri = new Uri(baseUri, $"MicroControllerSubDerivatives");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(microControllerSubDerivative), Encoding.UTF8, "application/json");

            OnCreateMicroControllerSubDerivative(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative>(response);
        }

        partial void OnDeleteMicroControllerSubDerivative(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteMicroControllerSubDerivative(int microControllerSubDerivativesId = default(int))
        {
            var uri = new Uri(baseUri, $"MicroControllerSubDerivatives({microControllerSubDerivativesId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteMicroControllerSubDerivative(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetMicroControllerSubDerivativeByMicroControllerSubDerivativesId(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> GetMicroControllerSubDerivativeByMicroControllerSubDerivativesId(string expand = default(string), int microControllerSubDerivativesId = default(int))
        {
            var uri = new Uri(baseUri, $"MicroControllerSubDerivatives({microControllerSubDerivativesId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetMicroControllerSubDerivativeByMicroControllerSubDerivativesId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative>(response);
        }

        partial void OnUpdateMicroControllerSubDerivative(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateMicroControllerSubDerivative(int microControllerSubDerivativesId = default(int), HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative microControllerSubDerivative = default(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative))
        {
            var uri = new Uri(baseUri, $"MicroControllerSubDerivatives({microControllerSubDerivativesId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", microControllerSubDerivative.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(microControllerSubDerivative), Encoding.UTF8, "application/json");

            OnUpdateMicroControllerSubDerivative(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportSiliconVendorsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/siliconvendors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/siliconvendors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportSiliconVendorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/siliconvendors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/siliconvendors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSiliconVendors(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.SiliconVendor>> GetSiliconVendors(Query query)
        {
            return await GetSiliconVendors(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.SiliconVendor>> GetSiliconVendors(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"SiliconVendors");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSiliconVendors(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.SiliconVendor>>(response);
        }

        partial void OnCreateSiliconVendor(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.SiliconVendor> CreateSiliconVendor(HardwareManagement.Server.Models.FocusDB.SiliconVendor siliconVendor = default(HardwareManagement.Server.Models.FocusDB.SiliconVendor))
        {
            var uri = new Uri(baseUri, $"SiliconVendors");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(siliconVendor), Encoding.UTF8, "application/json");

            OnCreateSiliconVendor(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.SiliconVendor>(response);
        }

        partial void OnDeleteSiliconVendor(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSiliconVendor(int siliconVendorId = default(int))
        {
            var uri = new Uri(baseUri, $"SiliconVendors({siliconVendorId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSiliconVendor(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetSiliconVendorBySiliconVendorId(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.SiliconVendor> GetSiliconVendorBySiliconVendorId(string expand = default(string), int siliconVendorId = default(int))
        {
            var uri = new Uri(baseUri, $"SiliconVendors({siliconVendorId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSiliconVendorBySiliconVendorId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.SiliconVendor>(response);
        }

        partial void OnUpdateSiliconVendor(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateSiliconVendor(int siliconVendorId = default(int), HardwareManagement.Server.Models.FocusDB.SiliconVendor siliconVendor = default(HardwareManagement.Server.Models.FocusDB.SiliconVendor))
        {
            var uri = new Uri(baseUri, $"SiliconVendors({siliconVendorId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", siliconVendor.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(siliconVendor), Encoding.UTF8, "application/json");

            OnUpdateSiliconVendor(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportAutosarVersionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/autosarversions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/autosarversions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportAutosarVersionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/autosarversions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/autosarversions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetAutosarVersions(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.AutosarVersion>> GetAutosarVersions(Query query)
        {
            return await GetAutosarVersions(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.AutosarVersion>> GetAutosarVersions(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"AutosarVersions");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAutosarVersions(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.AutosarVersion>>(response);
        }

        partial void OnCreateAutosarVersion(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.AutosarVersion> CreateAutosarVersion(HardwareManagement.Server.Models.FocusDB.AutosarVersion autosarVersion = default(HardwareManagement.Server.Models.FocusDB.AutosarVersion))
        {
            var uri = new Uri(baseUri, $"AutosarVersions");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(autosarVersion), Encoding.UTF8, "application/json");

            OnCreateAutosarVersion(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.AutosarVersion>(response);
        }

        partial void OnDeleteAutosarVersion(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteAutosarVersion(int autosarversionId = default(int))
        {
            var uri = new Uri(baseUri, $"AutosarVersions({autosarversionId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteAutosarVersion(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetAutosarVersionByAutosarversionId(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.AutosarVersion> GetAutosarVersionByAutosarversionId(string expand = default(string), int autosarversionId = default(int))
        {
            var uri = new Uri(baseUri, $"AutosarVersions({autosarversionId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetAutosarVersionByAutosarversionId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.AutosarVersion>(response);
        }

        partial void OnUpdateAutosarVersion(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateAutosarVersion(int autosarversionId = default(int), HardwareManagement.Server.Models.FocusDB.AutosarVersion autosarVersion = default(HardwareManagement.Server.Models.FocusDB.AutosarVersion))
        {
            var uri = new Uri(baseUri, $"AutosarVersions({autosarversionId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", autosarVersion.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(autosarVersion), Encoding.UTF8, "application/json");

            OnUpdateAutosarVersion(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportCompilerVendorsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/compilervendors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/compilervendors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportCompilerVendorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/compilervendors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/compilervendors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetCompilerVendors(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.CompilerVendor>> GetCompilerVendors(Query query)
        {
            return await GetCompilerVendors(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.CompilerVendor>> GetCompilerVendors(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"CompilerVendors");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCompilerVendors(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.CompilerVendor>>(response);
        }

        partial void OnCreateCompilerVendor(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.CompilerVendor> CreateCompilerVendor(HardwareManagement.Server.Models.FocusDB.CompilerVendor compilerVendor = default(HardwareManagement.Server.Models.FocusDB.CompilerVendor))
        {
            var uri = new Uri(baseUri, $"CompilerVendors");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(compilerVendor), Encoding.UTF8, "application/json");

            OnCreateCompilerVendor(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.CompilerVendor>(response);
        }

        partial void OnDeleteCompilerVendor(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteCompilerVendor(int compilerVendorId = default(int))
        {
            var uri = new Uri(baseUri, $"CompilerVendors({compilerVendorId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteCompilerVendor(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetCompilerVendorByCompilerVendorId(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.CompilerVendor> GetCompilerVendorByCompilerVendorId(string expand = default(string), int compilerVendorId = default(int))
        {
            var uri = new Uri(baseUri, $"CompilerVendors({compilerVendorId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCompilerVendorByCompilerVendorId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.CompilerVendor>(response);
        }

        partial void OnUpdateCompilerVendor(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateCompilerVendor(int compilerVendorId = default(int), HardwareManagement.Server.Models.FocusDB.CompilerVendor compilerVendor = default(HardwareManagement.Server.Models.FocusDB.CompilerVendor))
        {
            var uri = new Uri(baseUri, $"CompilerVendors({compilerVendorId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", compilerVendor.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(compilerVendor), Encoding.UTF8, "application/json");

            OnUpdateCompilerVendor(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportCompilerVersionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/compilerversions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/compilerversions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportCompilerVersionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/compilerversions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/compilerversions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetCompilerVersions(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.CompilerVersion>> GetCompilerVersions(Query query)
        {
            return await GetCompilerVersions(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.CompilerVersion>> GetCompilerVersions(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"CompilerVersions");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCompilerVersions(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.CompilerVersion>>(response);
        }

        partial void OnCreateCompilerVersion(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.CompilerVersion> CreateCompilerVersion(HardwareManagement.Server.Models.FocusDB.CompilerVersion compilerVersion = default(HardwareManagement.Server.Models.FocusDB.CompilerVersion))
        {
            var uri = new Uri(baseUri, $"CompilerVersions");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(compilerVersion), Encoding.UTF8, "application/json");

            OnCreateCompilerVersion(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.CompilerVersion>(response);
        }

        partial void OnDeleteCompilerVersion(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteCompilerVersion(int compilerVersionId = default(int))
        {
            var uri = new Uri(baseUri, $"CompilerVersions({compilerVersionId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteCompilerVersion(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetCompilerVersionByCompilerVersionId(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.CompilerVersion> GetCompilerVersionByCompilerVersionId(string expand = default(string), int compilerVersionId = default(int))
        {
            var uri = new Uri(baseUri, $"CompilerVersions({compilerVersionId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetCompilerVersionByCompilerVersionId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.CompilerVersion>(response);
        }

        partial void OnUpdateCompilerVersion(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateCompilerVersion(int compilerVersionId = default(int), HardwareManagement.Server.Models.FocusDB.CompilerVersion compilerVersion = default(HardwareManagement.Server.Models.FocusDB.CompilerVersion))
        {
            var uri = new Uri(baseUri, $"CompilerVersions({compilerVersionId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", compilerVersion.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(compilerVersion), Encoding.UTF8, "application/json");

            OnUpdateCompilerVersion(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportTresosAcgsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/tresosacgs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/tresosacgs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportTresosAcgsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/tresosacgs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/tresosacgs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetTresosAcgs(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.TresosAcg>> GetTresosAcgs(Query query)
        {
            return await GetTresosAcgs(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.TresosAcg>> GetTresosAcgs(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"TresosAcgs");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTresosAcgs(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.TresosAcg>>(response);
        }

        partial void OnCreateTresosAcg(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosAcg> CreateTresosAcg(HardwareManagement.Server.Models.FocusDB.TresosAcg tresosAcg = default(HardwareManagement.Server.Models.FocusDB.TresosAcg))
        {
            var uri = new Uri(baseUri, $"TresosAcgs");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(tresosAcg), Encoding.UTF8, "application/json");

            OnCreateTresosAcg(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.TresosAcg>(response);
        }

        partial void OnDeleteTresosAcg(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteTresosAcg(int tresosAcgid = default(int))
        {
            var uri = new Uri(baseUri, $"TresosAcgs({tresosAcgid})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteTresosAcg(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetTresosAcgByTresosAcgid(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosAcg> GetTresosAcgByTresosAcgid(string expand = default(string), int tresosAcgid = default(int))
        {
            var uri = new Uri(baseUri, $"TresosAcgs({tresosAcgid})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTresosAcgByTresosAcgid(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.TresosAcg>(response);
        }

        partial void OnUpdateTresosAcg(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateTresosAcg(int tresosAcgid = default(int), HardwareManagement.Server.Models.FocusDB.TresosAcg tresosAcg = default(HardwareManagement.Server.Models.FocusDB.TresosAcg))
        {
            var uri = new Uri(baseUri, $"TresosAcgs({tresosAcgid})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", tresosAcg.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(tresosAcg), Encoding.UTF8, "application/json");

            OnUpdateTresosAcg(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportTresosAutoCoresToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/tresosautocores/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/tresosautocores/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportTresosAutoCoresToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/tresosautocores/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/tresosautocores/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetTresosAutoCores(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>> GetTresosAutoCores(Query query)
        {
            return await GetTresosAutoCores(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>> GetTresosAutoCores(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"TresosAutoCores");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTresosAutoCores(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>>(response);
        }

        partial void OnCreateTresosAutoCore(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> CreateTresosAutoCore(HardwareManagement.Server.Models.FocusDB.TresosAutoCore tresosAutoCore = default(HardwareManagement.Server.Models.FocusDB.TresosAutoCore))
        {
            var uri = new Uri(baseUri, $"TresosAutoCores");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(tresosAutoCore), Encoding.UTF8, "application/json");

            OnCreateTresosAutoCore(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>(response);
        }

        partial void OnDeleteTresosAutoCore(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteTresosAutoCore(int tresosAutoCoreId = default(int))
        {
            var uri = new Uri(baseUri, $"TresosAutoCores({tresosAutoCoreId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteTresosAutoCore(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetTresosAutoCoreByTresosAutoCoreId(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> GetTresosAutoCoreByTresosAutoCoreId(string expand = default(string), int tresosAutoCoreId = default(int))
        {
            var uri = new Uri(baseUri, $"TresosAutoCores({tresosAutoCoreId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTresosAutoCoreByTresosAutoCoreId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>(response);
        }

        partial void OnUpdateTresosAutoCore(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateTresosAutoCore(int tresosAutoCoreId = default(int), HardwareManagement.Server.Models.FocusDB.TresosAutoCore tresosAutoCore = default(HardwareManagement.Server.Models.FocusDB.TresosAutoCore))
        {
            var uri = new Uri(baseUri, $"TresosAutoCores({tresosAutoCoreId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", tresosAutoCore.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(tresosAutoCore), Encoding.UTF8, "application/json");

            OnUpdateTresosAutoCore(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportTresosSafetyOsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/tresossafetyos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/tresossafetyos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportTresosSafetyOsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/tresossafetyos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/tresossafetyos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetTresosSafetyOs(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>> GetTresosSafetyOs(Query query)
        {
            return await GetTresosSafetyOs(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>> GetTresosSafetyOs(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"TresosSafetyOs");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTresosSafetyOs(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>>(response);
        }

        partial void OnCreateTresosSafetyO(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosSafetyO> CreateTresosSafetyO(HardwareManagement.Server.Models.FocusDB.TresosSafetyO tresosSafetyO = default(HardwareManagement.Server.Models.FocusDB.TresosSafetyO))
        {
            var uri = new Uri(baseUri, $"TresosSafetyOs");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(tresosSafetyO), Encoding.UTF8, "application/json");

            OnCreateTresosSafetyO(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>(response);
        }

        partial void OnDeleteTresosSafetyO(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteTresosSafetyO(int tresosSafetyOsid = default(int))
        {
            var uri = new Uri(baseUri, $"TresosSafetyOs({tresosSafetyOsid})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteTresosSafetyO(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetTresosSafetyOByTresosSafetyOsid(HttpRequestMessage requestMessage);

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosSafetyO> GetTresosSafetyOByTresosSafetyOsid(string expand = default(string), int tresosSafetyOsid = default(int))
        {
            var uri = new Uri(baseUri, $"TresosSafetyOs({tresosSafetyOsid})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTresosSafetyOByTresosSafetyOsid(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>(response);
        }

        partial void OnUpdateTresosSafetyO(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateTresosSafetyO(int tresosSafetyOsid = default(int), HardwareManagement.Server.Models.FocusDB.TresosSafetyO tresosSafetyO = default(HardwareManagement.Server.Models.FocusDB.TresosSafetyO))
        {
            var uri = new Uri(baseUri, $"TresosSafetyOs({tresosSafetyOsid})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", tresosSafetyO.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(tresosSafetyO), Encoding.UTF8, "application/json");

            OnUpdateTresosSafetyO(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}