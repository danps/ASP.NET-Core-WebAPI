using DPS.Api.Dtos;
using DPS.Api.Models;
using DPS.Api.V1.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace DPS.Api.Tests
{
    public class FornecedoresControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly WebApplicationFactory<Program> _factory;
        protected AppSettings? _appSettings;
        protected readonly HttpClient _client;

        public string UrlBase = "/api/v1/fornecedores";
        public Guid FornecedorIdValido = Guid.Parse("1c16e037-7aa2-42cf-b36b-74a228feebe9");

        public void AdicionarTokenAutorizacao()
        {
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, "user"),
                new Claim(ClaimTypes.Role, "Fornecedor"),
                new Claim("Fornecedor", "Adicionar"),
                new Claim("Fornecedor", "Atualizar"),
                new Claim("Fornecedor", "Excluir")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            string encodedToken = Gerador.GerarToken(_appSettings, claims);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", encodedToken);
        }

        public FornecedoresControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            SetAppSettings();
        }

        private void SetAppSettings()
        {
            var Configuration = _factory.Services.GetService<IConfiguration>();
            _appSettings = new AppSettings
            {
                Secret = Configuration["AppSettings:Secret"],
                ExpiracaoHoras = int.Parse(Configuration["AppSettings:ExpiracaoHoras"]),
                Emissor = Configuration["AppSettings:Emissor"],
                ValidoEm = Configuration["AppSettings:ValidoEm"]
            };
        }

        [Fact]
        public async Task ObterTodos_DeveRetornarTodosFornecedores()
        {
            // Act
            var response = await _client.GetAsync(UrlBase);

            // Assert
            response.EnsureSuccessStatusCode();
            var fornecedores = await response.Content.ReadFromJsonAsync<IEnumerable<FornecedorDto>>();
            Assert.NotNull(fornecedores);
            Assert.NotEmpty(fornecedores);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarFornecedorQuandoEncontrado()
        {
            // Arrange
            var fornecedorId = FornecedorIdValido;
            AdicionarTokenAutorizacao();

            // Act
            var response = await _client.GetAsync($"{UrlBase}/{fornecedorId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var fornecedor = await response.Content.ReadFromJsonAsync<FornecedorDto>();
            fornecedor.Should().NotBeNull();
            fornecedor.Id.Should().Be(fornecedorId);
        }

        [Fact]
        public async Task Adicionar_DeveAdicionarNovoFornecedor()
        {
            // Arrange
            var novoFornecedor = FornecedorFaker.GerarFornecedores(1).FirstOrDefault();
            AdicionarTokenAutorizacao();

            // Act
            var response = await _client.PostAsJsonAsync(UrlBase, novoFornecedor);

            // Assert
            response.EnsureSuccessStatusCode();

            var fornecedor = await response.Content.ReadFromJsonAsync<ResultWrapper>();
            fornecedor.Should().NotBeNull();
            fornecedor?.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Excluir_DeveExcluirQuandoEncontrado()
        {
            // Arrange
            var fornecedorId = FornecedorIdValido;
            AdicionarTokenAutorizacao();

            // Act
            var response = await _client.DeleteAsync($"{UrlBase}/{fornecedorId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var retorno = await response.Content.ReadFromJsonAsync<ResultWrapper>();
            retorno.Should().NotBeNull();
            retorno?.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Atualizar_DeveAtualizarFornecedorComSucesso()
        {
            // Arrange
            var fornecedor = FornecedorFaker.GerarFornecedores(1).FirstOrDefault();
            fornecedor.Id = FornecedorIdValido;
            AdicionarTokenAutorizacao();

            // Act
            var response = await _client.PutAsJsonAsync($"{UrlBase}/{fornecedor.Id}", fornecedor);

            // Assert
            response.EnsureSuccessStatusCode();
            var retorno = await response.Content.ReadFromJsonAsync<ResultWrapper>();
            retorno.Should().NotBeNull();
            retorno?.Success.Should().BeTrue();
        }
    }
}