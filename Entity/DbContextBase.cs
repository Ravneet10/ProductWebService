using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public abstract class DbContextBase : DbContext
    {
        public DbContextBase(string connectionString, Action<string> databaseQueryLog = null) : base(connectionString)
        {
            if (databaseQueryLog != null)
            {
                Database.Log = databaseQueryLog;
            }

            Database.SetInitializer<DbContext>(null);
        }

        protected DbContextBase(DbConnection dbConnection) : base(dbConnection, true)
        {
            Database.SetInitializer<DbContextBase>(null);
        }


        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return base.SaveChanges();
        }
    }

}
