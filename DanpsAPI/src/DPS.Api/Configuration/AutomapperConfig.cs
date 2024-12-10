using AutoMapper;
using DPS.Api.Dtos;
using DPS.Business.Models;

namespace DPS.Api.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Fornecedor, FornecedorDto>().ReverseMap();
            CreateMap<Endereco, EnderecoDto>().ReverseMap();
            CreateMap<ProdutoDto, Produto>();

            CreateMap<ProdutoImagemDto, Produto>().ReverseMap();

            CreateMap<Produto, ProdutoDto>()
                .ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome));
        }
    }
}