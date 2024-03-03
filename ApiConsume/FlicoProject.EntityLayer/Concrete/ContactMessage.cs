using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlicoProject.EntityLayer.Concrete
{
    public class ContactMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public string Answer { get; set; }
        public DateTime? MessageDate { get; set; }
        public DateTime? AnswerDate { get; set; }
        public string Status { get; set; }
    }
}
