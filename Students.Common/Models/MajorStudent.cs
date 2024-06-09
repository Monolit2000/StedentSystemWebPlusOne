using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Common.Models
{
    public class MajorStudent
    {
        public int MajorId { get; set; }
        public required Major Major { get; set; }

        public int StudentId { get; set; }
        public required Student Student { get; set; }
    }
}
