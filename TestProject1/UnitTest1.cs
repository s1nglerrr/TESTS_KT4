using System;
using Xunit;
using Moq;
using ConsoleApp1;

namespace TestProject1
{
    public class LibraryServiceTests
    {
        private readonly Mock<IStudentRepository> _studentRepoMock;
        private readonly Mock<IBookRepository> _bookRepoMock;
        private readonly Mock<INotificationService> _notificationMock;
        private readonly LibraryService _service;
        private readonly Student _student;
        private readonly Book _availableBook;
        private readonly Book _unavailableBook;

        public LibraryServiceTests()
        {
            _studentRepoMock = new Mock<IStudentRepository>();
            _bookRepoMock = new Mock<IBookRepository>();
            _notificationMock = new Mock<INotificationService>();
            _service = new LibraryService(_bookRepoMock.Object, _studentRepoMock.Object, _notificationMock.Object);

            _student = new Student { Id = 1, Name = "Иван Иванов", Grade = 5 };
            _availableBook = new Book { Id = 1, Title = "C#", Author = "MC.TIMUR3XXX", IsAvailable = true };
            _unavailableBook = new Book { Id = 1, Title = "Книга", Author = "Автор", IsAvailable = false };

            _studentRepoMock.Setup(x => x.GetStudentById(1)).Returns(_student);
            _bookRepoMock.Setup(x => x.GetBookById(1)).Returns(_availableBook);
        }

        [Fact]
        public void BorrowBook_StudentNotFound_ReturnsUnsuccessfulResult()
        {
            _studentRepoMock.Setup(x => x.GetStudentById(It.IsAny<int>())).Returns((Student)null);

            var result = _service.BorrowBook(1, 1);

            Assert.False(result.Success);
            Assert.Equal("Студент не найден", result.Message);
        }

        [Fact]
        public void BorrowBook_BookNotFound_ReturnsUnsuccessfulResult()
        {
            _bookRepoMock.Setup(x => x.GetBookById(It.IsAny<int>())).Returns((Book)null);

            var result = _service.BorrowBook(1, 1);

            Assert.False(result.Success);
            Assert.Equal("Книга не найдена", result.Message);
        }

        [Fact]
        public void BorrowBook_BookNotAvailable_ReturnsUnsuccessfulResult()
        {
            _bookRepoMock.Setup(x => x.GetBookById(1)).Returns(_unavailableBook);

            var result = _service.BorrowBook(1, 1);

            Assert.False(result.Success);
            Assert.Equal("Книга уже выдана", result.Message);
        }

        [Fact]
        public void BorrowBook_SuccessfulBorrow_ReturnsSuccessResult()
        {
            var result = _service.BorrowBook(1, 1);

            Assert.True(result.Success);
            Assert.Equal("Книга 'C#' успешно выдана студенту Иван Иванов", result.Message);
            Assert.False(_availableBook.IsAvailable);
            _bookRepoMock.Verify(x => x.UpdateBook(_availableBook), Times.Once);
            _notificationMock.Verify(x => x.SendSuccessNotification("Иван Иванов", "C#"), Times.Once);
        }
    }
}
