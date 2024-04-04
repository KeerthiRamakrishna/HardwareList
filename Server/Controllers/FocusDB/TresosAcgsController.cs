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
    //[Route("odata/FocusDB/TresosAcgs")]
    [Route("api/[controller]/[action]")]
    public partial class TresosAcgsController : ODataController
    {
        private HardwareManagement.Server.Data.FocusDBContext context;

        public TresosAcgsController(HardwareManagement.Server.Data.FocusDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<HardwareManagement.Server.Models.FocusDB.TresosAcg> GetTresosAcgs()
        {
            var items = this.context.TresosAcgs.AsQueryable<HardwareManagement.Server.Models.FocusDB.TresosAcg>();
            this.OnTresosAcgsRead(ref items);

            return items;
        }

        partial void OnTresosAcgsRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.TresosAcg> items);

        partial void OnTresosAcgGet(ref SingleResult<HardwareManagement.Server.Models.FocusDB.TresosAcg> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/FocusDB/TresosAcgs(TresosACGID={TresosACGID})")]
        public SingleResult<HardwareManagement.Server.Models.FocusDB.TresosAcg> GetTresosAcg(int key)
        {
            var items = this.context.TresosAcgs.Where(i => i.TresosACGID == key);
            var result = SingleResult.Create(items);

            OnTresosAcgGet(ref result);

            return result;
        }
        partial void OnTresosAcgDeleted(HardwareManagement.Server.Models.FocusDB.TresosAcg item);
        partial void OnAfterTresosAcgDeleted(HardwareManagement.Server.Models.FocusDB.TresosAcg item);

        [HttpDelete("/odata/FocusDB/TresosAcgs(TresosACGID={TresosACGID})")]
        public IActionResult DeleteTresosAcg(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.TresosAcgs
                    .Where(i => i.TresosACGID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.TresosAcg>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnTresosAcgDeleted(item);
                this.context.TresosAcgs.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTresosAcgDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTresosAcgUpdated(HardwareManagement.Server.Models.FocusDB.TresosAcg item);
        partial void OnAfterTresosAcgUpdated(HardwareManagement.Server.Models.FocusDB.TresosAcg item);

        [HttpPut("/odata/FocusDB/TresosAcgs(TresosACGID={TresosACGID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTresosAcg(int key, [FromBody]HardwareManagement.Server.Models.FocusDB.TresosAcg item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.TresosAcgs
                    .Where(i => i.TresosACGID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.TresosAcg>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnTresosAcgUpdated(item);
                this.context.TresosAcgs.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TresosAcgs.Where(i => i.TresosACGID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Architecture,Autosarversion,AvailabilityStatus,CompilerVendor,CompilerVersion,MicroControllerSubDerivative,SiliconVendor");
                this.OnAfterTresosAcgUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/FocusDB/TresosAcgs(TresosACGID={TresosACGID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTresosAcg(int key, [FromBody]Delta<HardwareManagement.Server.Models.FocusDB.TresosAcg> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.TresosAcgs
                    .Where(i => i.TresosACGID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<HardwareManagement.Server.Models.FocusDB.TresosAcg>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnTresosAcgUpdated(item);
                this.context.TresosAcgs.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TresosAcgs.Where(i => i.TresosACGID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Architecture,Autosarversion,AvailabilityStatus,CompilerVendor,CompilerVersion,MicroControllerSubDerivative,SiliconVendor");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTresosAcgCreated(HardwareManagement.Server.Models.FocusDB.TresosAcg item);
        partial void OnAfterTresosAcgCreated(HardwareManagement.Server.Models.FocusDB.TresosAcg item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] HardwareManagement.Server.Models.FocusDB.TresosAcg item)
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

                this.OnTresosAcgCreated(item);
                this.context.TresosAcgs.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TresosAcgs.Where(i => i.TresosACGID == item.TresosACGID);

                Request.QueryString = Request.QueryString.Add("$expand", "Architecture,Autosarversion,AvailabilityStatus,CompilerVendor,CompilerVersion,MicroControllerSubDerivative,SiliconVendor");

                this.OnAfterTresosAcgCreated(item);

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
