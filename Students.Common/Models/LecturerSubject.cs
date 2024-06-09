using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Common.Models
{
    public class LecturerSubject
    {
        public int LecturerId { get; set; }

        public required Lecturer Lecturer { get; set; }


        public int SubjectId { get; set; }

        public required Subject Subject { get; set; }

    }
}
