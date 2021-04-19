using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext db;

        public ModuleRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync<T>(T added)
        {
            await db.AddAsync(added);
        }

        public bool Any(int id)
        {
            return db.Modules.Any(m => m.Id == id);
        }

        public async Task<IEnumerable<Module>> GetAllModules()
        {
            return await db.Modules.ToListAsync();
        }

        public Task<Module> GetModule(int? Id)
        {
            return db.Modules.FirstOrDefaultAsync(m => m.Id == Id);
        }

        //public async Task<Module> GetModule(string title)
        //{
        //    return await db.Modules.FirstOrDefaultAsync(m => m.Title == title);
        //}

        public void RemoveModule(Module module)
        {
            db.Remove(module);
        }

        public async Task<bool> SaveAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }

    }
}
