using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectComite.Models
{
    public class Lid
    {
        public int lidId { get; set; }
        public string naam { get; set; }
        [ForeignKey("Gemeente")]
        public int gemeenteId { get; set; }
        public Gemeente gemeente { get; set; }
        public ICollection<ActieLid> acties { get; set; }
        public bool lidgeldBetaald { get; set; }
        public string emailAdres { get; set; }
        public string telefoonnummer { get; set; }
    }
}
