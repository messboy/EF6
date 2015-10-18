using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public partial class ContosoUniversityEntities : DbContext
    {
        public override int SaveChanges()
        {
            var entries = this.ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified)
                {
                    Console.WriteLine("proxy obj : " + entry.Entity.GetType().Name);

                    var poco = System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(entry.Entity.GetType());
                    Console.WriteLine("POCO obj : " + poco.Name);

                    entry.CurrentValues.SetValues(new { 
                        ModifiedOn = DateTime.Now,
                        OtherTableModifiedOn = DateTime.Now
                    });
                }
            }


            return base.SaveChanges();
        }
    }
}
