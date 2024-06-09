using AutoMapper;
using Stock2u.DTO;
using Stock2u.Models;

namespace Stock2u.Mappings
{
    public class EntitiesToDTOMappingProfile : Profile
    {
        public EntitiesToDTOMappingProfile()
        {
            CreateMap<Produto,ProdutoPost>().ReverseMap();
            CreateMap<Produto, ProdutosGet>().ReverseMap();
            CreateMap<EstoqueRestaurante, EstoqueRestaurantePost>().ReverseMap();
            CreateMap<EstoqueRestaurante, EstoqueRestauranteGet>().ReverseMap();
        }
    }
}
