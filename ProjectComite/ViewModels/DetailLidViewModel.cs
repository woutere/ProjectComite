﻿using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectComite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectComite.ViewModels
{
    public class DetailLidViewModel
    {
        public Lid lid { get; set; }
        public SelectList acties { get; set; }
    }
}
