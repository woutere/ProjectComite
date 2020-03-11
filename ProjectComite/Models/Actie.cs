﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectComite.Models
{
    public class Actie
    {
        public int actieId { get; set; }
        public string informatie { get; set; }
        [ForeignKey("Gemeente")]
        public int GemeenteId { get; set; }
        public Gemeente gemeente { get; set; }
        public ICollection<Lid> leden{get;set;}
    }
}