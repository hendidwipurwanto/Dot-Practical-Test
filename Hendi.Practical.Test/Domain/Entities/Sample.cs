using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Sample  // this is just sample classs to test the code first initialize the DB
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
