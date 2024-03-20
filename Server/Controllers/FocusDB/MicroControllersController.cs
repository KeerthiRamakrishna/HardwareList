using System;
using System.Net;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HardwareManagement.Server.Controllers.FocusDB
{
    //[Route("odata/FocusDB/MicroControllers")]
    [Route("api/[controller]/[action]")]
    public partial class MicroControllersController : ODataController
    {
        private HardwareManagement.Server.Data.FocusDBContext context;

        public MicroControllersController(HardwareManagement.Server.Data.FocusDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<HardwareManagement.Server.Models.FocusDB.MicroController> GetMicroControllers()
        {
            var items = this.context.MicroControllers.AsQueryable<HardwareManagement.Server.Models.FocusDB.MicroController>();
            this.OnMicroControllersRead(ref items);

            return items;
        }

        partial void OnMicroControllersRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.MicroController> items);

        partial void OnMicroControllerGet(ref SingleResult<HardwareManagement.Server.Models.FocusDB.MicroController> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/FocusDB/MicroControllers(HardwareId={HardwareId})")]
        public SingleResult<HardwareManagement.Server.Models.FocusDB.MicroController> GetMicroController(int key)
        {
            var items = this.context.MicroControllers.Where(i => i.HardwareId == key);
            var result = SingleResult.Create(items);

            OnMicroControllerGet(ref result);

            return result;
        }
        partial void OnMicroControllerDeleted(HardwareManagement.Server.Models.FocusDB.MicroController item);
        partial void OnAfterMicroControllerDeleted(HardwareManagement.Server.Models.FocusDB.MicroController item);

        [HttpDelete("/odata/FocusDB/MicroControllers(HardwareId={HardwareId})")]
        public IActionResult DeleteMicroController(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.MicroControllers
                    .Where(i => i.HardwareId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.MicroController>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMicroControllerDeleted(item);
                this.context.MicroControllers.Remove(item);
                this.context.SaveChanges();
                this.OnAfterMicroControllerDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMicroControllerUpdated(HardwareManagement.Server.Models.FocusDB.MicroController item);
        partial void OnAfterMicroControllerUpdated(HardwareManagement.Server.Models.FocusDB.MicroController item);

        [HttpPut("/odata/FocusDB/MicroControllers(HardwareId={HardwareId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutMicroController(int key, [FromBody]HardwareManagement.Server.Models.FocusDB.MicroController item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.MicroControllers
                    .Where(i => i.HardwareId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.MicroController>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMicroControllerUpdated(item);
                this.context.MicroControllers.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MicroControllers.Where(i => i.HardwareId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AvailabilityStatus,MicroControllerDerivative,MicroControllerSubDerivative,SiliconVendor");
                this.OnAfterMicroControllerUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/FocusDB/MicroControllers(HardwareId={HardwareId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchMicroController(int key, [FromBody]Delta<HardwareManagement.Server.Models.FocusDB.MicroController> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.MicroControllers
                    .Where(i => i.HardwareId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.MicroController>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnMicroControllerUpdated(item);
                this.context.MicroControllers.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MicroControllers.Where(i => i.HardwareId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AvailabilityStatus,MicroControllerDerivative,MicroControllerSubDerivative,SiliconVendor");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMicroControllerCreated(HardwareManagement.Server.Models.FocusDB.MicroController item);
        partial void OnAfterMicroControllerCreated(HardwareManagement.Server.Models.FocusDB.MicroController item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] HardwareManagement.Server.Models.FocusDB.MicroController item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnMicroControllerCreated(item);
                this.context.MicroControllers.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MicroControllers.Where(i => i.HardwareId == item.HardwareId);

                Request.QueryString = Request.QueryString.Add("$expand", "AvailabilityStatus,MicroControllerDerivative,MicroControllerSubDerivative,SiliconVendor");

                this.OnAfterMicroControllerCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
