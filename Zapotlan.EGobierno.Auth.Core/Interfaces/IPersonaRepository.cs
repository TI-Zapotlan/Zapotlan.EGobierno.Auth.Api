﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Core.Interfaces
{
    public interface IPersonaRepository
    {
        Task<IEnumerable<Persona>> Gets();
        Task<Persona> Get(Guid id);

    }
}