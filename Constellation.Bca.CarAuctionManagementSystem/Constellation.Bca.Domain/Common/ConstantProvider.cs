
namespace Constellation.Bca.Domain.Common
{
    public static class ConstantProvider
    {
        public static string UniqueIdentifierPattern = "JYAVP18E07A005321";
        public static string GetDuplicatedIdentifierMessage(string identifier) => $"The identifier ({identifier}) already exists";
    }
}
