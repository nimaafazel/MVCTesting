using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EntityFUnit.Models;

namespace EntityFUnit.Controllers
{
    public class PeopleController : Controller
    {
        //private MyContext db = new MyContext();

        /// <summary>
        /// Interface to the PersonRepository to get the class implementing it, depending on each case.
        /// </summary>
        private IPersonRepository personRepo;

        /// <summary>
        /// Empty constructor that implements the personRepo as a EFPersonRepository, which has a context and access the database.
        /// </summary>
        public PeopleController()
        {
            personRepo = new EFPersonRepository();
        }

        /// <summary>
        /// Constructor with Dependency Injection that receives the interface (that could be a class already implemented) to implement a PersonRepository
        /// </summary>
        /// <param name="personRepository"></param>
        public PeopleController(IPersonRepository personRepository)
        {
            personRepo = personRepository;
        }

        // GET: People
        public ActionResult Index()
        {
            //return View(db.persons.ToList());
            if (personRepo.persons == null)
                return new RedirectResult("Home");

            return View(personRepo.persons.ToList());
        }
        
        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)            
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);            

            var per = personRepo.persons;  // get the set of persons

            if (per == null)  // if the set is null, then we show a bad request page
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Person person;  // Show details of this object

            // since the persons set is a IQueryable, we use LINQ to find the Person by Id
            person = per.SingleOrDefault(p => p.id == id);  // look up on the context

            // if we don't find a Person, then we show the Not Found Page
            if (person == null)            
                return HttpNotFound();
                        
            // otherwise, we found our person, and we return it as a model to our view.
            return View(person);
        }
        
        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        //// POST: People/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id,name,lastName,age")] Person person)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.persons.Add(person);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(person);
        //}

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,lastName,age")] Person person)
        {            
            if (ModelState.IsValid)
            {
                personRepo.AddOrUpdate(person);
                return new RedirectResult("Index");
            }
            return View(person);
        }


        // GET: People/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Person person = db.persons.Find(id);
        //    if (person == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(person);
        //}

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = personRepo.persons.FirstOrDefault(p => p.id == id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }


        //// POST: People/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "id,name,lastName,age")] Person person)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(person).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(person);
        //}

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,lastName,age")] Person person)
        {
            if (ModelState.IsValid)
            {
                personRepo.AddOrUpdate(person);
                return RedirectToAction("Index");
            }
            return View(person);
        }


        //// GET: People/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Person person = db.persons.Find(id);
        //    if (person == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(person);
        //}

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = personRepo.persons.FirstOrDefault(p => p.id == id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }


        // POST: People/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Person person = db.persons.Find(id);
        //    db.persons.Remove(person);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = personRepo.persons.FirstOrDefault(p => p.id == id);
            personRepo.Delete(person);            
            return RedirectToAction("Index");
        }   

        /*
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        */
    }
}
