using Constellation.Bca.Domain.Enums;

namespace Constellation.Bca.Domain.ValueObjects
{
    public class QueryFilter
    {
        public Columns Field { get; set; }
        public Operator Operator { get; set; }
        public string Value { get; set; }
        public Logic Logic { get; set; }
        public List<QueryFilter> QueryFilters { get; set; }
    }
}
