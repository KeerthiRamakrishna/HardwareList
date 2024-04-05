using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

using HardwareManagement.Server.Data;
using HardwareManagement.Client.Pages;

namespace HardwareManagement.Server
{
    public partial class FocusDBService
    {
        FocusDBContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly FocusDBContext context;
        private readonly NavigationManager navigationManager;

        public FocusDBService(FocusDBContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportArchitecturesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/architectures/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/architectures/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportArchitecturesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/architectures/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/architectures/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnArchitecturesRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.Architecture> items);

        public async Task<IQueryable<HardwareManagement.Server.Models.FocusDB.Architecture>> GetArchitectures(Query query = null)
        {
            var items = Context.Architectures.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnArchitecturesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnArchitectureGet(HardwareManagement.Server.Models.FocusDB.Architecture item);
        partial void OnGetArchitectureByArchitectureId(ref IQueryable<HardwareManagement.Server.Models.FocusDB.Architecture> items);


        public async Task<HardwareManagement.Server.Models.FocusDB.Architecture> GetArchitectureByArchitectureId(int architectureid)
        {
            var items = Context.Architectures
                              .AsNoTracking()
                              .Where(i => i.ArchitectureID == architectureid);

 
            OnGetArchitectureByArchitectureId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnArchitectureGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnArchitectureCreated(HardwareManagement.Server.Models.FocusDB.Architecture item);
        partial void OnAfterArchitectureCreated(HardwareManagement.Server.Models.FocusDB.Architecture item);

        public async Task<HardwareManagement.Server.Models.FocusDB.Architecture> CreateArchitecture(HardwareManagement.Server.Models.FocusDB.Architecture architecture)
        {
            OnArchitectureCreated(architecture);

            var existingItem = Context.Architectures
                              .Where(i => i.ArchitectureID == architecture.ArchitectureID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Architectures.Add(architecture);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(architecture).State = EntityState.Detached;
                throw;
            }

            OnAfterArchitectureCreated(architecture);

            return architecture;
        }

        public async Task<HardwareManagement.Server.Models.FocusDB.Architecture> CancelArchitectureChanges(HardwareManagement.Server.Models.FocusDB.Architecture item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnArchitectureUpdated(HardwareManagement.Server.Models.FocusDB.Architecture item);
        partial void OnAfterArchitectureUpdated(HardwareManagement.Server.Models.FocusDB.Architecture item);

        public async Task<HardwareManagement.Server.Models.FocusDB.Architecture> UpdateArchitecture(int architectureid, HardwareManagement.Server.Models.FocusDB.Architecture architecture)
        {
            OnArchitectureUpdated(architecture);

            var itemToUpdate = Context.Architectures
                              .Where(i => i.ArchitectureID == architecture.ArchitectureID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(architecture);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterArchitectureUpdated(architecture);

            return architecture;
        }

        partial void OnArchitectureDeleted(HardwareManagement.Server.Models.FocusDB.Architecture item);
        partial void OnAfterArchitectureDeleted(HardwareManagement.Server.Models.FocusDB.Architecture item);

        public async Task<HardwareManagement.Server.Models.FocusDB.Architecture> DeleteArchitecture(int architectureid)
        {
            var itemToDelete = Context.Architectures
                              .Where(i => i.ArchitectureID == architectureid)
                              .Include(i => i.TresosAcgs)
                              .Include(i => i.TresosAutoCores)
                              .Include(i => i.TresosSafetyOs)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnArchitectureDeleted(itemToDelete);


            Context.Architectures.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterArchitectureDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAutosarVersionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/autosarversions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/autosarversions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAutosarVersionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/autosarversions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/autosarversions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAutosarVersionsRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.AutosarVersion> items);

        public async Task<IQueryable<HardwareManagement.Server.Models.FocusDB.AutosarVersion>> GetAutosarVersions(Query query = null)
        {
            var items = Context.AutosarVersions.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAutosarVersionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAutosarVersionGet(HardwareManagement.Server.Models.FocusDB.AutosarVersion item);
        partial void OnGetAutosarVersionByAutosarversionId(ref IQueryable<HardwareManagement.Server.Models.FocusDB.AutosarVersion> items);


        public async Task<HardwareManagement.Server.Models.FocusDB.AutosarVersion> GetAutosarVersionByAutosarversionId(int autosarversionid)
        {
            var items = Context.AutosarVersions
                              .AsNoTracking()
                              .Where(i => i.AUTOSARVersionID == autosarversionid);

 
            OnGetAutosarVersionByAutosarversionId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAutosarVersionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAutosarVersionCreated(HardwareManagement.Server.Models.FocusDB.AutosarVersion item);
        partial void OnAfterAutosarVersionCreated(HardwareManagement.Server.Models.FocusDB.AutosarVersion item);

        public async Task<HardwareManagement.Server.Models.FocusDB.AutosarVersion> CreateAutosarVersion(HardwareManagement.Server.Models.FocusDB.AutosarVersion autosarversion)
        {
            OnAutosarVersionCreated(autosarversion);

            var existingItem = Context.AutosarVersions
                              .Where(i => i.AUTOSARVersionID == autosarversion.AUTOSARVersionID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AutosarVersions.Add(autosarversion);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(autosarversion).State = EntityState.Detached;
                throw;
            }

            OnAfterAutosarVersionCreated(autosarversion);

            return autosarversion;
        }

        public async Task<HardwareManagement.Server.Models.FocusDB.AutosarVersion> CancelAutosarVersionChanges(HardwareManagement.Server.Models.FocusDB.AutosarVersion item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAutosarVersionUpdated(HardwareManagement.Server.Models.FocusDB.AutosarVersion item);
        partial void OnAfterAutosarVersionUpdated(HardwareManagement.Server.Models.FocusDB.AutosarVersion item);

        public async Task<HardwareManagement.Server.Models.FocusDB.AutosarVersion> UpdateAutosarVersion(int autosarversionid, HardwareManagement.Server.Models.FocusDB.AutosarVersion autosarversion)
        {
            OnAutosarVersionUpdated(autosarversion);

            var itemToUpdate = Context.AutosarVersions
                              .Where(i => i.AUTOSARVersionID == autosarversion.AUTOSARVersionID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(autosarversion);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAutosarVersionUpdated(autosarversion);

            return autosarversion;
        }

        partial void OnAutosarVersionDeleted(HardwareManagement.Server.Models.FocusDB.AutosarVersion item);
        partial void OnAfterAutosarVersionDeleted(HardwareManagement.Server.Models.FocusDB.AutosarVersion item);

        public async Task<HardwareManagement.Server.Models.FocusDB.AutosarVersion> DeleteAutosarVersion(int autosarversionid)
        {
            var itemToDelete = Context.AutosarVersions
                              .Where(i => i.AUTOSARVersionID == autosarversionid)
                              .Include(i => i.TresosAcgs)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAutosarVersionDeleted(itemToDelete);


            Context.AutosarVersions.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAutosarVersionDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAvailabilityStatusesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/availabilitystatuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/availabilitystatuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAvailabilityStatusesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/availabilitystatuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/availabilitystatuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAvailabilityStatusesRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> items);

        public async Task<IQueryable<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus>> GetAvailabilityStatuses(Query query = null)
        {
            var items = Context.AvailabilityStatuses.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAvailabilityStatusesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAvailabilityStatusGet(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item);
        partial void OnGetAvailabilityStatusByAvailabilityStatusId(ref IQueryable<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> items);


        public async Task<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> GetAvailabilityStatusByAvailabilityStatusId(int availabilitystatusid)
        {
            var items = Context.AvailabilityStatuses
                              .AsNoTracking()
                              .Where(i => i.AvailabilityStatusID == availabilitystatusid);

 
            OnGetAvailabilityStatusByAvailabilityStatusId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAvailabilityStatusGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAvailabilityStatusCreated(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item);
        partial void OnAfterAvailabilityStatusCreated(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item);

        public async Task<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> CreateAvailabilityStatus(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus availabilitystatus)
        {
            OnAvailabilityStatusCreated(availabilitystatus);

            var existingItem = Context.AvailabilityStatuses
                              .Where(i => i.AvailabilityStatusID == availabilitystatus.AvailabilityStatusID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AvailabilityStatuses.Add(availabilitystatus);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(availabilitystatus).State = EntityState.Detached;
                throw;
            }

            OnAfterAvailabilityStatusCreated(availabilitystatus);

            return availabilitystatus;
        }

        public async Task<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> CancelAvailabilityStatusChanges(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAvailabilityStatusUpdated(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item);
        partial void OnAfterAvailabilityStatusUpdated(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item);

        public async Task<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> UpdateAvailabilityStatus(int availabilitystatusid, HardwareManagement.Server.Models.FocusDB.AvailabilityStatus availabilitystatus)
        {
            OnAvailabilityStatusUpdated(availabilitystatus);

            var itemToUpdate = Context.AvailabilityStatuses
                              .Where(i => i.AvailabilityStatusID == availabilitystatus.AvailabilityStatusID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(availabilitystatus);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAvailabilityStatusUpdated(availabilitystatus);

            return availabilitystatus;
        }

        partial void OnAvailabilityStatusDeleted(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item);
        partial void OnAfterAvailabilityStatusDeleted(HardwareManagement.Server.Models.FocusDB.AvailabilityStatus item);

        public async Task<HardwareManagement.Server.Models.FocusDB.AvailabilityStatus> DeleteAvailabilityStatus(int availabilitystatusid)
        {
            var itemToDelete = Context.AvailabilityStatuses
                              .Where(i => i.AvailabilityStatusID == availabilitystatusid)
                              .Include(i => i.MicroControllers)
                              .Include(i => i.TresosAcgs)
                              .Include(i => i.TresosAutoCores)
                              .Include(i => i.TresosSafetyOs)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAvailabilityStatusDeleted(itemToDelete);


            Context.AvailabilityStatuses.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAvailabilityStatusDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCompilerVendorsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/compilervendors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/compilervendors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCompilerVendorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/compilervendors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/compilervendors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCompilerVendorsRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.CompilerVendor> items);

        public async Task<IQueryable<HardwareManagement.Server.Models.FocusDB.CompilerVendor>> GetCompilerVendors(Query query = null)
        {
            var items = Context.CompilerVendors.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCompilerVendorsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCompilerVendorGet(HardwareManagement.Server.Models.FocusDB.CompilerVendor item);
        partial void OnGetCompilerVendorByCompilerVendorId(ref IQueryable<HardwareManagement.Server.Models.FocusDB.CompilerVendor> items);


        public async Task<HardwareManagement.Server.Models.FocusDB.CompilerVendor> GetCompilerVendorByCompilerVendorId(int compilervendorid)
        {
            var items = Context.CompilerVendors
                              .AsNoTracking()
                              .Where(i => i.CompilerVendorID == compilervendorid);

 
            OnGetCompilerVendorByCompilerVendorId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCompilerVendorGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCompilerVendorCreated(HardwareManagement.Server.Models.FocusDB.CompilerVendor item);
        partial void OnAfterCompilerVendorCreated(HardwareManagement.Server.Models.FocusDB.CompilerVendor item);

        public async Task<HardwareManagement.Server.Models.FocusDB.CompilerVendor> CreateCompilerVendor(HardwareManagement.Server.Models.FocusDB.CompilerVendor compilervendor)
        {
            OnCompilerVendorCreated(compilervendor);

            var existingItem = Context.CompilerVendors
                              .Where(i => i.CompilerVendorID == compilervendor.CompilerVendorID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.CompilerVendors.Add(compilervendor);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(compilervendor).State = EntityState.Detached;
                throw;
            }

            OnAfterCompilerVendorCreated(compilervendor);

            return compilervendor;
        }

        public async Task<HardwareManagement.Server.Models.FocusDB.CompilerVendor> CancelCompilerVendorChanges(HardwareManagement.Server.Models.FocusDB.CompilerVendor item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCompilerVendorUpdated(HardwareManagement.Server.Models.FocusDB.CompilerVendor item);
        partial void OnAfterCompilerVendorUpdated(HardwareManagement.Server.Models.FocusDB.CompilerVendor item);

        public async Task<HardwareManagement.Server.Models.FocusDB.CompilerVendor> UpdateCompilerVendor(int compilervendorid, HardwareManagement.Server.Models.FocusDB.CompilerVendor compilervendor)
        {
            OnCompilerVendorUpdated(compilervendor);

            var itemToUpdate = Context.CompilerVendors
                              .Where(i => i.CompilerVendorID == compilervendor.CompilerVendorID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(compilervendor);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCompilerVendorUpdated(compilervendor);

            return compilervendor;
        }

        partial void OnCompilerVendorDeleted(HardwareManagement.Server.Models.FocusDB.CompilerVendor item);
        partial void OnAfterCompilerVendorDeleted(HardwareManagement.Server.Models.FocusDB.CompilerVendor item);

        public async Task<HardwareManagement.Server.Models.FocusDB.CompilerVendor> DeleteCompilerVendor(int compilervendorid)
        {
            var itemToDelete = Context.CompilerVendors
                              .Where(i => i.CompilerVendorID == compilervendorid)
                              .Include(i => i.TresosAcgs)
                              .Include(i => i.TresosAutoCores)
                              .Include(i => i.TresosSafetyOs)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCompilerVendorDeleted(itemToDelete);


            Context.CompilerVendors.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCompilerVendorDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCompilerVersionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/compilerversions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/compilerversions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCompilerVersionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/compilerversions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/compilerversions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCompilerVersionsRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.CompilerVersion> items);

        public async Task<IQueryable<HardwareManagement.Server.Models.FocusDB.CompilerVersion>> GetCompilerVersions(Query query = null)
        {
            var items = Context.CompilerVersions.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCompilerVersionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCompilerVersionGet(HardwareManagement.Server.Models.FocusDB.CompilerVersion item);
        partial void OnGetCompilerVersionByCompilerVersionId(ref IQueryable<HardwareManagement.Server.Models.FocusDB.CompilerVersion> items);


        public async Task<HardwareManagement.Server.Models.FocusDB.CompilerVersion> GetCompilerVersionByCompilerVersionId(int compilerversionid)
        {
            var items = Context.CompilerVersions
                              .AsNoTracking()
                              .Where(i => i.CompilerVersionID == compilerversionid);

 
            OnGetCompilerVersionByCompilerVersionId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCompilerVersionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCompilerVersionCreated(HardwareManagement.Server.Models.FocusDB.CompilerVersion item);
        partial void OnAfterCompilerVersionCreated(HardwareManagement.Server.Models.FocusDB.CompilerVersion item);

        public async Task<HardwareManagement.Server.Models.FocusDB.CompilerVersion> CreateCompilerVersion(HardwareManagement.Server.Models.FocusDB.CompilerVersion compilerversion)
        {
            OnCompilerVersionCreated(compilerversion);

            var existingItem = Context.CompilerVersions
                              .Where(i => i.CompilerVersionID == compilerversion.CompilerVersionID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.CompilerVersions.Add(compilerversion);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(compilerversion).State = EntityState.Detached;
                throw;
            }

            OnAfterCompilerVersionCreated(compilerversion);

            return compilerversion;
        }

        public async Task<HardwareManagement.Server.Models.FocusDB.CompilerVersion> CancelCompilerVersionChanges(HardwareManagement.Server.Models.FocusDB.CompilerVersion item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCompilerVersionUpdated(HardwareManagement.Server.Models.FocusDB.CompilerVersion item);
        partial void OnAfterCompilerVersionUpdated(HardwareManagement.Server.Models.FocusDB.CompilerVersion item);

        public async Task<HardwareManagement.Server.Models.FocusDB.CompilerVersion> UpdateCompilerVersion(int compilerversionid, HardwareManagement.Server.Models.FocusDB.CompilerVersion compilerversion)
        {
            OnCompilerVersionUpdated(compilerversion);

            var itemToUpdate = Context.CompilerVersions
                              .Where(i => i.CompilerVersionID == compilerversion.CompilerVersionID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(compilerversion);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCompilerVersionUpdated(compilerversion);

            return compilerversion;
        }

        partial void OnCompilerVersionDeleted(HardwareManagement.Server.Models.FocusDB.CompilerVersion item);
        partial void OnAfterCompilerVersionDeleted(HardwareManagement.Server.Models.FocusDB.CompilerVersion item);

        public async Task<HardwareManagement.Server.Models.FocusDB.CompilerVersion> DeleteCompilerVersion(int compilerversionid)
        {
            var itemToDelete = Context.CompilerVersions
                              .Where(i => i.CompilerVersionID == compilerversionid)
                              .Include(i => i.TresosAcgs)
                              .Include(i => i.TresosAutoCores)
                              .Include(i => i.TresosSafetyOs)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCompilerVersionDeleted(itemToDelete);


            Context.CompilerVersions.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCompilerVersionDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportMicroControllerDerivativesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/microcontrollerderivatives/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/microcontrollerderivatives/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMicroControllerDerivativesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/microcontrollerderivatives/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/microcontrollerderivatives/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMicroControllerDerivativesRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> items);

        public async Task<IQueryable<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative>> GetMicroControllerDerivatives(Query query = null)
        {
            var items = Context.MicroControllerDerivatives.AsQueryable();

            items = items.Include(i => i.SiliconVendor);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnMicroControllerDerivativesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMicroControllerDerivativeGet(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item);
        partial void OnGetMicroControllerDerivativeByMicroControllerDerivativesId(ref IQueryable<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> items);


        public async Task<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> GetMicroControllerDerivativeByMicroControllerDerivativesId(int microcontrollerderivativesid)
        {
            var items = Context.MicroControllerDerivatives
                              .AsNoTracking()
                              .Where(i => i.MicroControllerDerivativesID == microcontrollerderivativesid);

            items = items.Include(i => i.SiliconVendor);
 
            OnGetMicroControllerDerivativeByMicroControllerDerivativesId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnMicroControllerDerivativeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMicroControllerDerivativeCreated(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item);
        partial void OnAfterMicroControllerDerivativeCreated(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item);

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> CreateMicroControllerDerivative(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative microcontrollerderivative)
        {
            OnMicroControllerDerivativeCreated(microcontrollerderivative);

            var existingItem = Context.MicroControllerDerivatives
                              .Where(i => i.MicroControllerDerivativesID == microcontrollerderivative.MicroControllerDerivativesID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.MicroControllerDerivatives.Add(microcontrollerderivative);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(microcontrollerderivative).State = EntityState.Detached;
                throw;
            }

            OnAfterMicroControllerDerivativeCreated(microcontrollerderivative);

            return microcontrollerderivative;
        }

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> CancelMicroControllerDerivativeChanges(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMicroControllerDerivativeUpdated(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item);
        partial void OnAfterMicroControllerDerivativeUpdated(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item);

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> UpdateMicroControllerDerivative(int microcontrollerderivativesid, HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative microcontrollerderivative)
        {
            OnMicroControllerDerivativeUpdated(microcontrollerderivative);

            var itemToUpdate = Context.MicroControllerDerivatives
                              .Where(i => i.MicroControllerDerivativesID == microcontrollerderivative.MicroControllerDerivativesID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(microcontrollerderivative);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMicroControllerDerivativeUpdated(microcontrollerderivative);

            return microcontrollerderivative;
        }

        partial void OnMicroControllerDerivativeDeleted(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item);
        partial void OnAfterMicroControllerDerivativeDeleted(HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative item);

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroControllerDerivative> DeleteMicroControllerDerivative(int microcontrollerderivativesid)
        {
            var itemToDelete = Context.MicroControllerDerivatives
                              .Where(i => i.MicroControllerDerivativesID == microcontrollerderivativesid)
                              .Include(i => i.MicroControllers)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMicroControllerDerivativeDeleted(itemToDelete);


            Context.MicroControllerDerivatives.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMicroControllerDerivativeDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportMicroControllersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/microcontrollers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/microcontrollers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMicroControllersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/microcontrollers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/microcontrollers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMicroControllersRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.MicroController> items);

        public async Task<IQueryable<HardwareManagement.Server.Models.FocusDB.MicroController>> GetMicroControllers(Query query = null)
        {
            var items = Context.MicroControllers.AsQueryable();

            items = items.Include(i => i.AvailabilityStatus);
            items = items.Include(i => i.MicroControllerDerivative);
            items = items.Include(i => i.MicroControllerSubDerivative);
            items = items.Include(i => i.SiliconVendor);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnMicroControllersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMicroControllerGet(HardwareManagement.Server.Models.FocusDB.MicroController item);
        partial void OnGetMicroControllerByHardwareId(ref IQueryable<HardwareManagement.Server.Models.FocusDB.MicroController> items);


        public async Task<HardwareManagement.Server.Models.FocusDB.MicroController> GetMicroControllerByHardwareId(int microControllersID)
        {
            var items = Context.MicroControllers
            .AsNoTracking()
                              .Where(i => i.MicroControllersID == microControllersID);

            items = items.Include(i => i.AvailabilityStatus);
            items = items.Include(i => i.MicroControllerDerivative);
            items = items.Include(i => i.MicroControllerSubDerivative);
            items = items.Include(i => i.SiliconVendor);
 
            OnGetMicroControllerByHardwareId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnMicroControllerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMicroControllerCreated(HardwareManagement.Server.Models.FocusDB.MicroController item);
        partial void OnAfterMicroControllerCreated(HardwareManagement.Server.Models.FocusDB.MicroController item);

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroController> CreateMicroController(HardwareManagement.Server.Models.FocusDB.MicroController microcontroller)
        {
            OnMicroControllerCreated(microcontroller);

            var existingItem = Context.MicroControllers
                              .Where(i => i.MicroControllersID == microcontroller.MicroControllersID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.MicroControllers.Add(microcontroller);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(microcontroller).State = EntityState.Detached;
                throw;
            }

            OnAfterMicroControllerCreated(microcontroller);

            return microcontroller;
        }

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroController> CancelMicroControllerChanges(HardwareManagement.Server.Models.FocusDB.MicroController item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMicroControllerUpdated(HardwareManagement.Server.Models.FocusDB.MicroController item);
        partial void OnAfterMicroControllerUpdated(HardwareManagement.Server.Models.FocusDB.MicroController item);

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroController> UpdateMicroController(int microControllersID, HardwareManagement.Server.Models.FocusDB.MicroController microcontroller)
        {
            OnMicroControllerUpdated(microcontroller);

            var itemToUpdate = Context.MicroControllers
                              .Where(i => i.MicroControllersID == microcontroller.MicroControllersID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(microcontroller);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMicroControllerUpdated(microcontroller);

            return microcontroller;
        }

        partial void OnMicroControllerDeleted(HardwareManagement.Server.Models.FocusDB.MicroController item);
        partial void OnAfterMicroControllerDeleted(HardwareManagement.Server.Models.FocusDB.MicroController item);

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroController> DeleteMicroController(int microControllersID)
        {
            var itemToDelete = Context.MicroControllers
                              .Where(i => i.MicroControllersID == microControllersID)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMicroControllerDeleted(itemToDelete);


            Context.MicroControllers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMicroControllerDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportMicroControllerSubDerivativesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/microcontrollersubderivatives/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/microcontrollersubderivatives/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMicroControllerSubDerivativesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/microcontrollersubderivatives/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/microcontrollersubderivatives/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMicroControllerSubDerivativesRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> items);

        public async Task<IQueryable<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative>> GetMicroControllerSubDerivatives(Query query = null)
        {
            var items = Context.MicroControllerSubDerivatives.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnMicroControllerSubDerivativesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMicroControllerSubDerivativeGet(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item);
        partial void OnGetMicroControllerSubDerivativeByMicroControllerSubDerivativesId(ref IQueryable<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> items);


        public async Task<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> GetMicroControllerSubDerivativeByMicroControllerSubDerivativesId(int microcontrollersubderivativesid)
        {
            var items = Context.MicroControllerSubDerivatives
                              .AsNoTracking()
                              .Where(i => i.MicroControllerSubDerivativesID == microcontrollersubderivativesid);

 
            OnGetMicroControllerSubDerivativeByMicroControllerSubDerivativesId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnMicroControllerSubDerivativeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMicroControllerSubDerivativeCreated(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item);
        partial void OnAfterMicroControllerSubDerivativeCreated(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item);

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> CreateMicroControllerSubDerivative(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative microcontrollersubderivative)
        {
            OnMicroControllerSubDerivativeCreated(microcontrollersubderivative);

            var existingItem = Context.MicroControllerSubDerivatives
                              .Where(i => i.MicroControllerSubDerivativesID == microcontrollersubderivative.MicroControllerSubDerivativesID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.MicroControllerSubDerivatives.Add(microcontrollersubderivative);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(microcontrollersubderivative).State = EntityState.Detached;
                throw;
            }

            OnAfterMicroControllerSubDerivativeCreated(microcontrollersubderivative);

            return microcontrollersubderivative;
        }

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> CancelMicroControllerSubDerivativeChanges(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMicroControllerSubDerivativeUpdated(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item);
        partial void OnAfterMicroControllerSubDerivativeUpdated(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item);

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> UpdateMicroControllerSubDerivative(int microcontrollersubderivativesid, HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative microcontrollersubderivative)
        {
            OnMicroControllerSubDerivativeUpdated(microcontrollersubderivative);

            var itemToUpdate = Context.MicroControllerSubDerivatives
                              .Where(i => i.MicroControllerSubDerivativesID == microcontrollersubderivative.MicroControllerSubDerivativesID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(microcontrollersubderivative);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMicroControllerSubDerivativeUpdated(microcontrollersubderivative);

            return microcontrollersubderivative;
        }

        partial void OnMicroControllerSubDerivativeDeleted(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item);
        partial void OnAfterMicroControllerSubDerivativeDeleted(HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative item);

        public async Task<HardwareManagement.Server.Models.FocusDB.MicroControllerSubDerivative> DeleteMicroControllerSubDerivative(int microcontrollersubderivativesid)
        {
            var itemToDelete = Context.MicroControllerSubDerivatives
                              .Where(i => i.MicroControllerSubDerivativesID == microcontrollersubderivativesid)
                              .Include(i => i.MicroControllers)
                              .Include(i => i.TresosAcgs)
                              .Include(i => i.TresosAutoCores)
                              .Include(i => i.TresosSafetyOs)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMicroControllerSubDerivativeDeleted(itemToDelete);


            Context.MicroControllerSubDerivatives.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMicroControllerSubDerivativeDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSiliconVendorsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/siliconvendors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/siliconvendors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSiliconVendorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/siliconvendors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/siliconvendors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSiliconVendorsRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.SiliconVendor> items);

        public async Task<IQueryable<HardwareManagement.Server.Models.FocusDB.SiliconVendor>> GetSiliconVendors(Query query = null)
        {
            var items = Context.SiliconVendors.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSiliconVendorsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSiliconVendorGet(HardwareManagement.Server.Models.FocusDB.SiliconVendor item);
        partial void OnGetSiliconVendorBySiliconVendorId(ref IQueryable<HardwareManagement.Server.Models.FocusDB.SiliconVendor> items);


        public async Task<HardwareManagement.Server.Models.FocusDB.SiliconVendor> GetSiliconVendorBySiliconVendorId(int siliconvendorid)
        {
            var items = Context.SiliconVendors
                              .AsNoTracking()
                              .Where(i => i.SiliconVendorID == siliconvendorid);

 
            OnGetSiliconVendorBySiliconVendorId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSiliconVendorGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSiliconVendorCreated(HardwareManagement.Server.Models.FocusDB.SiliconVendor item);
        partial void OnAfterSiliconVendorCreated(HardwareManagement.Server.Models.FocusDB.SiliconVendor item);

        public async Task<HardwareManagement.Server.Models.FocusDB.SiliconVendor> CreateSiliconVendor(HardwareManagement.Server.Models.FocusDB.SiliconVendor siliconvendor)
        {
            OnSiliconVendorCreated(siliconvendor);

            var existingItem = Context.SiliconVendors
                              .Where(i => i.SiliconVendorID == siliconvendor.SiliconVendorID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.SiliconVendors.Add(siliconvendor);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(siliconvendor).State = EntityState.Detached;
                throw;
            }

            OnAfterSiliconVendorCreated(siliconvendor);

            return siliconvendor;
        }

        public async Task<HardwareManagement.Server.Models.FocusDB.SiliconVendor> CancelSiliconVendorChanges(HardwareManagement.Server.Models.FocusDB.SiliconVendor item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSiliconVendorUpdated(HardwareManagement.Server.Models.FocusDB.SiliconVendor item);
        partial void OnAfterSiliconVendorUpdated(HardwareManagement.Server.Models.FocusDB.SiliconVendor item);

        public async Task<HardwareManagement.Server.Models.FocusDB.SiliconVendor> UpdateSiliconVendor(int siliconvendorid, HardwareManagement.Server.Models.FocusDB.SiliconVendor siliconvendor)
        {
            OnSiliconVendorUpdated(siliconvendor);

            var itemToUpdate = Context.SiliconVendors
                              .Where(i => i.SiliconVendorID == siliconvendor.SiliconVendorID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(siliconvendor);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSiliconVendorUpdated(siliconvendor);

            return siliconvendor;
        }

        partial void OnSiliconVendorDeleted(HardwareManagement.Server.Models.FocusDB.SiliconVendor item);
        partial void OnAfterSiliconVendorDeleted(HardwareManagement.Server.Models.FocusDB.SiliconVendor item);

        public async Task<HardwareManagement.Server.Models.FocusDB.SiliconVendor> DeleteSiliconVendor(int siliconvendorid)
        {
            var itemToDelete = Context.SiliconVendors
                              .Where(i => i.SiliconVendorID == siliconvendorid)
                              .Include(i => i.MicroControllerDerivatives)
                              .Include(i => i.MicroControllers)
                              .Include(i => i.TresosAcgs)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSiliconVendorDeleted(itemToDelete);


            Context.SiliconVendors.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSiliconVendorDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTresosAcgsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/tresosacgs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/tresosacgs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTresosAcgsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/tresosacgs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/tresosacgs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTresosAcgsRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.TresosAcg> items);

        public async Task<IQueryable<HardwareManagement.Server.Models.FocusDB.TresosAcg>> GetTresosAcgs(Query query = null)
        {
            var items = Context.TresosAcgs.AsQueryable();

            items = items.Include(i => i.Architecture);
            items = items.Include(i => i.Autosarversion);
            items = items.Include(i => i.AvailabilityStatus);
            items = items.Include(i => i.CompilerVendor);
            items = items.Include(i => i.CompilerVersion);
            items = items.Include(i => i.MicroControllerSubDerivative);
            items = items.Include(i => i.SiliconVendor);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTresosAcgsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTresosAcgGet(HardwareManagement.Server.Models.FocusDB.TresosAcg item);
        partial void OnGetTresosAcgByTresosAcgid(ref IQueryable<HardwareManagement.Server.Models.FocusDB.TresosAcg> items);


        public async Task<HardwareManagement.Server.Models.FocusDB.TresosAcg> GetTresosAcgByTresosAcgid(int tresosacgid)
        {
            var items = Context.TresosAcgs
                              .AsNoTracking()
                              .Where(i => i.TresosACGID == tresosacgid);

            items = items.Include(i => i.Architecture);
            items = items.Include(i => i.Autosarversion);
            items = items.Include(i => i.AvailabilityStatus);
            items = items.Include(i => i.CompilerVendor);
            items = items.Include(i => i.CompilerVersion);
            items = items.Include(i => i.MicroControllerSubDerivative);
            items = items.Include(i => i.SiliconVendor);
 
            OnGetTresosAcgByTresosAcgid(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTresosAcgGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTresosAcgCreated(HardwareManagement.Server.Models.FocusDB.TresosAcg item);
        partial void OnAfterTresosAcgCreated(HardwareManagement.Server.Models.FocusDB.TresosAcg item);

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosAcg> CreateTresosAcg(HardwareManagement.Server.Models.FocusDB.TresosAcg tresosacg)
        {
            OnTresosAcgCreated(tresosacg);

            var existingItem = Context.TresosAcgs
                              .Where(i => i.TresosACGID == tresosacg.TresosACGID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TresosAcgs.Add(tresosacg);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tresosacg).State = EntityState.Detached;
                throw;
            }

            OnAfterTresosAcgCreated(tresosacg);

            return tresosacg;
        }

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosAcg> CancelTresosAcgChanges(HardwareManagement.Server.Models.FocusDB.TresosAcg item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTresosAcgUpdated(HardwareManagement.Server.Models.FocusDB.TresosAcg item);
        partial void OnAfterTresosAcgUpdated(HardwareManagement.Server.Models.FocusDB.TresosAcg item);

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosAcg> UpdateTresosAcg(int tresosacgid, HardwareManagement.Server.Models.FocusDB.TresosAcg tresosacg)
        {
            OnTresosAcgUpdated(tresosacg);

            var itemToUpdate = Context.TresosAcgs
                              .Where(i => i.TresosACGID == tresosacg.TresosACGID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tresosacg);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTresosAcgUpdated(tresosacg);

            return tresosacg;
        }

        partial void OnTresosAcgDeleted(HardwareManagement.Server.Models.FocusDB.TresosAcg item);
        partial void OnAfterTresosAcgDeleted(HardwareManagement.Server.Models.FocusDB.TresosAcg item);

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosAcg> DeleteTresosAcg(int tresosacgid)
        {
            var itemToDelete = Context.TresosAcgs
                              .Where(i => i.TresosACGID == tresosacgid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTresosAcgDeleted(itemToDelete);


            Context.TresosAcgs.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTresosAcgDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTresosAutoCoresToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/tresosautocores/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/tresosautocores/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTresosAutoCoresToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/tresosautocores/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/tresosautocores/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTresosAutoCoresRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> items);

        public async Task<IQueryable<HardwareManagement.Server.Models.FocusDB.TresosAutoCore>> GetTresosAutoCores(Query query = null)
        {
            var items = Context.TresosAutoCores.AsQueryable();

            items = items.Include(i => i.Architecture);
            items = items.Include(i => i.AvailabilityStatus);
            items = items.Include(i => i.CompilerVendor);
            items = items.Include(i => i.CompilerVersion);
            items = items.Include(i => i.MicroControllerSubDerivative);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTresosAutoCoresRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTresosAutoCoreGet(HardwareManagement.Server.Models.FocusDB.TresosAutoCore item);
        partial void OnGetTresosAutoCoreByTresosAutoCoreId(ref IQueryable<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> items);


        public async Task<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> GetTresosAutoCoreByTresosAutoCoreId(int tresosautocoreid)
        {
            var items = Context.TresosAutoCores
                              .AsNoTracking()
                              .Where(i => i.TresosAutoCoreID == tresosautocoreid);

            items = items.Include(i => i.Architecture);
            items = items.Include(i => i.AvailabilityStatus);
            items = items.Include(i => i.CompilerVendor);
            items = items.Include(i => i.CompilerVersion);
            items = items.Include(i => i.MicroControllerSubDerivative);
 
            OnGetTresosAutoCoreByTresosAutoCoreId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTresosAutoCoreGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTresosAutoCoreCreated(HardwareManagement.Server.Models.FocusDB.TresosAutoCore item);
        partial void OnAfterTresosAutoCoreCreated(HardwareManagement.Server.Models.FocusDB.TresosAutoCore item);

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> CreateTresosAutoCore(HardwareManagement.Server.Models.FocusDB.TresosAutoCore tresosautocore)
        {
            OnTresosAutoCoreCreated(tresosautocore);

            var existingItem = Context.TresosAutoCores
                              .Where(i => i.TresosAutoCoreID == tresosautocore.TresosAutoCoreID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TresosAutoCores.Add(tresosautocore);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tresosautocore).State = EntityState.Detached;
                throw;
            }

            OnAfterTresosAutoCoreCreated(tresosautocore);

            return tresosautocore;
        }

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> CancelTresosAutoCoreChanges(HardwareManagement.Server.Models.FocusDB.TresosAutoCore item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTresosAutoCoreUpdated(HardwareManagement.Server.Models.FocusDB.TresosAutoCore item);
        partial void OnAfterTresosAutoCoreUpdated(HardwareManagement.Server.Models.FocusDB.TresosAutoCore item);

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> UpdateTresosAutoCore(int tresosautocoreid, HardwareManagement.Server.Models.FocusDB.TresosAutoCore tresosautocore)
        {
            OnTresosAutoCoreUpdated(tresosautocore);

            var itemToUpdate = Context.TresosAutoCores
                              .Where(i => i.TresosAutoCoreID == tresosautocore.TresosAutoCoreID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tresosautocore);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTresosAutoCoreUpdated(tresosautocore);

            return tresosautocore;
        }

        partial void OnTresosAutoCoreDeleted(HardwareManagement.Server.Models.FocusDB.TresosAutoCore item);
        partial void OnAfterTresosAutoCoreDeleted(HardwareManagement.Server.Models.FocusDB.TresosAutoCore item);

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosAutoCore> DeleteTresosAutoCore(int tresosautocoreid)
        {
            var itemToDelete = Context.TresosAutoCores
                              .Where(i => i.TresosAutoCoreID == tresosautocoreid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTresosAutoCoreDeleted(itemToDelete);


            Context.TresosAutoCores.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTresosAutoCoreDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTresosSafetyOsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/tresossafetyos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/tresossafetyos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTresosSafetyOsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/focusdb/tresossafetyos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/focusdb/tresossafetyos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTresosSafetyOsRead(ref IQueryable<HardwareManagement.Server.Models.FocusDB.TresosSafetyO> items);

        public async Task<IQueryable<HardwareManagement.Server.Models.FocusDB.TresosSafetyO>> GetTresosSafetyOs(Query query = null)
        {
            var items = Context.TresosSafetyOs.AsQueryable();

            items = items.Include(i => i.Architecture);
            items = items.Include(i => i.AvailabilityStatus);
            items = items.Include(i => i.CompilerVendor);
            items = items.Include(i => i.CompilerVersion);
            items = items.Include(i => i.MicroControllerSubDerivative);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTresosSafetyOsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTresosSafetyOGet(HardwareManagement.Server.Models.FocusDB.TresosSafetyO item);
        partial void OnGetTresosSafetyOByTresosSafetyOsid(ref IQueryable<HardwareManagement.Server.Models.FocusDB.TresosSafetyO> items);


        public async Task<HardwareManagement.Server.Models.FocusDB.TresosSafetyO> GetTresosSafetyOByTresosSafetyOsid(int tresossafetyosid)
        {
            var items = Context.TresosSafetyOs
                              .AsNoTracking()
                              .Where(i => i.TresosSafetyOSID == tresossafetyosid);

            items = items.Include(i => i.Architecture);
            items = items.Include(i => i.AvailabilityStatus);
            items = items.Include(i => i.CompilerVendor);
            items = items.Include(i => i.CompilerVersion);
            items = items.Include(i => i.MicroControllerSubDerivative);
 
            OnGetTresosSafetyOByTresosSafetyOsid(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTresosSafetyOGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTresosSafetyOCreated(HardwareManagement.Server.Models.FocusDB.TresosSafetyO item);
        partial void OnAfterTresosSafetyOCreated(HardwareManagement.Server.Models.FocusDB.TresosSafetyO item);

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosSafetyO> CreateTresosSafetyO(HardwareManagement.Server.Models.FocusDB.TresosSafetyO tresossafetyo)
        {
            OnTresosSafetyOCreated(tresossafetyo);

            var existingItem = Context.TresosSafetyOs
                              .Where(i => i.TresosSafetyOSID == tresossafetyo.TresosSafetyOSID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TresosSafetyOs.Add(tresossafetyo);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tresossafetyo).State = EntityState.Detached;
                throw;
            }

            OnAfterTresosSafetyOCreated(tresossafetyo);

            return tresossafetyo;
        }

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosSafetyO> CancelTresosSafetyOChanges(HardwareManagement.Server.Models.FocusDB.TresosSafetyO item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTresosSafetyOUpdated(HardwareManagement.Server.Models.FocusDB.TresosSafetyO item);
        partial void OnAfterTresosSafetyOUpdated(HardwareManagement.Server.Models.FocusDB.TresosSafetyO item);

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosSafetyO> UpdateTresosSafetyO(int tresossafetyosid, HardwareManagement.Server.Models.FocusDB.TresosSafetyO tresossafetyo)
        {
            OnTresosSafetyOUpdated(tresossafetyo);

            var itemToUpdate = Context.TresosSafetyOs
                              .Where(i => i.TresosSafetyOSID == tresossafetyo.TresosSafetyOSID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tresossafetyo);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTresosSafetyOUpdated(tresossafetyo);

            return tresossafetyo;
        }

        partial void OnTresosSafetyODeleted(HardwareManagement.Server.Models.FocusDB.TresosSafetyO item);
        partial void OnAfterTresosSafetyODeleted(HardwareManagement.Server.Models.FocusDB.TresosSafetyO item);

        public async Task<HardwareManagement.Server.Models.FocusDB.TresosSafetyO> DeleteTresosSafetyO(int tresossafetyosid)
        {
            var itemToDelete = Context.TresosSafetyOs
                              .Where(i => i.TresosSafetyOSID == tresossafetyosid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTresosSafetyODeleted(itemToDelete);


            Context.TresosSafetyOs.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTresosSafetyODeleted(itemToDelete);

            return itemToDelete;
        }
        }
}