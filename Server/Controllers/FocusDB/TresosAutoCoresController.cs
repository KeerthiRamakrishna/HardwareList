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
    [Route("odata/FocusDB/TresosAutoCores")]
    public partial class TresosAutoCoresController : ODataController
    {
        private HardwareManagement.Server.Data.FocusDBContext context;

        public TresosAutoCoresController(HardwareManagement.Server.Data.FocusDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> GetTresosAutoCores()
        {
            var items = this.context.TresosAutoCores.AsQueryable<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>();
            this.OnTresosAutoCoresRead(ref items);

            return items;
        }

        partial void OnTresosAutoCoresRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> items);

        partial void OnTresosAutoCoreGet(ref SingleResult<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/FocusDB/TresosAutoCores(TresosAutoCoreID={TresosAutoCoreID})")]
        public SingleResult<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> GetTresosAutoCore(int key)
        {
            var items = this.context.TresosAutoCores.Where(i => i.TresosAutoCoreID == key);
            var result = SingleResult.Create(items);

            OnTresosAutoCoreGet(ref result);

            return result;
        }
        partial void OnTresosAutoCoreDeleted(HardwareManagement.Server.Models.FocusDB.TresosAutoCore item);
        partial void OnAfterTresosAutoCoreDeleted(HardwareManagement.Server.Models.FocusDB.TresosAutoCore item);

        [HttpDelete("/odata/FocusDB/TresosAutoCores(TresosAutoCoreID={TresosAutoCoreID})")]
        public IActionResult DeleteTresosAutoCore(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.TresosAutoCores
                    .Where(i => i.TresosAutoCoreID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnTresosAutoCoreDeleted(item);
                this.context.TresosAutoCores.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTresosAutoCoreDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTresosAutoCoreUpdated(HardwareManagement.Server.Models.FocusDB.TresosAutoCore item);
        partial void OnAfterTresosAutoCoreUpdated(HardwareManagement.Server.Models.FocusDB.TresosAutoCore item);

        [HttpPut("/odata/FocusDB/TresosAutoCores(TresosAutoCoreID={TresosAutoCoreID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTresosAutoCore(int key, [FromBody]HardwareManagement.Server.Models.FocusDB.TresosAutoCore item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.TresosAutoCores
                    .Where(i => i.TresosAutoCoreID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnTresosAutoCoreUpdated(item);
                this.context.TresosAutoCores.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TresosAutoCores.Where(i => i.TresosAutoCoreID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Architecture,AvailabilityStatus,CompilerVendor,CompilerVersion,MicroControllerSubDerivative");
                this.OnAfterTresosAutoCoreUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/FocusDB/TresosAutoCores(TresosAutoCoreID={TresosAutoCoreID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTresosAutoCore(int key, [FromBody]Delta<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.TresosAutoCores
                    .Where(i => i.TresosAutoCoreID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnTresosAutoCoreUpdated(item);
                this.context.TresosAutoCores.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TresosAutoCores.Where(i => i.TresosAutoCoreID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Architecture,AvailabilityStatus,CompilerVendor,CompilerVersion,MicroControllerSubDerivative");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTresosAutoCoreCreated(HardwareManagement.Server.Models.FocusDB.TresosAutoCore item);
        partial void OnAfterTresosAutoCoreCreated(HardwareManagement.Server.Models.FocusDB.TresosAutoCore item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] HardwareManagement.Server.Models.FocusDB.TresosAutoCore item)
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

                this.OnTresosAutoCoreCreated(item);
                this.context.TresosAutoCores.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TresosAutoCores.Where(i => i.TresosAutoCoreID == item.TresosAutoCoreID);

                Request.QueryString = Request.QueryString.Add("$expand", "Architecture,AvailabilityStatus,CompilerVendor,CompilerVersion,MicroControllerSubDerivative");

                this.OnAfterTresosAutoCoreCreated(item);

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
