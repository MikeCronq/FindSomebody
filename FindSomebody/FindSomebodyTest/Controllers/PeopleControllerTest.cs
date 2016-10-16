using FindSomebody.Models;
using FindSomebody.ViewModels;
using FindSomebodyTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace FindSomebody.Controllers.Tests
{
    [TestClass()]
    public class PeopleControllerTest
    {
        private PeopleController _controller;
        private Person[] _people;

        private Mock<PeopleDbContext> _dbMock;
        private ContextMocks _contextMocks;

        [TestInitialize]
        public void Initialize()
        {
            _people = new Person[5];

            for (int i = 0; i < _people.Length; ++i)
            {
                _people[i] = new Person()
                {
                    ID = i,
                    Name = i.ToString(),
                    Email = i + "@website.com",
                    Address = i + " street name",
                    Age = i,
                    Interests = "blah"
                };
            }

            var mockDbSet = EntityFrameworkMockFactory.CreateMockDbSet(_people.AsQueryable());
            mockDbSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => _people.FirstOrDefault(x => x.ID == (int)ids[0]));

            _dbMock = new Mock<PeopleDbContext>();
            _dbMock.Setup(x => x.People).Returns(mockDbSet.Object);

            _controller = new PeopleController(_dbMock.Object);

            _contextMocks = new ContextMocks(_controller);
        }

        [TestMethod()]
        public void IndexTest()
        {
            //IsAjaxRequest == false;
            _contextMocks.RequestMock.Setup(x => x.Headers).Returns(new WebHeaderCollection() {
                {"X-Requested-With", "HttpRequest"}
            });

            var result = _controller.Index(string.Empty) as ViewResult;
            var people = (IEnumerable<Person>)result.ViewData.Model;
            Assert.AreEqual(_people.Count(), people.Count());
        }

        [TestMethod()]
        public void IndexAjaxSearchTest()
        {
            //IsAjaxRequest == false;
            _contextMocks.RequestMock.Setup(x => x.Headers).Returns(new WebHeaderCollection() {
                {"X-Requested-With", "XMLHttpRequest"}
            });

            var result = _controller.Index("1") as PartialViewResult;
            var people = (IEnumerable<Person>)result.ViewData.Model;
            Assert.AreEqual(1, people.Count());
        }

        [TestMethod()]
        public void AutocompleteTest()
        {
            var result = _controller.Autocomplete("1") as JsonResult;
            var people = (IQueryable<string>)result.Data;
            Assert.AreEqual(_people[1].Name, people.First());
        }

        [TestMethod()]
        public void DetailsTest()
        {
            var result = _controller.Details(1) as ViewResult;
            var person = (Person)result.ViewData.Model;
            Assert.AreEqual(_people[1], person);
        }

        [TestMethod()]
        public void CreateTest()
        {
            var viewModel = new EditPersonViewModel();

            var result = _controller.Create(viewModel) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod()]
        public void EditTest()
        {
            var viewModel = new EditPersonViewModel();

            _dbMock.Setup(x => x.SetModified(It.IsAny<object>())).Verifiable();

            var result = _controller.Edit(viewModel) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            _dbMock.Verify();
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            _controller.Delete(1);
        }

        [TestMethod()]
        public void DeleteConfirmedTest()
        {
            Assert.Fail();
        }
    }
}