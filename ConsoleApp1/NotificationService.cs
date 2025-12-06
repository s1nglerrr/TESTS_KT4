using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public interface INotificationService
    {
        void SendSuccessNotification(string studentName, string bookTitle);
        void SendWarningNotification(string message);
    }
}
