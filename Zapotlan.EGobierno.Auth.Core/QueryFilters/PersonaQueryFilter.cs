﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Enumerations;

namespace Zapotlan.EGobierno.Auth.Core.QueryFilters
{
    public class PersonaQueryFilter : DefaultQueryFilter
    {
        public string? Texto { get; set; }
        public PersonaEstadoVidaType? EstadoVida { get; set; } = PersonaEstadoVidaType.Ninguno;

        public PersonaOrderFilterType? Orden { get; set; } = PersonaOrderFilterType.Ninguno; 
    }
}
