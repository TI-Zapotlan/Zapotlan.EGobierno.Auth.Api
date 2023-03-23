using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapotlan.EGobierno.Auth.Core.Enumerations
{
    public enum UsuarioEstatusTipo
    {
        Ninguno,
        Activo,
        Baja,
        Eliminado
    }

    public enum UsuarioRolTipo
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

    public enum UsuarioOrdenFilterTipo
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
