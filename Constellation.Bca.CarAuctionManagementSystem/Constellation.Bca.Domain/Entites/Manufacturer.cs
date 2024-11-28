
namespace Constellation.Bca.Domain.Entites
{
    public class Manufacturer
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        public IEnumerable<Model>? Models { get; set; }
    }
}
