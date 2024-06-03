using Moq;
using Microsoft.AspNetCore.Mvc;
using LibraryApplicationAPI.Interfaces;
using LibraryApplicationAPI.Controllers;
using LibraryApplicationAPI.Models;

namespace LibraryTestApplication
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //}

        private Mock<IBookRepository> _mockRepo;
        private LibraryManagment _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepo = new Mock<IBookRepository>();
            _controller = new LibraryManagment(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetBooks_ReturnsOkResult_WithListOfBooks()
        {
            // Arrange
            var books = new List<Book> { new Book { Id = 1, Title = "Test Book", Author = "any" , CopiesAvailable = 2 } };
            _mockRepo.Setup(repo => repo.GetAllBooksAsync()).ReturnsAsync(books);

            // Act
            var result = await _controller.GetBooks();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnBooks = okResult.Value as IEnumerable<Book>;
            Assert.AreEqual(books, returnBooks);
        }

        [TestMethod]
        public async Task GetBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetBookByIdAsync(It.IsAny<int>())).ReturnsAsync((Book)null);

            // Act
            var result = await _controller.GetBook(1);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetBook_ReturnsOkResult_WithBook()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test Book" };
            _mockRepo.Setup(repo => repo.GetBookByIdAsync(It.IsAny<int>())).ReturnsAsync(book);

            // Act
            var result = await _controller.GetBook(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnBook = okResult.Value as Book;
            Assert.AreEqual(book, returnBook);
        }

        [TestMethod]
        public async Task AddBook_ReturnsCreatedAtAction_WithBook()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test Book" };
            _mockRepo.Setup(repo => repo.AddBookAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddBook(book);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(nameof(_controller.GetBook), createdAtActionResult.ActionName);
            var returnBook = createdAtActionResult.Value as Book;
            Assert.AreEqual(book, returnBook);
        }

        [TestMethod]
        public async Task DeleteBook_ReturnsOkResult_WhenBookIsDeleted()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteBookAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteBook(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Book Deleted", okResult.Value);
        }

        [TestMethod]
        public async Task LendBook_ReturnsBadRequest_WhenBookNotAvailable()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.LendBookAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _controller.LendBook(1);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Book not available.", badRequestResult.Value);
        }

        [TestMethod]
        public async Task LendBook_ReturnsOkResult_WhenBookIsLended()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.LendBookAsync(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _controller.LendBook(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Book lended", okResult.Value);
        }

        [TestMethod]
        public async Task ReturnBook_ReturnsBadRequest_WhenBookNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.ReturnBookAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _controller.ReturnBook(1);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Book not found.", badRequestResult.Value);
        }

        [TestMethod]
        public async Task ReturnBook_ReturnsOkResult_WhenBookIsReturned()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.ReturnBookAsync(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _controller.ReturnBook(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Book Returned", okResult.Value);
        }
    
    }
}