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

        //public async Task OnGetAsync()
        //{
        //    HttpClient httpClient = _httpClientFactory.CreateClient();
        //    try
        //    {
        //        HttpResponseMessage httpResponse = await httpClient.GetAsync(pokeUrl);
        //        using (JsonDocument content = await JsonDocument.ParseAsync(await httpResponse.Content.ReadAsStreamAsync()))
        //        {


        //            JsonElement results = content.RootElement.GetProperty("results");

        //            int i = 1;

        //            foreach (var element in results.EnumerateArray())
        //            {
        //                _pokemen.Add(new Pokeman
        //                {
        //                    Id = i,
        //                    Name = element.GetProperty("name").ToString(),
        //                    Url = element.GetProperty("url").ToString(),
        //                    Image = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{i}.png"
        //                });

        //                i++;
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(SearchString))
        //        {
        //            _pokemen = _pokemen.Where(x => x.Name.Contains(SearchString, System.StringComparison.OrdinalIgnoreCase)).ToList();
        //        }
        //    }
        //    catch (HttpRequestException e)
        //    {
        //        _logger.LogError(e.ToString());
        //    }

        //    Pokemon = _pokemen;

        //}
        public async Task OnGetAsync()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse = await httpClient.GetAsync(pokeUrl);

            APIResponse apiResponse;

            if (httpResponse.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                string responseText = await httpResponse.Content.ReadAsStringAsync();
                apiResponse = JsonSerializer.Deserialize<APIResponse>(responseText, options);

                int i = 1;
                foreach(Pokeman p in apiResponse.Results)
                {
                    p.Id = i;
                    p.Image = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{i}.png";

                    i++;
                }

                ViewData["InitData"] = new
                {
                    apiResponse
                };
            }
            else
            {
                _logger.LogError(httpResponse.ReasonPhrase);
            }

        }

        public void HandleSearch()
        {
            _logger.LogInformation("Logging search");
        }
    }
}
