using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapotlan.EGobierno.Auth.Core.Enumerations
{
    public enum PersonaEstadoVidaTipo
    {
        Ninguno,
        Vivo,
        Fallecido,
        Desconocido
    }

    public enum PersonaOrdenFilterTipo
    { 
        Ninguno,
        Nombre,
        PrimerApellido,
        EstadoVida,
        NombreDesc,
        PrimerApellidoDesc
    }
}
