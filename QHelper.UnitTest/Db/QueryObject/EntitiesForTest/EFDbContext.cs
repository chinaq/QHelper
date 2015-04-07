using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHelper.UnitTest.Db.QueryObject.EntitiesForTest
{
    public class EFDbContext:DbContext
    {
        public DbSet<Video> Videos { get; set; }
    }
}
