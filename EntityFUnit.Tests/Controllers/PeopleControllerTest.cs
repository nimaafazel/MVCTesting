using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityFUnit.Controllers;
using EntityFUnit.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System;
using System.Net;

namespace EntityFUnit.Tests.Controllers
{
    [TestClass]
    public class PeopleControllerTest
    {
        [TestMethod]
        public void IndexShouldReturnAViewResultWhenListNotEmpty()
        {
            // Arrange
            // create a fixed list of Persons
            List<Person> per = new List<Person>();
            per.Add(new Person { id = 1, name = "Dude", lastName = "Hey!", age = 23 });
            per.Add(new Person { id = 2, name = "Jonathan", lastName = "Bryce", age = 12 });
            per.Add(new Person { id = 3, name = "Michael", lastName = "Fox", age = 31 });

            // Stub the IPersonRepository
            var personRepo = new EntityFUnit.Models.Fakes.StubIPersonRepository();

            // Implement the persons get method
            personRepo.PersonsGet = () => per.AsQueryable<Person>();               
            
            // create a controller with the Dependency Injection Constructor
            var controller = new PeopleController(personRepo);

            // Act
            // call the Index method
            var result = controller.Index();

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void IndexShouldReturnAViewResultWhenListEmpty()
        {
            // Arrange
            // create a fixed list of Persons
            List<Person> per = new List<Person>();
            
            // Stub the IPersonRepository
            var personRepo = new EntityFUnit.Models.Fakes.StubIPersonRepository();

            // Implement the persons get method
            personRepo.PersonsGet = () => per.AsQueryable<Person>();

            // create a controller with the Dependency Injection Constructor
            var controller = new PeopleController(personRepo);

            // Act
            // call the Index method
            var result = controller.Index();

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void IndexShouldRedirectToHomeWhenListIsNull()
        {
            // Arrange
            //List<Person> per = null;

            var personRepo = new Models.Fakes.StubIPersonRepository();  // create the stub
            personRepo.PersonsGet = () => null;  // implement get persons

            var controller = new PeopleController(personRepo);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual(((RedirectResult)result).Url, "Home");
        }

        [TestMethod]
        public void DetailsShouldReturnBadRequestErrorWhenIdIsNull()
        {
            // Arrange
            var personRepo = new Models.Fakes.StubIPersonRepository();
            personRepo.PersonsGet = () => new List<Person>().AsQueryable<Person>();

            var controller = new PeopleController(personRepo);

            // act
            var result = controller.Details(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
            Assert.AreEqual(((HttpStatusCodeResult)result).StatusCode, 400);
        }

        [TestMethod]
        public void DetailsShouldReturnNotFoundErrorWhenPersonIsNotFound()
        {
            // Arrange
            // populate a list to simulate database with records, to try to find an non-existent id.
            // Since the interface IPersonRepository uses an IQueryable, we create one from a list
            IQueryable<Person> per = new List<Person>()
            {
                new Person { id = 1, name = "Dude", lastName = "Hey!", age = 23 },
                new Person { id = 2, name = "Jonathan", lastName = "Bryce", age = 12 },
                new Person { id = 3, name = "Michael", lastName = "Fox", age = 31 }
            }.AsQueryable();
            
            var personRepo = new Models.Fakes.StubIPersonRepository();
            personRepo.PersonsGet = () => per;

            var controller = new PeopleController(personRepo);

            // act
            var result = controller.Details(9);  // since the id 9 is not in our list, it will test this behavior

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));            
        }

        [TestMethod]
        public void CreateShouldReturnToIndexAfterSuccesfulInsert()
        {
            // Arrange
            Person person = new Person() { id = 10, name = "test", lastName = "test", age = 1 };
            var persons = new List<Person>().AsQueryable();
            
            var personRepo = new Models.Fakes.StubIPersonRepository();
            personRepo.PersonsGet = () => persons;

            var controller = new PeopleController(personRepo);             

            // Act
            var result = controller.Create(person);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual(((RedirectResult)result).Url, "Index");
        }

        [TestMethod]
        public void EditShouldGoToEditOnValidId()
        {
            // Arrange
            var persons = new List<Person>()
            {
                new Person() { id=1, name="nima", lastName="afazel" }
            }.AsQueryable();

            var personRepo = new Models.Fakes.StubIPersonRepository();
            personRepo.PersonsGet = () => persons;

            var controller = new PeopleController(personRepo);
            // Act
            var result = controller.Edit(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void EditShouldReturnNotFoundPageOnInexistentId()
        {
            // Arrange
            var persons = new List<Person>()
            {
                new Person() { id=1, name="hello", lastName="world" }
            }.AsQueryable();
            var personRepo = new Models.Fakes.StubIPersonRepository();
            personRepo.PersonsGet = () => persons;
            var controller = new PeopleController(personRepo);

            // Act
            var result = controller.Edit(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
        }

        [TestMethod]
        public void EditShouldReturnToIndexIfValidId()
        {
            // Arrange
            var persons = new List<Person>()
            {
                new Person() { id=1, name="Nima", lastName="Afazel" }
            }.AsQueryable();
            var per = new Person() { id = 1, name = "Nima", lastName = "Afazel", age = 28 }; // person edited
            var personRepo = new Models.Fakes.StubIPersonRepository();
            personRepo.PersonsGet = () => persons;

            var controller = new PeopleController(personRepo);
            
            // Act
            var result = controller.Edit(per);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void DeleteShouldShowPageOnValidId()
        {
            // Arrange
            var persons = new List<Person>()
            {
                new Person() { id=1, name="test", lastName="cool" }
            }.AsQueryable();
            var personRepo = new Models.Fakes.StubIPersonRepository();
            personRepo.PersonsGet = () => persons;
            var controller = new PeopleController(personRepo);

            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void DeleteShouldShowBadRequestOnIdNull()
        {
            // Arrange
            var personRepo = new Models.Fakes.StubIPersonRepository();
            personRepo.PersonsGet = () => new List<Person>().AsQueryable();

            var controller = new PeopleController(personRepo);

            // Act
            var result = controller.Delete(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
            Assert.AreEqual(((HttpStatusCodeResult)result).StatusCode, (int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void DeleteShouldShowNotFoundPageIfPersonIsNotFound()
        {
            // Arrange
            var personRepo = new Models.Fakes.StubIPersonRepository();
            personRepo.PersonsGet = () => new List<Person>().AsQueryable();

            var controller = new PeopleController(personRepo);

            // Act
            var result = controller.Delete(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        public void DeleteShouldReturnToIndexAfterSuccesfulDeletion()
        {
            // Arrange
            var personRepo = new Models.Fakes.StubIPersonRepository();
            personRepo.PersonsGet = () => new List<Person>
            {
                new Person() { id = 1, name = "Hola", lastName = "hello" }
            }.AsQueryable();
            var controller = new PeopleController(personRepo);

            // Act
            var result = controller.DeleteConfirmed(1);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }
    }
}
