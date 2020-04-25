using ProjectComite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectComite.ViewModels
{
    public class DetailActieViewModel
    {
        public Actie actie { get; set; }
        public List<Lid> leden { get; set; }
    }
}
