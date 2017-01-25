using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EntityFUnit.Models
{
    /// <summary>
    /// Database context. Whhen we instantiate a new object of this type, it will make a connection to the database and retun a context of it, 
    /// giving us access to operations in the database.
    /// </summary>
    public class MyContext : DbContext
    {
        /// <summary>
        /// List of Persons (table in the database)
        /// </summary>
        public virtual DbSet<Person> persons { get; set; }


        public MyContext() : base("name=MyContext")
        {
        }
    }
}