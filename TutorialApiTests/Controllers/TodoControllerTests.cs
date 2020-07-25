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

namespace TutorialApiTests
{
    public class TodoControllerTests
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
            return items;
        }

        [Fact]
        public async Task GetTodoItems_ReturnsInvalidException_WithEmptyList()
        {
            var items = new List<TodoItem>();
            var mockRepository = new Mock<ITodoItemRepository>();
            mockRepository.Setup(repo => repo.GetItemsListAsync())
                .ReturnsAsync(items);
            var controller = new TodoController(mockRepository.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => 
                await controller.GetTodoItems());
        }

        [Fact]
        public async Task GetTodoItems_ReturnsResultValue_WithListOfTodoItems()
        {
            var mockRepository = new Mock<ITodoItemRepository>();
            mockRepository.Setup(repo => repo.GetItemsListAsync())
                .ReturnsAsync(GetTestItems());
            var controller = new TodoController(mockRepository.Object);

            var result = await controller.GetTodoItems();

            var items = Assert.IsType<List<TodoItem>>(result.Value);
            Assert.Equal(2, result.Value.Count);
            var firstItem = items.Find(item => item.Id == 1);
            Assert.Equal("Test One", firstItem.Name);
        }

        [Fact]
        public async Task GetTodoItem_ReturnsNotFound_WhenTodoItemIsNull()
        {
            long testId = 1;
            var mockRepository = new Mock<ITodoItemRepository>();
            mockRepository.Setup(repo => repo.GetItemByIdAsync(testId))
                .ReturnsAsync((TodoItem)null);
            var controller = new TodoController(mockRepository.Object);

            var result = await controller.GetTodoItem(testId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetTodoItem_ReturnsResultValue_WithListOfTodoItems()
        {
            long testId = 1;
            var mockRepository = new Mock<ITodoItemRepository>();
            mockRepository.Setup(repo => repo.GetItemByIdAsync(testId))
                .ReturnsAsync(GetTestItems().FirstOrDefault(td => td.Id == testId));
            var controller = new TodoController(mockRepository.Object);

            var result = await controller.GetTodoItem(testId);

            Assert.IsType<TodoItem>(result.Value);
            Assert.Equal(testId, result.Value.Id);
            Assert.Equal("Test One", result.Value.Name);
            Assert.True(result.Value.IsComplete);
        }

        [Fact]
        public async Task PostTodoItem_ReturnsCreatedAtAction()
        {
            string testName = "test name";
            var mockRepository = new Mock<ITodoItemRepository>();
            mockRepository.Setup(repo => repo.PostItemAsync(It.IsAny<TodoItem>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            var controller = new TodoController(mockRepository.Object);
            var newTodoItem = new TodoItem
            {
                Name = testName,
                IsComplete = false
            };

            var result = await controller.PostTodoItem(newTodoItem);

            var actionResult = Assert.IsType<ActionResult<TodoItem>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<TodoItem>(createdAtActionResult.Value);
            mockRepository.Verify();
            Assert.Equal(testName, returnValue.Name);
            Assert.False(returnValue.IsComplete);
        }

        [Fact]
        public async Task PutTodoItem_ReturnsBadRequest_WhenIdIsNotItemId()
        {
            long testId = 2;
            long testItemId = 1;
            string testName = "test name";
            var mockRepository = new Mock<ITodoItemRepository>();
            var controller = new TodoController(mockRepository.Object);
            var newTodoItem = new TodoItem
            {
                Id = testItemId,
                Name = testName,
                IsComplete = true
            };

            var result = await controller.PutTodoItem(testId, newTodoItem);

            Assert.IsType<BadRequestResult>(result);
            mockRepository.Verify();
        }

        [Fact]
        public async Task PutTodoItem_ReturnsNoContent()
        {
            long testId = 1;
            string testName = "test name";
            var mockRepository = new Mock<ITodoItemRepository>();
            mockRepository.Setup(repo => repo.PutItemAsync(It.IsAny<TodoItem>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            var controller = new TodoController(mockRepository.Object);
            var newTodoItem = new TodoItem
            {
                Id = testId,
                Name = testName,
                IsComplete = true
            };

            var result = await controller.PutTodoItem(testId, newTodoItem);

            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<NoContentResult>(result);
            mockRepository.Verify();
        }

        [Fact]
        public async Task DeleteTodoItem_ReturnsNotFound_WhenTodoItemIsNull()
        {
            long testId = 5;
            var mockRepository = new Mock<ITodoItemRepository>();
            mockRepository.Setup(repo => repo.GetItemByIdAsync(testId))
                .ReturnsAsync((TodoItem)null)
                .Verifiable();
            var controller = new TodoController(mockRepository.Object);

            var result = await controller.DeleteTodoItem(testId);

            Assert.IsType<NotFoundResult>(result);
            mockRepository.Verify();
        }

        [Fact]
        public async Task DeleteTodoItem_ReturnsNoContent()
        {
            long testId = 1;
            var testTodoItem = new TodoItem
            {
                Id = testId,
                Name = "test name",
                IsComplete = true
            };
            var mockRepository = new Mock<ITodoItemRepository>();
            mockRepository.Setup(repo => repo.GetItemByIdAsync(testId))
                .ReturnsAsync(testTodoItem)
                .Verifiable();
            var controller = new TodoController(mockRepository.Object);

            var result = await controller.DeleteTodoItem(testId);

            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<NoContentResult>(result);
            mockRepository.Verify();
        }
    }
}
