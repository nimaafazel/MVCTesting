using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EntityFUnit.Models
{
    /// <summary>
    /// Repository that implements the interface IPersonRepository to manage the Person model.
    /// </summary>
    public class EFPersonRepository : IPersonRepository
    {
        MyContext context = new MyContext(); // create a new context, by connecting to the database.

        /// <summary>
        /// Return the Perons list from the context.
        /// </summary>
        public IQueryable<Person> persons
        {
            get
            {
                return context.persons;
            }            
        }

        /// <summary>
        /// Add or update the changes made to Person and return the new Person changed.
        /// </summary>
        /// <param name="person"></param>
        /// <returns>The Person object modified and/or added to the context.</returns>
        public Person AddOrUpdate(Person person)
        {
            // we are assuming that if the id is zero, then we don't have it on the database (context) yet.
            if (person.id == 0)
                context.persons.Add(person);            

            // otherwise, is supposed to be in already, so we just modifiy the previous entry with the new info.
            else
                context.Entry(person).State = System.Data.Entity.EntityState.Modified;

            context.SaveChanges();  // save the changes to the context
            return person;  // return the Person object.
        }

        /// <summary>
        /// Removes a Person from the context (database).
        /// </summary>
        /// <param name="person">The Person object to be removed.</param>
        public void Delete(Person person)
        {
            context.persons.Remove(person);  // remove the Person from the context
            context.SaveChanges();  // save the changes
        }
    }
}