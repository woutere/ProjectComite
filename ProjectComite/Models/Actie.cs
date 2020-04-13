using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectComite.Models
{
    public class Actie
    {
        public int actieId { get; set; }
        public string naam { get; set; }
        public string informatie { get; set; }
        [ForeignKey("Gemeente")]
        public int gemeenteId { get; set; }
        public Gemeente gemeente { get; set; }
        public ICollection<ActieLid> leden{get;set;}

        [NotMapped]
        public bool CheckboxAnswer { get; set; }
    }
}
