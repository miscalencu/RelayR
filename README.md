# RelayR.AspNetCore

Simple implementation of Mediator pattern using channels in .Net.

Uses a BackgroundJob to consume the channels and execute the handlers.

Supports dispatching Events (no returned value) and Queries (returned value).

## ✅ TO DO:
📌 Short term:
- add unit tests
- add load tests
- add benchmarks
- add support for multiple handlers for the same event
- add support for Commands (one handler, optional returned data).
- ~~register all handlers automatically using reflection~~
- add extra parameters for channels creation

Long term:
- implement pipeline behaviors: Logging, Validation, Performance timing, Retry logic, Authorization
- add pre and post processors

🙏 Thanks to @andrea-pellegrinelli for co-authoring this project and for the collaboration!