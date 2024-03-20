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
    //[Route("odata/FocusDB/CompilerVendors")]
    [Route("api/[controller]/[action]")]
    public partial class CompilerVendorsController : ODataController
    {
        private HardwareManagement.Server.Data.FocusDBContext context;

        public CompilerVendorsController(HardwareManagement.Server.Data.FocusDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<HardwareManagement.Server.Models.FocusDB.CompilerVendor> GetCompilerVendors()
        {
            var items = this.context.CompilerVendors.AsQueryable<HardwareManagement.Server.Models.FocusDB.CompilerVendor>();
            this.OnCompilerVendorsRead(ref items);

            return items;
        }

        partial void OnCompilerVendorsRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.CompilerVendor> items);

        partial void OnCompilerVendorGet(ref SingleResult<HardwareManagement.Server.Models.FocusDB.CompilerVendor> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/FocusDB/CompilerVendors(CompilerVendorID={CompilerVendorID})")]
        public SingleResult<HardwareManagement.Server.Models.FocusDB.CompilerVendor> GetCompilerVendor(int key)
        {
            var items = this.context.CompilerVendors.Where(i => i.CompilerVendorID == key);
            var result = SingleResult.Create(items);

            OnCompilerVendorGet(ref result);

            return result;
        }
        partial void OnCompilerVendorDeleted(HardwareManagement.Server.Models.FocusDB.CompilerVendor item);
        partial void OnAfterCompilerVendorDeleted(HardwareManagement.Server.Models.FocusDB.CompilerVendor item);

        [HttpDelete("/odata/FocusDB/CompilerVendors(CompilerVendorID={CompilerVendorID})")]
        public IActionResult DeleteCompilerVendor(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.CompilerVendors
                    .Where(i => i.CompilerVendorID == key)
                    .Include(i => i.TresosAcgs)
                    .Include(i => i.TresosAutoCores)
                    .Include(i => i.TresosSafetyOs)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.CompilerVendor>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnCompilerVendorDeleted(item);
                this.context.CompilerVendors.Remove(item);
                this.context.SaveChanges();
                this.OnAfterCompilerVendorDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCompilerVendorUpdated(HardwareManagement.Server.Models.FocusDB.CompilerVendor item);
        partial void OnAfterCompilerVendorUpdated(HardwareManagement.Server.Models.FocusDB.CompilerVendor item);

        [HttpPut("/odata/FocusDB/CompilerVendors(CompilerVendorID={CompilerVendorID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCompilerVendor(int key, [FromBody]HardwareManagement.Server.Models.FocusDB.CompilerVendor item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.CompilerVendors
                    .Where(i => i.CompilerVendorID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.CompilerVendor>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnCompilerVendorUpdated(item);
                this.context.CompilerVendors.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CompilerVendors.Where(i => i.CompilerVendorID == key);
                
                this.OnAfterCompilerVendorUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/FocusDB/CompilerVendors(CompilerVendorID={CompilerVendorID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCompilerVendor(int key, [FromBody]Delta<HardwareManagement.Server.Models.FocusDB.CompilerVendor> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.CompilerVendors
                    .Where(i => i.CompilerVendorID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.CompilerVendor>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnCompilerVendorUpdated(item);
                this.context.CompilerVendors.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CompilerVendors.Where(i => i.CompilerVendorID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCompilerVendorCreated(HardwareManagement.Server.Models.FocusDB.CompilerVendor item);
        partial void OnAfterCompilerVendorCreated(HardwareManagement.Server.Models.FocusDB.CompilerVendor item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] HardwareManagement.Server.Models.FocusDB.CompilerVendor item)
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

                this.OnCompilerVendorCreated(item);
                this.context.CompilerVendors.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CompilerVendors.Where(i => i.CompilerVendorID == item.CompilerVendorID);

                

                this.OnAfterCompilerVendorCreated(item);

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
