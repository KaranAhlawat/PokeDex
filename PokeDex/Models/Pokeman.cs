using System.Collections.Generic;
using System.Text.Json;

namespace PokeDex.Models
{
    //public class Pokeman
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Url { get; set; }
    //    public string Image { get; set; }

    //    public override string ToString() => JsonSerializer.Serialize(this);
    //}

    public class APIResponse
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string? Previous { get; set; }
        public List<Pokeman> Results { get; set; }
    }

    public class Pokeman
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
    }

}
