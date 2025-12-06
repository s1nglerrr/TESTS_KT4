using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class LibraryService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly INotificationService _notificationService;

        public LibraryService(
            IBookRepository bookRepository,
            IStudentRepository studentRepository,
            INotificationService notificationService)
        {
            _bookRepository = bookRepository;
            _studentRepository = studentRepository;
            _notificationService = notificationService;
        }

        public BorrowResult BorrowBook(int studentId, int bookId)
        {
            Student student = _studentRepository.GetStudentById(studentId);
            if (student == null)
            {
                return new BorrowResult
                {
                    Success = false,
                    Message = "Студент не найден"
                };
            }

            Book book = _bookRepository.GetBookById(bookId);
            if (book == null)
            {
                return new BorrowResult
                {
                    Success = false,
                    Message = "Книга не найдена"
                };
            }

            if (!book.IsAvailable)
            {
                return new BorrowResult
                {
                    Success = false,
                    Message = "Книга уже выдана"
                };
            }

            book.IsAvailable = false;
            _bookRepository.UpdateBook(book);

            _notificationService.SendSuccessNotification(student.Name, book.Title);

            return new BorrowResult
            {
                Success = true,
                Message = $"Книга '{book.Title}' успешно выдана студенту {student.Name}"
            };
        }
    }
}
