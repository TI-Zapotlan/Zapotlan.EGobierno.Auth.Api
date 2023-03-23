using AutoMapper;
using Zapotlan.EGobierno.Auth.Core.DTOs;
using Zapotlan.EGobierno.Auth.Core.Entities;

namespace Zapotlan.EGobierno.Auth.Api.Mappings
{
    public class AreasMapping
    {
        private readonly IMapper _mapper;

        public AreasMapping(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IEnumerable<AreaDto> AreasToListDto(IEnumerable<Area> items)
        { 
            var itemsDto = new List<AreaDto>();

            foreach(var item in items) 
            {
                var itemDto = _mapper.Map<AreaDto>(item); //new AreaDto();



                itemsDto.Add(itemDto);
            }

            return itemsDto;
        }
    }
}
