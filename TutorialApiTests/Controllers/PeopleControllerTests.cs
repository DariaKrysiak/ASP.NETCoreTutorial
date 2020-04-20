using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TutorialApi.Controllers;
using TutorialApi.Models;
using TutorialApi.Repositories;
using Xunit;

namespace TutorialApiTests.Controllers
{
    public class PeopleControllerTests
    {
        private List<Person> GetTestPeople()
        {
            var people = new List<Person>();
            people.Add(new Person()
            {
                Id = 1,
                FirstName = "Test Name One"
            });
            people.Add(new Person()
            {
                Id = 2,
                FirstName = "Test Name Two"
            });
            return people;
        }

        [Fact]
        public async Task GetPeople_ReturnsInvalidException_WithEmptyList()
        {
            var people = new List<Person>();
            var mockRepository = new Mock<IPersonRepository>();
            mockRepository.Setup(repo => repo.GetPeopleListAsync())
                .ReturnsAsync(people);
            var controller = new PeopleController(mockRepository.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await controller.GetPeople());
        }

        [Fact]
        public async Task GetPeople_ReturnsResultValue_WithListOfPeople()
        {
            var mockRepository = new Mock<IPersonRepository>();
            mockRepository.Setup(repo => repo.GetPeopleListAsync())
                .ReturnsAsync(GetTestPeople());
            var controller = new PeopleController(mockRepository.Object);

            var result = await controller.GetPeople();

            var people = Assert.IsType<List<Person>>(result.Value);
            Assert.Equal(2, result.Value.Count);
            var firstPerson = people.Find(p => p.Id == 1);
            Assert.Equal("Test Name One", firstPerson.FirstName);
        }

        [Fact]
        public async Task GetPerson_ReturnsNotFound_WhenPersonIsNull()
        {
            long testId = 1;
            var mockRepository = new Mock<IPersonRepository>();
            mockRepository.Setup(repo => repo.GetPersonByIdAsync(testId))
                .ReturnsAsync((Person)null);
            var controller = new PeopleController(mockRepository.Object);

            var result = await controller.GetPerson(testId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetPerson_ReturnsResultValue_WithListOfPeople()
        {
            long testId = 1;
            var mockRepository = new Mock<IPersonRepository>();
            mockRepository.Setup(repo => repo.GetPersonByIdAsync(testId))
                .ReturnsAsync(GetTestPeople().FirstOrDefault(p => p.Id == testId));
            var controller = new PeopleController(mockRepository.Object);

            var result = await controller.GetPerson(testId);

            Assert.IsType<Person>(result.Value);
            Assert.Equal(testId, result.Value.Id);
            Assert.Equal("Test Name One", result.Value.FirstName);
        }

        [Fact]
        public async Task PostPerson_ReturnsCreatedAtAction()
        {
            string testFirstName = "test first name";
            var mockRepository = new Mock<IPersonRepository>();
            mockRepository.Setup(repo => repo.PostPersonAsync(It.IsAny<Person>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            var controller = new PeopleController(mockRepository.Object);
            var newPerson = new Person { FirstName = testFirstName };

            var result = await controller.PostPerson(newPerson);

            var actionResult = Assert.IsType<ActionResult<Person>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Person>(createdAtActionResult.Value);
            mockRepository.Verify();
            Assert.Equal(testFirstName, returnValue.FirstName);
        }
    }
}
