﻿using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectComite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectComite.ViewModels
{
    public class CreateLidViewModel
    {
        public Lid lid { get; set; }
        public SelectList gemeentes { get; set; }
        public List<Actie> acties { get; set; }
    }
}
