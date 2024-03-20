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
    //[Route("odata/FocusDB/TresosSafetyOs")]
    [Route("api/[controller]/[action]")]
    public partial class TresosSafetyOsController : ODataController
    {
        private HardwareManagement.Server.Data.FocusDBContext context;

        public TresosSafetyOsController(HardwareManagement.Server.Data.FocusDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<HardwareManagement.Server.Models.FocusDB.TresosSafetyO> GetTresosSafetyOs()
        {
            var items = this.context.TresosSafetyOs.AsQueryable<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>();
            this.OnTresosSafetyOsRead(ref items);

            return items;
        }

        partial void OnTresosSafetyOsRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.TresosSafetyO> items);

        partial void OnTresosSafetyOGet(ref SingleResult<HardwareManagement.Server.Models.FocusDB.TresosSafetyO> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/FocusDB/TresosSafetyOs(TresosSafetyOSID={TresosSafetyOSID})")]
        public SingleResult<HardwareManagement.Server.Models.FocusDB.TresosSafetyO> GetTresosSafetyO(int key)
        {
            var items = this.context.TresosSafetyOs.Where(i => i.TresosSafetyOSID == key);
            var result = SingleResult.Create(items);

            OnTresosSafetyOGet(ref result);

            return result;
        }
        partial void OnTresosSafetyODeleted(HardwareManagement.Server.Models.FocusDB.TresosSafetyO item);
        partial void OnAfterTresosSafetyODeleted(HardwareManagement.Server.Models.FocusDB.TresosSafetyO item);

        [HttpDelete("/odata/FocusDB/TresosSafetyOs(TresosSafetyOSID={TresosSafetyOSID})")]
        public IActionResult DeleteTresosSafetyO(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.TresosSafetyOs
                    .Where(i => i.TresosSafetyOSID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnTresosSafetyODeleted(item);
                this.context.TresosSafetyOs.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTresosSafetyODeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTresosSafetyOUpdated(HardwareManagement.Server.Models.FocusDB.TresosSafetyO item);
        partial void OnAfterTresosSafetyOUpdated(HardwareManagement.Server.Models.FocusDB.TresosSafetyO item);

        [HttpPut("/odata/FocusDB/TresosSafetyOs(TresosSafetyOSID={TresosSafetyOSID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTresosSafetyO(int key, [FromBody]HardwareManagement.Server.Models.FocusDB.TresosSafetyO item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.TresosSafetyOs
                    .Where(i => i.TresosSafetyOSID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnTresosSafetyOUpdated(item);
                this.context.TresosSafetyOs.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TresosSafetyOs.Where(i => i.TresosSafetyOSID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Architecture,AvailabilityStatus,CompilerVendor,CompilerVersion,MicroControllerSubDerivative");
                this.OnAfterTresosSafetyOUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/FocusDB/TresosSafetyOs(TresosSafetyOSID={TresosSafetyOSID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTresosSafetyO(int key, [FromBody]Delta<HardwareManagement.Server.Models.FocusDB.TresosSafetyO> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.TresosSafetyOs
                    .Where(i => i.TresosSafetyOSID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnTresosSafetyOUpdated(item);
                this.context.TresosSafetyOs.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TresosSafetyOs.Where(i => i.TresosSafetyOSID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Architecture,AvailabilityStatus,CompilerVendor,CompilerVersion,MicroControllerSubDerivative");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTresosSafetyOCreated(HardwareManagement.Server.Models.FocusDB.TresosSafetyO item);
        partial void OnAfterTresosSafetyOCreated(HardwareManagement.Server.Models.FocusDB.TresosSafetyO item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] HardwareManagement.Server.Models.FocusDB.TresosSafetyO item)
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

                this.OnTresosSafetyOCreated(item);
                this.context.TresosSafetyOs.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TresosSafetyOs.Where(i => i.TresosSafetyOSID == item.TresosSafetyOSID);

                Request.QueryString = Request.QueryString.Add("$expand", "Architecture,AvailabilityStatus,CompilerVendor,CompilerVersion,MicroControllerSubDerivative");

                this.OnAfterTresosSafetyOCreated(item);

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
