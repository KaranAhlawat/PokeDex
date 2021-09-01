using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using PokeDex.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace PokeDex.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory) =>
            (_logger, _httpClientFactory) = (logger, httpClientFactory);

        private readonly string pokeUrl = "https://pokeapi.co/api/v2/pokemon/?limit=150";

        private List<Pokeman> _pokemen { get; set; } = new List<Pokeman>();
        public List<Pokeman> Pokemon { get; set; } = new List<Pokeman>();

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            try
            {
                HttpResponseMessage httpResponse = await httpClient.GetAsync(pokeUrl);
                using (JsonDocument content = await JsonDocument.ParseAsync(await httpResponse.Content.ReadAsStreamAsync()))
                {


                    JsonElement results = content.RootElement.GetProperty("results");

                    int i = 1;

                    foreach (var element in results.EnumerateArray())
                    {
                        _pokemen.Add(new Pokeman
                        {
                            Id = i,
                            Name = element.GetProperty("name").ToString(),
                            Url = element.GetProperty("url").ToString(),
                            Image = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{i}.png"
                        });

                        i++;
                    }
                }

                if (!string.IsNullOrEmpty(SearchString))
                {
                    _pokemen = _pokemen.Where(x => x.Name.Contains(SearchString, System.StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e.ToString());
            }

            Pokemon = _pokemen;

        }

        public void HandleSearch()
        {
            _logger.LogInformation("Logging search");
        }
    }
}
