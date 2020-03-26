using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectComite.Models
{
    public class Gemeente
    {
        public int gemeenteId { get; set; }
        public string naam { get; set; }
        public string postcode { get; set; }
        public ICollection<Lid> leden { get; set; }
        public ICollection<ActieGemeente> acties { get; set; }
        [NotMapped]
        public bool CheckboxAnswer { get; set; }
        //public override string ToString()
        //{
        //    return naam;
        //}
    }
}
