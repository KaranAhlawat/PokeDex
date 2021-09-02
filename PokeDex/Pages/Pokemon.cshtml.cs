using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PokeDex.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeDex.Pages
{
    public class PokemonModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PokemonModel> _logger;
        private readonly string url = "https://pokeapi.co/api/v2/pokemon/";

        public int Id { get; set; }
        public string PokeName { get; set; }
        public List<string> PokeTypes { get; set; }
        public string ImgUrl { get; set; }

        public PokemonModel(ILogger<PokemonModel> logger, IHttpClientFactory httpClientFactory) =>
            (_logger, _httpClientFactory) = (logger, httpClientFactory);

        public async Task OnGetAsync(int id)
        {
            Id = id;

            ImgUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{Id}.png";

            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse = await httpClient.GetAsync(url + $"{Id}");

            if (httpResponse.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Stream responseStream = await httpResponse.Content.ReadAsStreamAsync();

                using (JsonDocument content = await JsonDocument.ParseAsync(responseStream))
                {
                    JsonElement name = content.RootElement.GetProperty("name");

                    PokeName = name.ToString();

                    JsonElement types = content.RootElement.GetProperty("types");

                    PokeTypes = new List<string>();

                    foreach (var element in types.EnumerateArray())
                    {
                        var type = element.GetProperty("type").GetProperty("name").ToString();

                        PokeTypes.Add(type);
                    }

                }
            }
            else
            {
                _logger.LogInformation(httpResponse.ReasonPhrase);
            }
        }
    }
}
