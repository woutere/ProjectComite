using ProjectComite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectComite.ViewModels
{
    public class DetailGemeenteViewModel
    {
        public Gemeente gemeente { get; set; }
        public List<Lid> leden { get; set; }
        public List<Actie> acties { get; set; }
    }
}
