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
    [Route("odata/FocusDB/MicroControllerDerivatives")]
    public partial class MicroControllerDerivativesController : ODataController
    {
        private HardwareManagement.Server.Data.FocusDBContext context;

        public MicroControllerDerivativesController(HardwareManagement.Server.Data.FocusDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> GetMicroControllerDerivatives()
        {
            var items = this.context.MicroControllerDerivatives.AsQueryable<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative>();
            this.OnMicroControllerDerivativesRead(ref items);

            return items;
        }

        partial void OnMicroControllerDerivativesRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> items);

        partial void OnMicroControllerDerivativeGet(ref SingleResult<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/FocusDB/MicroControllerDerivatives(MicroControllerDerivativesID={MicroControllerDerivativesID})")]
        public SingleResult<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> GetMicroControllerDerivative(int key)
        {
            var items = this.context.MicroControllerDerivatives.Where(i => i.MicroControllerDerivativesID == key);
            var result = SingleResult.Create(items);

            OnMicroControllerDerivativeGet(ref result);

            return result;
        }
        partial void OnMicroControllerDerivativeDeleted(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item);
        partial void OnAfterMicroControllerDerivativeDeleted(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item);

        [HttpDelete("/odata/FocusDB/MicroControllerDerivatives(MicroControllerDerivativesID={MicroControllerDerivativesID})")]
        public IActionResult DeleteMicroControllerDerivative(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.MicroControllerDerivatives
                    .Where(i => i.MicroControllerDerivativesID == key)
                    .Include(i => i.MicroControllers)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMicroControllerDerivativeDeleted(item);
                this.context.MicroControllerDerivatives.Remove(item);
                this.context.SaveChanges();
                this.OnAfterMicroControllerDerivativeDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMicroControllerDerivativeUpdated(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item);
        partial void OnAfterMicroControllerDerivativeUpdated(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item);

        [HttpPut("/odata/FocusDB/MicroControllerDerivatives(MicroControllerDerivativesID={MicroControllerDerivativesID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutMicroControllerDerivative(int key, [FromBody]HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.MicroControllerDerivatives
                    .Where(i => i.MicroControllerDerivativesID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMicroControllerDerivativeUpdated(item);
                this.context.MicroControllerDerivatives.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MicroControllerDerivatives.Where(i => i.MicroControllerDerivativesID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "SiliconVendor");
                this.OnAfterMicroControllerDerivativeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/FocusDB/MicroControllerDerivatives(MicroControllerDerivativesID={MicroControllerDerivativesID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchMicroControllerDerivative(int key, [FromBody]Delta<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.MicroControllerDerivatives
                    .Where(i => i.MicroControllerDerivativesID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnMicroControllerDerivativeUpdated(item);
                this.context.MicroControllerDerivatives.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MicroControllerDerivatives.Where(i => i.MicroControllerDerivativesID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "SiliconVendor");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMicroControllerDerivativeCreated(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item);
        partial void OnAfterMicroControllerDerivativeCreated(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item)
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

                this.OnMicroControllerDerivativeCreated(item);
                this.context.MicroControllerDerivatives.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MicroControllerDerivatives.Where(i => i.MicroControllerDerivativesID == item.MicroControllerDerivativesID);

                Request.QueryString = Request.QueryString.Add("$expand", "SiliconVendor");

                this.OnAfterMicroControllerDerivativeCreated(item);

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
