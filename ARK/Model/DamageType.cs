﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    public class DamageType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsFunctional { get; set; }
    }
}
