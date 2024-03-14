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
    [Route("odata/FocusDB/SiliconVendors")]
    public partial class SiliconVendorsController : ODataController
    {
        private HardwareManagement.Server.Data.FocusDBContext context;

        public SiliconVendorsController(HardwareManagement.Server.Data.FocusDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<HardwareManagement.Server.Models.FocusDB.SiliconVendor> GetSiliconVendors()
        {
            var items = this.context.SiliconVendors.AsQueryable<HardwareManagement.Server.Models.FocusDB.SiliconVendor>();
            this.OnSiliconVendorsRead(ref items);

            return items;
        }

        partial void OnSiliconVendorsRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.SiliconVendor> items);

        partial void OnSiliconVendorGet(ref SingleResult<HardwareManagement.Server.Models.FocusDB.SiliconVendor> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/FocusDB/SiliconVendors(SiliconVendorID={SiliconVendorID})")]
        public SingleResult<HardwareManagement.Server.Models.FocusDB.SiliconVendor> GetSiliconVendor(int key)
        {
            var items = this.context.SiliconVendors.Where(i => i.SiliconVendorID == key);
            var result = SingleResult.Create(items);

            OnSiliconVendorGet(ref result);

            return result;
        }
        partial void OnSiliconVendorDeleted(HardwareManagement.Server.Models.FocusDB.SiliconVendor item);
        partial void OnAfterSiliconVendorDeleted(HardwareManagement.Server.Models.FocusDB.SiliconVendor item);

        [HttpDelete("/odata/FocusDB/SiliconVendors(SiliconVendorID={SiliconVendorID})")]
        public IActionResult DeleteSiliconVendor(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.SiliconVendors
                    .Where(i => i.SiliconVendorID == key)
                    .Include(i => i.MicroControllerDerivatives)
                    .Include(i => i.MicroControllers)
                    .Include(i => i.TresosAcgs)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.SiliconVendor>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSiliconVendorDeleted(item);
                this.context.SiliconVendors.Remove(item);
                this.context.SaveChanges();
                this.OnAfterSiliconVendorDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSiliconVendorUpdated(HardwareManagement.Server.Models.FocusDB.SiliconVendor item);
        partial void OnAfterSiliconVendorUpdated(HardwareManagement.Server.Models.FocusDB.SiliconVendor item);

        [HttpPut("/odata/FocusDB/SiliconVendors(SiliconVendorID={SiliconVendorID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSiliconVendor(int key, [FromBody]HardwareManagement.Server.Models.FocusDB.SiliconVendor item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SiliconVendors
                    .Where(i => i.SiliconVendorID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.SiliconVendor>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSiliconVendorUpdated(item);
                this.context.SiliconVendors.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SiliconVendors.Where(i => i.SiliconVendorID == key);
                
                this.OnAfterSiliconVendorUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/FocusDB/SiliconVendors(SiliconVendorID={SiliconVendorID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSiliconVendor(int key, [FromBody]Delta<HardwareManagement.Server.Models.FocusDB.SiliconVendor> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SiliconVendors
                    .Where(i => i.SiliconVendorID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.SiliconVendor>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnSiliconVendorUpdated(item);
                this.context.SiliconVendors.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SiliconVendors.Where(i => i.SiliconVendorID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSiliconVendorCreated(HardwareManagement.Server.Models.FocusDB.SiliconVendor item);
        partial void OnAfterSiliconVendorCreated(HardwareManagement.Server.Models.FocusDB.SiliconVendor item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] HardwareManagement.Server.Models.FocusDB.SiliconVendor item)
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

                this.OnSiliconVendorCreated(item);
                this.context.SiliconVendors.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SiliconVendors.Where(i => i.SiliconVendorID == item.SiliconVendorID);

                

                this.OnAfterSiliconVendorCreated(item);

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
