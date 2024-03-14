using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using HardwareManagement.Server.Data;

namespace HardwareManagement.Server.Controllers
{
    public partial class ExportFocusDBController : ExportController
    {
        private readonly FocusDBContext context;
        private readonly FocusDBService service;

        public ExportFocusDBController(FocusDBContext context, FocusDBService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/FocusDB/architectures/csv")]
        [HttpGet("/export/FocusDB/architectures/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportArchitecturesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetArchitectures(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/architectures/excel")]
        [HttpGet("/export/FocusDB/architectures/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportArchitecturesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetArchitectures(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/availabilitystatuses/csv")]
        [HttpGet("/export/FocusDB/availabilitystatuses/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAvailabilityStatusesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAvailabilityStatuses(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/availabilitystatuses/excel")]
        [HttpGet("/export/FocusDB/availabilitystatuses/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAvailabilityStatusesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAvailabilityStatuses(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/microcontrollerderivatives/csv")]
        [HttpGet("/export/FocusDB/microcontrollerderivatives/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMicroControllerDerivativesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMicroControllerDerivatives(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/microcontrollerderivatives/excel")]
        [HttpGet("/export/FocusDB/microcontrollerderivatives/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMicroControllerDerivativesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMicroControllerDerivatives(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/microcontrollers/csv")]
        [HttpGet("/export/FocusDB/microcontrollers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMicroControllersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMicroControllers(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/microcontrollers/excel")]
        [HttpGet("/export/FocusDB/microcontrollers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMicroControllersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMicroControllers(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/microcontrollersubderivatives/csv")]
        [HttpGet("/export/FocusDB/microcontrollersubderivatives/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMicroControllerSubDerivativesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMicroControllerSubDerivatives(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/microcontrollersubderivatives/excel")]
        [HttpGet("/export/FocusDB/microcontrollersubderivatives/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMicroControllerSubDerivativesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMicroControllerSubDerivatives(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/siliconvendors/csv")]
        [HttpGet("/export/FocusDB/siliconvendors/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSiliconVendorsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSiliconVendors(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/siliconvendors/excel")]
        [HttpGet("/export/FocusDB/siliconvendors/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSiliconVendorsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSiliconVendors(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/autosarversions/csv")]
        [HttpGet("/export/FocusDB/autosarversions/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAutosarVersionsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAutosarVersions(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/autosarversions/excel")]
        [HttpGet("/export/FocusDB/autosarversions/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAutosarVersionsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAutosarVersions(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/compilervendors/csv")]
        [HttpGet("/export/FocusDB/compilervendors/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCompilerVendorsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCompilerVendors(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/compilervendors/excel")]
        [HttpGet("/export/FocusDB/compilervendors/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCompilerVendorsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCompilerVendors(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/compilerversions/csv")]
        [HttpGet("/export/FocusDB/compilerversions/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCompilerVersionsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCompilerVersions(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/compilerversions/excel")]
        [HttpGet("/export/FocusDB/compilerversions/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCompilerVersionsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCompilerVersions(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/tresosacgs/csv")]
        [HttpGet("/export/FocusDB/tresosacgs/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTresosAcgsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTresosAcgs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/tresosacgs/excel")]
        [HttpGet("/export/FocusDB/tresosacgs/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTresosAcgsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTresosAcgs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/tresosautocores/csv")]
        [HttpGet("/export/FocusDB/tresosautocores/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTresosAutoCoresToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTresosAutoCores(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/tresosautocores/excel")]
        [HttpGet("/export/FocusDB/tresosautocores/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTresosAutoCoresToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTresosAutoCores(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/tresossafetyos/csv")]
        [HttpGet("/export/FocusDB/tresossafetyos/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTresosSafetyOsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTresosSafetyOs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/FocusDB/tresossafetyos/excel")]
        [HttpGet("/export/FocusDB/tresossafetyos/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTresosSafetyOsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTresosSafetyOs(), Request.Query, false), fileName);
        }
    }
}
