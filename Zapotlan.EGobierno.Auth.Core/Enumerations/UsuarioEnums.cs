using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapotlan.EGobierno.Auth.Core.Enumerations
{
    public enum UsuarioEstatusType
    {
        Ninguno,
        Activo,
        Baja,
        Eliminado
    }

    public enum UsuarioRolType
    {
        Ninguno,
        Presidente,
        Regidor,
        Director,
        Jefe,
        Empleado,
        Coordinador,
        Asesor,
        Secretario
    }

    public enum UsuarioOrderFilterType
    { 
        Ninguno,
        Username,
        Nombre,
        FechaAlta,
        UsernameDesc,
        NombreDesc,
        FechaAltaDesc
    }
}
