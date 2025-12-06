using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public interface IBookRepository
    {
        Book GetBookById(int id);
        void UpdateBook(Book book);
    }
}
