using RelayR.Api.Models;
using RelayR.AspNetCore.Requests;

namespace RelayR.Api.Requests;

public record MyRequest(string Info) : IRequest<MyRequestResponse>;