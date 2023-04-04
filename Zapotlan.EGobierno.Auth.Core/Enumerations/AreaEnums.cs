using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapotlan.EGobierno.Auth.Core.Enumerations
{
    public enum AreaType
    {
        Ninguno,
        Presidencia,
        Regidores,
        Direccion,
        Sub_Direccion,
        Departamento,
        Jefatura,
        Coordinacion,
        Secretaria,
        Sub_Jefatura
    }

    public enum AreaEstatusType
    { 
        Ninguno,
        Activo,
        Inactivo,
        Eliminado
    }

    public enum AreaOrderFilterType
    { 
        Ninguno,
        Clave,
        Nombre,
        FechaActualizacion,
        ClaveDesc,
        NombreDesc,
        FechaActualizacionDesc
    }
}
