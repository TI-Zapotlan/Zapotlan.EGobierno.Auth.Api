﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapotlan.EGobierno.Auth.Core.Enumerations
{
    public enum DerechoAccesoType
    {
        Ninguno,
        Normal,
        PermitirTodos,
        DenegarTodos
    }

    public enum DerechoOrderFilterType
    { 
        Ninguno,
        ID,
        Nombre,
        FechaActualizacion,
        IDDesc,
        NombreDesc,
        FechaActualizacionDesc
    }
}
