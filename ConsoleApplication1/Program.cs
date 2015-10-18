using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //練習預存程序 + Log
            using (var db = new ContosoUniversityEntities())
            {
                //Log 可存入其他地方
                db.Database.Log = (msg) =>
                {
                    Trace.Write(msg);
                };


                //使用view
                var data = db.vwCourse;
                foreach (var item in data)
                {
                    Console.WriteLine(item.Credits + "\t" + item.Title);
                }

                Console.Read();
            }

        }

        private static void 練習列舉()
        {
            //練習列舉
            using (var db = new ContosoUniversityEntities())
            {
                var data = db.Course;
                foreach (var item in data)
                {
                    Console.WriteLine(item.Credits + "\t" + item.Title);
                }
            }
        }

        private static void 使用預存程序()
        {
            //使用預存程序
            using (var db = new ContosoUniversityEntities())
            {
                var data = db.GetCourse("%git%");
                foreach (var item in data)
                {
                    Console.WriteLine(item.CourseID + "\t" + item.Title);
                }
            }
        }

        private static void 展示並行模式()
        {
            //展示並行模式 ,會丟出updateConcurrencyException 然後可以自行決定後續
            //參考debug_img
            using (var db = new ContosoUniversityEntities())
            {
                var c = db.Course.Find(1);
                c.Credits = CourseCreated.high;

                Console.ReadKey();
                db.SaveChanges();
            }
        }

        private static void 離線模式()
        {
            //離線模式 (不在同一個dbcontext 生命週期)

            Course c;
            using (var db = new ContosoUniversityEntities())
            {
                c = db.Course.Find(1);
                c.Credits = CourseCreated.low;

            }

            using (var db = new ContosoUniversityEntities())
            {
                db.Database.Log = Console.Write;
                //db.Course.Attach(c); //等同entry.state
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
                c.Credits = CourseCreated.mid;

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

        private static void Day1_Practics(ContosoUniversityEntities db)
        {
            // db.Configuration.ProxyCreationEnabled = false;

            var data = from p in db.Department
                       select p;

            foreach (var department in data)
            {
                Console.WriteLine(department.DepartmentID + "\t" + department.Name);

                //var courses = from q in db.Course
                //              where q.DepartmentID == department.DepartmentID
                //              select q;

                //var courses = department.Course;

                foreach (var course in department.Course)
                {
                    Console.WriteLine("\t" + course.CourseID + "\t" + course.Title);
                }
            }

            Console.WriteLine("--------------------------------------------------------");

            foreach (var c in db.Course)
            {
                Console.WriteLine(c.CourseID + "\t" + c.Title + "\t" + c.Department.Name);
            }

            Console.WriteLine("--------------------------------------------------------");

            //var c1 = new Course()
            //{
            //    Title = "Test",
            //    Credits = 5,
            //    DepartmentID=4
            //};

            //db.Course.Add(c1);
            //db.SaveChanges();

            //foreach (var c in db.Course.AsNoTracking())
            //{
            //    Console.WriteLine(c.CourseID + "\t" + c.Title + "\t" + c.Department.Name);
            //}


            //Console.WriteLine("--------------------------------------------------------");

            var c2 = db.Course.Find(7);
            c2.Instructors.Add(db.Person.Find(5));
            db.SaveChanges();


            // 取得 SQL Server 伺服器時間的方法
            DateTime dt = db.Database.SqlQuery<DateTime>("select getdate()").First();


            var sql = @"SELECT Course.CourseID, Course.Title, Course.Credits, Department.Name AS DepartmentName FROM Course INNER JOIN Department ON Course.DepartmentID = Department.DepartmentID";

            var data2 = db.Database.SqlQuery<Course>(sql);

            foreach (var item in data2)
            {
                Console.WriteLine(item.CourseID + "\t" + item.Title + "\t");
            }

            Console.WriteLine("--------------------------------------------------------");
        }

    }
}
