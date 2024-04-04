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
    //[Route("odata/FocusDB/AutosarVersions")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public partial class AutosarVersionsController : ODataController
    {
        private HardwareManagement.Server.Data.FocusDBContext context;

        public AutosarVersionsController(HardwareManagement.Server.Data.FocusDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<HardwareManagement.Server.Models.FocusDB.AutosarVersion> GetAutosarVersions()
        {
            var items = this.context.AutosarVersions.AsQueryable<HardwareManagement.Server.Models.FocusDB.AutosarVersion>();
            this.OnAutosarVersionsRead(ref items);

            return items;
        }

        partial void OnAutosarVersionsRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.AutosarVersion> items);

        partial void OnAutosarVersionGet(ref SingleResult<HardwareManagement.Server.Models.FocusDB.AutosarVersion> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/FocusDB/AutosarVersions(AUTOSARVersionID={AUTOSARVersionID})")]
        public SingleResult<HardwareManagement.Server.Models.FocusDB.AutosarVersion> GetAutosarVersion(int key)
        {
            var items = this.context.AutosarVersions.Where(i => i.AUTOSARVersionID == key);
            var result = SingleResult.Create(items);

            OnAutosarVersionGet(ref result);

            return result;
        }
        partial void OnAutosarVersionDeleted(HardwareManagement.Server.Models.FocusDB.AutosarVersion item);
        partial void OnAfterAutosarVersionDeleted(HardwareManagement.Server.Models.FocusDB.AutosarVersion item);

        [HttpDelete("/odata/FocusDB/AutosarVersions(AUTOSARVersionID={AUTOSARVersionID})")]
        public IActionResult DeleteAutosarVersion(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AutosarVersions
                    .Where(i => i.AUTOSARVersionID == key)
                    .Include(i => i.TresosAcgs)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.AutosarVersion>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAutosarVersionDeleted(item);
                this.context.AutosarVersions.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAutosarVersionDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAutosarVersionUpdated(HardwareManagement.Server.Models.FocusDB.AutosarVersion item);
        partial void OnAfterAutosarVersionUpdated(HardwareManagement.Server.Models.FocusDB.AutosarVersion item);

        [HttpPut("/odata/FocusDB/AutosarVersions(AUTOSARVersionID={AUTOSARVersionID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAutosarVersion(int key, [FromBody]HardwareManagement.Server.Models.FocusDB.AutosarVersion item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AutosarVersions
                    .Where(i => i.AUTOSARVersionID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.AutosarVersion>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAutosarVersionUpdated(item);
                this.context.AutosarVersions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AutosarVersions.Where(i => i.AUTOSARVersionID == key);
                
                this.OnAfterAutosarVersionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/FocusDB/AutosarVersions(AUTOSARVersionID={AUTOSARVersionID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAutosarVersion(int key, [FromBody]Delta<HardwareManagement.Server.Models.FocusDB.AutosarVersion> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AutosarVersions
                    .Where(i => i.AUTOSARVersionID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.AutosarVersion>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAutosarVersionUpdated(item);
                this.context.AutosarVersions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AutosarVersions.Where(i => i.AUTOSARVersionID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAutosarVersionCreated(HardwareManagement.Server.Models.FocusDB.AutosarVersion item);
        partial void OnAfterAutosarVersionCreated(HardwareManagement.Server.Models.FocusDB.AutosarVersion item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] HardwareManagement.Server.Models.FocusDB.AutosarVersion item)
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

                this.OnAutosarVersionCreated(item);
                this.context.AutosarVersions.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AutosarVersions.Where(i => i.AUTOSARVersionID == item.AUTOSARVersionID);

                

                this.OnAfterAutosarVersionCreated(item);

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
