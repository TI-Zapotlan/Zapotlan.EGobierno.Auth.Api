﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Area> AreaRepository { get; }
        IRepository<Persona> PersonaRepository { get; }

        IDerechoRepository DerechoRepository { get;  }
        IGrupoRepository GrupoRepository { get; }
        IUsuarioRepository UsuarioRepository { get; }

        void SaveChanges();

        Task SaveChangesAsync();
    }
}
