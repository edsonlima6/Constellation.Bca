The solution implemented has been built using a Domain-Driven Design architecture to centralize business domains, rules, and contracts.
The presentation layer has the user interface WebApi to expose endpoints related to Auctions, Auction Bid, and Vehicle, it also has TDD to cover unit tests related to the business requirement.
The Application layer is responsible for communicating between the UI and domain/infrastructure layers, using Mediatr's splitting CQRS principle: Commands perform data writing to the database, and queries read and expose data to an external target.
The Domain layer has entities, interfaces, enums, exceptions, and notification domain based on business needed, in this case (auction, auction events like startAuction, stopAuction, placeBid, add a vehicle, and query vehicle based on filters).
The infrastructure layer has the responsibility to connect to the database, perform queries, add entities
