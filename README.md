# Henry Meds Code Assessment

Thank you for the opportunity to do this code assessment! This was a great challenge and provided the chance to showcase the sythesis of several skill areas, and I really enjoyed doing it.

## Starting the API

To run and test this API:

- Ensure that the .NET 6 runtime is installed
- Clone this repository
- Navigate to the project directory
- Run <code>dotnet run</code>
- Navigate to <code>/swagger/index.html</code> to use the API endpoints, or your favorite API testing tool

## Overall design

For the backing database, I selected SQLite for expediency, simplicity, and data persistence with testing.

I wanted reservations, providers, and provider schedules to be separate tables as I viewed them as related but separate concerns. 
I created the object models first to reflect my vision of their appropriate structures, then the tables, followed by the API controllers.

For testing, the providers table is seeded with 2 test providers.

## Assumptions

For the sake of time, I had to make a few assumptions

- Provider availability can be partitioned into a whole number of 15 minute blocks
- Provider availability can only occur on 15 minute multiples (e.g. HH:15, HH:30)

## If I had more time, I would add

- Product features
	- Ability for a client to get their reservations, confirmed or otherwise
	- Ability for clients to change or cancel their reservation
	- Ability for providers to reschedule or cancel their reservations
	- A provider-oriented view that would show their full schedule, availability and reservations both
- More extensive request validation, e.g.
	- Ensure provider schedule updates are in the future
	- Ensure reservation requests are in the future
- Exception handling, in particular custom exceptions to better model what happened
- Unit testing, in particular valid and invalid request/response testing for each API endpoint
- Ability to handle times that are not 15 minute multiples
- Streamline object models more, create more base classes for common data (e.g. error message fields)
- Async/await for database operations

## What I'd add for production

- Authentication
- Authorization
	- Making sure a provider can only update their own schedule
	- Making sure a client can only confirm their own reservations
	- Making sure a client can only view/book their provider's schedule (if a provider assignment is applicable)
- Using a full-fledged RDBMS, such as PostgreSQL
	- Use foreign keys to link reservation and provider ids across tables
- Moving the expired reservation clearing task to out-of-band


# Thank you again!