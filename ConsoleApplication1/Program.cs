using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Course c;
            using (var db = new ContosoUniversityEntities())
            {
                c = db.Course.Find(1);
                c.Credits = 200;             

            }

            using (var db = new ContosoUniversityEntities())
            {
                db.Database.Log = Console.Write;
                db.Entry(c).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                Console.WriteLine(c.Credits);
            }
           
        }

        private void Day2_Entity() 
        {
            using (var db = new ContosoUniversityEntities())
            {

                //db.Database.Log = Console.Write;

                Console.WriteLine("----------------------------------------------");

                var c = db.Course.Find(5);
                c.Credits = 100;

                var ce = db.Entry(c);
                ce.State = System.Data.Entity.EntityState.Modified;
                var val = ce.OriginalValues.GetValue<int>("Credits");

                if (ce.State == System.Data.Entity.EntityState.Modified)
                {
                    Console.WriteLine(ce.State);
                    Console.WriteLine(val);
                }


                db.SaveChanges();
            }
        }



    }
}
