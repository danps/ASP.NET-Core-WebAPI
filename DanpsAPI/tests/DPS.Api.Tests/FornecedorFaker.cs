using Bogus;
using Bogus.Extensions.Brazil;
using DPS.Api.Dtos;

namespace DPS.Api.Tests
{
    public class FornecedorFaker
    {
        public static List<FornecedorDto> GerarFornecedoresProdutos(int quantidade)
        {
            var enderecoFaker = new Faker<EnderecoDto>("pt_BR")
                .RuleFor(e => e.Id, f => Guid.NewGuid())
                .RuleFor(e => e.Logradouro, f => f.Address.StreetAddress())
                .RuleFor(e => e.Numero, f => f.Address.BuildingNumber())
                .RuleFor(e => e.Complemento, f => f.Address.SecondaryAddress())
                .RuleFor(e => e.Bairro, f => f.Address.City())
                .RuleFor(e => e.Cep, f => f.Address.ZipCode("########"))
                .RuleFor(e => e.Cidade, f => f.Address.City())
                .RuleFor(e => e.Estado, f => f.Address.State());

            var produtoFaker = new Faker<ProdutoDto>("pt_BR")
                .RuleFor(p => p.Id, f => Guid.NewGuid())
                .RuleFor(p => p.Nome, f => f.Commerce.ProductName())
                .RuleFor(p => p.Descricao, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Valor, f => f.Random.Decimal(0, 1000));

            var fornecedorFaker = new Faker<FornecedorDto>("pt_BR")
                .RuleFor(f => f.Id, f => Guid.NewGuid())
                .RuleFor(f => f.Nome, f => f.Company.CompanyName())
                .RuleFor(f => f.Documento, f => f.Person.Cpf(false))
                .RuleFor(f => f.TipoFornecedor, f => 1)
                .RuleFor(f => f.Endereco, f => enderecoFaker.Generate())
                .RuleFor(f => f.Ativo, f => f.Random.Bool())
                .RuleFor(f => f.Produtos, f => produtoFaker.Generate(f.Random.Int(1, 5)));

            return fornecedorFaker.Generate(quantidade);
        }

        public static List<FornecedorDto> GerarFornecedores(int quantidade)
        {
            var enderecoFaker = new Faker<EnderecoDto>("pt_BR")
                .RuleFor(e => e.Id, f => Guid.NewGuid())
                .RuleFor(e => e.Logradouro, f => f.Address.StreetAddress())
                .RuleFor(e => e.Numero, f => f.Address.BuildingNumber())
                .RuleFor(e => e.Complemento, f => f.Address.SecondaryAddress())
                .RuleFor(e => e.Bairro, f => f.Address.City())
                .RuleFor(e => e.Cep, f => f.Address.ZipCode("########"))
                .RuleFor(e => e.Cidade, f => f.Address.City())
                .RuleFor(e => e.Estado, f => f.Address.State());
            var fornecedorFaker = new Faker<FornecedorDto>("pt_BR")
                .RuleFor(f => f.Id, f => Guid.NewGuid())
                .RuleFor(f => f.Nome, f => f.Company.CompanyName())
                .RuleFor(f => f.Documento, f => f.Person.Cpf(false))
                .RuleFor(f => f.TipoFornecedor, f => 1)
                .RuleFor(f => f.Endereco, f => enderecoFaker.Generate())
                .RuleFor(f => f.Ativo, f => f.Random.Bool());

            return fornecedorFaker.Generate(quantidade);
        }

    }
}