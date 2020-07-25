using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TutorialApi.Controllers;
using TutorialApi.Models;
using TutorialApi.Repositories;
using TutorialApi.WebModels;
using Xunit;

namespace TutorialApiTests.Controllers
{
    public class TodoWithPersonsNamesControllerTests
    {
        private List<TodoItem> GetTestItems()
        {
            var items = new List<TodoItem>();
            items.Add(new TodoItem()
            {
                Id = 1,
                Name = "Test One",
                IsComplete = true
            });
            items.Add(new TodoItem()
            {
                Id = 2,
                Name = "Test Two",
                IsComplete = false
            });
            items.Add(new TodoItem()
            {
                Id = 3,
                Name = "Test Three",
                IsComplete = false
            });
            return items;
        }

        [Fact]
        public async Task GetTodoItemsWithPersonsNames_ReturnsInvalidException_WithEmptyList()
        {
            var items = new List<TodoItem>();
            var mockRepositoryItem = new Mock<ITodoItemRepository>();
            var mockRepositoryPerson = new Mock<IPersonRepository>();
            mockRepositoryItem.Setup(repo => repo.GetItemsListAsync())
                .ReturnsAsync(items);
            var controller = new TodoWithPersonsNamesController(
                mockRepositoryItem.Object, mockRepositoryPerson.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await controller.GetTodoItemsWithPersonsNames());
        }

        [Fact]
        public async Task GetTodoItemsWithPersonsNames_ReturnsOkObjectResult_WithListOfTodoItemsWithPersonsNames()
        {
            var mockRepositoryItem = new Mock<ITodoItemRepository>();
            var mockRepositoryPerson = new Mock<IPersonRepository>();
            mockRepositoryItem.Setup(repo => repo.GetItemsListAsync())
                .ReturnsAsync(GetTestItems())
                .Verifiable();
            mockRepositoryPerson.Setup(repo => repo.GetPersonName(It.IsAny<TodoItem>()))
                .ReturnsAsync("test")
                .Verifiable();
            var controller = new TodoWithPersonsNamesController(
                mockRepositoryItem.Object, mockRepositoryPerson.Object);

            var result = await controller.GetTodoItemsWithPersonsNames();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<TodoItemWithPersonsName>>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<TodoItemWithPersonsName>>(okObjectResult.Value);
            mockRepositoryItem.Verify();
            mockRepositoryPerson.Verify();
            Assert.Equal(3, returnValue.Count);
            Assert.Equal("test", returnValue.Find(item => item.TodoItemName == "Test One").PersonName);
        }
    }
}
