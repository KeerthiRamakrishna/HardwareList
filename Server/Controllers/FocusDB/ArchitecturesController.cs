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
    //[Route("odata/FocusDB/Architectures")]
    [Route("api/[controller]/[action]")]
    public partial class ArchitecturesController : ODataController
    {
        private HardwareManagement.Server.Data.FocusDBContext context;

        public ArchitecturesController(HardwareManagement.Server.Data.FocusDBContext context)
        {
            this.context = context;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 10, MaxAnyAllExpressionDepth = 10, MaxNodeCount = 1000)]
        public IEnumerable<HardwareManagement.Server.Models.FocusDB.Architecture> GetArchitectures()
        {
            var items = this.context.Architectures.AsQueryable<HardwareManagement.Server.Models.FocusDB.Architecture>();
            this.OnArchitecturesRead(ref items);

            return items;
        }

        partial void OnArchitecturesRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.Architecture> items);

        partial void OnArchitectureGet(ref SingleResult<HardwareManagement.Server.Models.FocusDB.Architecture> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/FocusDB/Architectures(ArchitectureID={ArchitectureID})")]
        public SingleResult<HardwareManagement.Server.Models.FocusDB.Architecture> GetArchitecture(int key)
        {
            var items = this.context.Architectures.Where(i => i.ArchitectureID == key);
            var result = SingleResult.Create(items);

            OnArchitectureGet(ref result);

            return result;
        }
        partial void OnArchitectureDeleted(HardwareManagement.Server.Models.FocusDB.Architecture item);
        partial void OnAfterArchitectureDeleted(HardwareManagement.Server.Models.FocusDB.Architecture item);

        [HttpDelete("/odata/FocusDB/Architectures(ArchitectureID={ArchitectureID})")]
        public IActionResult DeleteArchitecture(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Architectures
                    .Where(i => i.ArchitectureID == key)
                    .Include(i => i.TresosAcgs)
                    .Include(i => i.TresosAutoCores)
                    .Include(i => i.TresosSafetyOs)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.Architecture>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnArchitectureDeleted(item);
                this.context.Architectures.Remove(item);
                this.context.SaveChanges();
                this.OnAfterArchitectureDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnArchitectureUpdated(HardwareManagement.Server.Models.FocusDB.Architecture item);
        partial void OnAfterArchitectureUpdated(HardwareManagement.Server.Models.FocusDB.Architecture item);

        [HttpPut("/odata/FocusDB/Architectures(ArchitectureID={ArchitectureID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutArchitecture(int key, [FromBody]HardwareManagement.Server.Models.FocusDB.Architecture item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Architectures
                    .Where(i => i.ArchitectureID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.Architecture>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnArchitectureUpdated(item);
                this.context.Architectures.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Architectures.Where(i => i.ArchitectureID == key);
                
                this.OnAfterArchitectureUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/FocusDB/Architectures(ArchitectureID={ArchitectureID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchArchitecture(int key, [FromBody]Delta<HardwareManagement.Server.Models.FocusDB.Architecture> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Architectures
                    .Where(i => i.ArchitectureID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.Architecture>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnArchitectureUpdated(item);
                this.context.Architectures.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Architectures.Where(i => i.ArchitectureID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnArchitectureCreated(HardwareManagement.Server.Models.FocusDB.Architecture item);
        partial void OnAfterArchitectureCreated(HardwareManagement.Server.Models.FocusDB.Architecture item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth = 10, MaxAnyAllExpressionDepth = 10, MaxNodeCount = 1000)]
        public IActionResult Post([FromBody] HardwareManagement.Server.Models.FocusDB.Architecture item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnArchitectureCreated(item);
                this.context.Architectures.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Architectures.Where(i => i.ArchitectureID == item.ArchitectureID);



                this.OnAfterArchitectureCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
