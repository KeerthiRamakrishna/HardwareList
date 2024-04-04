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
    //[Route("odata/FocusDB/AvailabilityStatuses")]
    [Route("api/[controller]/[action]")]
    public partial class AvailabilityStatusesController : ODataController
    {
        private HardwareManagement.Server.Data.FocusDBContext context;

        public AvailabilityStatusesController(HardwareManagement.Server.Data.FocusDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> GetAvailabilityStatuses()
        {
            var items = this.context.AvailabilityStatuses.AsQueryable<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus>();
            this.OnAvailabilityStatusesRead(ref items);

            return items;
        }

        partial void OnAvailabilityStatusesRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> items);

        partial void OnAvailabilityStatusGet(ref SingleResult<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/FocusDB/AvailabilityStatuses(AvailabilityStatusID={AvailabilityStatusID})")]
        public SingleResult<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> GetAvailabilityStatus(int key)
        {
            var items = this.context.AvailabilityStatuses.Where(i => i.AvailabilityStatusID == key);
            var result = SingleResult.Create(items);

            OnAvailabilityStatusGet(ref result);

            return result;
        }
        partial void OnAvailabilityStatusDeleted(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item);
        partial void OnAfterAvailabilityStatusDeleted(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item);

        [HttpDelete("/odata/FocusDB/AvailabilityStatuses(AvailabilityStatusID={AvailabilityStatusID})")]
        public IActionResult DeleteAvailabilityStatus(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AvailabilityStatuses
                    .Where(i => i.AvailabilityStatusID == key)
                    .Include(i => i.MicroControllers)
                    .Include(i => i.TresosAcgs)
                    .Include(i => i.TresosAutoCores)
                    .Include(i => i.TresosSafetyOs)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAvailabilityStatusDeleted(item);
                this.context.AvailabilityStatuses.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAvailabilityStatusDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAvailabilityStatusUpdated(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item);
        partial void OnAfterAvailabilityStatusUpdated(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item);

        [HttpPut("/odata/FocusDB/AvailabilityStatuses(AvailabilityStatusID={AvailabilityStatusID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAvailabilityStatus(int key, [FromBody]HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AvailabilityStatuses
                    .Where(i => i.AvailabilityStatusID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAvailabilityStatusUpdated(item);
                this.context.AvailabilityStatuses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AvailabilityStatuses.Where(i => i.AvailabilityStatusID == key);
                
                this.OnAfterAvailabilityStatusUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/FocusDB/AvailabilityStatuses(AvailabilityStatusID={AvailabilityStatusID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAvailabilityStatus(int key, [FromBody]Delta<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AvailabilityStatuses
                    .Where(i => i.AvailabilityStatusID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAvailabilityStatusUpdated(item);
                this.context.AvailabilityStatuses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AvailabilityStatuses.Where(i => i.AvailabilityStatusID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAvailabilityStatusCreated(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item);
        partial void OnAfterAvailabilityStatusCreated(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item)
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

                this.OnAvailabilityStatusCreated(item);
                this.context.AvailabilityStatuses.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AvailabilityStatuses.Where(i => i.AvailabilityStatusID == item.AvailabilityStatusID);

                

                this.OnAfterAvailabilityStatusCreated(item);

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
