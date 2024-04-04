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
    //[Route("odata/FocusDB/CompilerVersions")]
    [Route("api/[controller]/[action]")]
    public partial class CompilerVersionsController : ODataController
    {
        private HardwareManagement.Server.Data.FocusDBContext context;

        public CompilerVersionsController(HardwareManagement.Server.Data.FocusDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<HardwareManagement.Server.Models.FocusDB.CompilerVersion> GetCompilerVersions()
        {
            var items = this.context.CompilerVersions.AsQueryable<HardwareManagement.Server.Models.FocusDB.CompilerVersion>();
            this.OnCompilerVersionsRead(ref items);

            return items;
        }

        partial void OnCompilerVersionsRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.CompilerVersion> items);

        partial void OnCompilerVersionGet(ref SingleResult<HardwareManagement.Server.Models.FocusDB.CompilerVersion> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/FocusDB/CompilerVersions(CompilerVersionID={CompilerVersionID})")]
        public SingleResult<HardwareManagement.Server.Models.FocusDB.CompilerVersion> GetCompilerVersion(int key)
        {
            var items = this.context.CompilerVersions.Where(i => i.CompilerVersionID == key);
            var result = SingleResult.Create(items);

            OnCompilerVersionGet(ref result);

            return result;
        }
        partial void OnCompilerVersionDeleted(HardwareManagement.Server.Models.FocusDB.CompilerVersion item);
        partial void OnAfterCompilerVersionDeleted(HardwareManagement.Server.Models.FocusDB.CompilerVersion item);

        [HttpDelete("/odata/FocusDB/CompilerVersions(CompilerVersionID={CompilerVersionID})")]
        public IActionResult DeleteCompilerVersion(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.CompilerVersions
                    .Where(i => i.CompilerVersionID == key)
                    .Include(i => i.TresosAcgs)
                    .Include(i => i.TresosAutoCores)
                    .Include(i => i.TresosSafetyOs)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.CompilerVersion>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnCompilerVersionDeleted(item);
                this.context.CompilerVersions.Remove(item);
                this.context.SaveChanges();
                this.OnAfterCompilerVersionDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCompilerVersionUpdated(HardwareManagement.Server.Models.FocusDB.CompilerVersion item);
        partial void OnAfterCompilerVersionUpdated(HardwareManagement.Server.Models.FocusDB.CompilerVersion item);

        [HttpPut("/odata/FocusDB/CompilerVersions(CompilerVersionID={CompilerVersionID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCompilerVersion(int key, [FromBody]HardwareManagement.Server.Models.FocusDB.CompilerVersion item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.CompilerVersions
                    .Where(i => i.CompilerVersionID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.CompilerVersion>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnCompilerVersionUpdated(item);
                this.context.CompilerVersions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CompilerVersions.Where(i => i.CompilerVersionID == key);
                
                this.OnAfterCompilerVersionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/FocusDB/CompilerVersions(CompilerVersionID={CompilerVersionID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCompilerVersion(int key, [FromBody]Delta<HardwareManagement.Server.Models.FocusDB.CompilerVersion> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.CompilerVersions
                    .Where(i => i.CompilerVersionID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.CompilerVersion>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnCompilerVersionUpdated(item);
                this.context.CompilerVersions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CompilerVersions.Where(i => i.CompilerVersionID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCompilerVersionCreated(HardwareManagement.Server.Models.FocusDB.CompilerVersion item);
        partial void OnAfterCompilerVersionCreated(HardwareManagement.Server.Models.FocusDB.CompilerVersion item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] HardwareManagement.Server.Models.FocusDB.CompilerVersion item)
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

                this.OnCompilerVersionCreated(item);
                this.context.CompilerVersions.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CompilerVersions.Where(i => i.CompilerVersionID == item.CompilerVersionID);

                

                this.OnAfterCompilerVersionCreated(item);

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
