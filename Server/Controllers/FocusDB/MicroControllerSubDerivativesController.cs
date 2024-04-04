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
    //[Route("odata/FocusDB/MicroControllerSubDerivatives")]
    [Route("api/[controller]/[action]")]
    public partial class MicroControllerSubDerivativesController : ODataController
    {
        private HardwareManagement.Server.Data.FocusDBContext context;

        public MicroControllerSubDerivativesController(HardwareManagement.Server.Data.FocusDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> GetMicroControllerSubDerivatives()
        {
            var items = this.context.MicroControllerSubDerivatives.AsQueryable<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative>();
            this.OnMicroControllerSubDerivativesRead(ref items);

            return items;
        }

        partial void OnMicroControllerSubDerivativesRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> items);

        partial void OnMicroControllerSubDerivativeGet(ref SingleResult<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/FocusDB/MicroControllerSubDerivatives(MicroControllerSubDerivativesID={MicroControllerSubDerivativesID})")]
        public SingleResult<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> GetMicroControllerSubDerivative(int key)
        {
            var items = this.context.MicroControllerSubDerivatives.Where(i => i.MicroControllerSubDerivativesID == key);
            var result = SingleResult.Create(items);

            OnMicroControllerSubDerivativeGet(ref result);

            return result;
        }
        partial void OnMicroControllerSubDerivativeDeleted(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item);
        partial void OnAfterMicroControllerSubDerivativeDeleted(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item);

        [HttpDelete("/odata/FocusDB/MicroControllerSubDerivatives(MicroControllerSubDerivativesID={MicroControllerSubDerivativesID})")]
        public IActionResult DeleteMicroControllerSubDerivative(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.MicroControllerSubDerivatives
                    .Where(i => i.MicroControllerSubDerivativesID == key)
                    .Include(i => i.MicroControllers)
                    .Include(i => i.TresosAcgs)
                    .Include(i => i.TresosAutoCores)
                    .Include(i => i.TresosSafetyOs)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMicroControllerSubDerivativeDeleted(item);
                this.context.MicroControllerSubDerivatives.Remove(item);
                this.context.SaveChanges();
                this.OnAfterMicroControllerSubDerivativeDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMicroControllerSubDerivativeUpdated(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item);
        partial void OnAfterMicroControllerSubDerivativeUpdated(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item);

        [HttpPut("/odata/FocusDB/MicroControllerSubDerivatives(MicroControllerSubDerivativesID={MicroControllerSubDerivativesID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutMicroControllerSubDerivative(int key, [FromBody]HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.MicroControllerSubDerivatives
                    .Where(i => i.MicroControllerSubDerivativesID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMicroControllerSubDerivativeUpdated(item);
                this.context.MicroControllerSubDerivatives.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MicroControllerSubDerivatives.Where(i => i.MicroControllerSubDerivativesID == key);
                
                this.OnAfterMicroControllerSubDerivativeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/FocusDB/MicroControllerSubDerivatives(MicroControllerSubDerivativesID={MicroControllerSubDerivativesID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchMicroControllerSubDerivative(int key, [FromBody]Delta<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.MicroControllerSubDerivatives
                    .Where(i => i.MicroControllerSubDerivativesID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnMicroControllerSubDerivativeUpdated(item);
                this.context.MicroControllerSubDerivatives.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MicroControllerSubDerivatives.Where(i => i.MicroControllerSubDerivativesID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMicroControllerSubDerivativeCreated(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item);
        partial void OnAfterMicroControllerSubDerivativeCreated(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item)
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

                this.OnMicroControllerSubDerivativeCreated(item);
                this.context.MicroControllerSubDerivatives.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.MicroControllerSubDerivatives.Where(i => i.MicroControllerSubDerivativesID == item.MicroControllerSubDerivativesID);

                

                this.OnAfterMicroControllerSubDerivativeCreated(item);

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
