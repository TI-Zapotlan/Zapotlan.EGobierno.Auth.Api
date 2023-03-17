using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zapotlan.EGobierno.Auth.Core.Entities;
using Zapotlan.EGobierno.Auth.Core.Interfaces;

namespace Zapotlan.EGobierno.Auth.Core.Services
{
    public class PersonaService : IPersonaService
    {
        private readonly IUnitOfWork _unitOfWork;

        // CONSTRUCTOR

        public PersonaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // METHODS 

        public IEnumerable<Persona> Gets()
        { 
            return _unitOfWork.PersonaRepository.Gets();
        }

        public async Task<Persona?> GetAsync(Guid id) 
        {
            return await _unitOfWork.PersonaRepository.GetAsync(id);
        }
    }
}
