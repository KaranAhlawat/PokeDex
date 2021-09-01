using System.Text.Json;

namespace PokeDex.Models
{
    public class Pokeman
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
