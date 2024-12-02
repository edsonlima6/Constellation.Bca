
namespace Constellation.Bca.Domain.Common
{
    public static class ConstantProvider
    {
        public static string UniqueIdentifierPattern = "JYAVP18E07A005321";
        public static string GetDuplicatedIdentifierMessage(string identifier) => $"The identifier ({identifier}) already exists";

        public static string GetExistingActiveAuctionMessage() => $"There is an active auction already created";
        public static string GetNonExistingVehicleMessage() => $"Vehicle does not exists";
        public static string GetInvalidAuctionIdMessage() => $"Invalid auction id";
        public static string GetNotFoundAuctionIdMessage() => $"Auction not found";
        public static string GetClosed_atMustBeGreanterThanCurrentDayMessage() => $"Closed_at field must be greanter or equal than current day";
        public static string GetNonExistingActiveAuctionMessage() => $"There is no active auction.";
        public static string GetUserDoesNotExistsMessage() => $"User does not exists";
        public static string GetAmountLesserThanLatestBidAuctiontionMessage(string value) => $"The bid amount is lesser than the latest one. ({value})";
    }
}
