using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFUnit.Models
{
    /// <summary>
    /// Interface to decouple the Person model with a PersonRepository.
    /// </summary>
    public interface IPersonRepository
    {
        IQueryable<Person> persons { get; }
        Person AddOrUpdate(Person person);
        void Delete(Person person);
    }
}
