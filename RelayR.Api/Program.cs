using RelayR.Api.Notifications;
using RelayR.Api.Requests;
using RelayR.AspNetCore.Core;
using RelayR.AspNetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRelayR(c => c.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("publishnotification", (IMediator mediator) =>
{
    mediator.Publish(new MyNotification($"Hello from publish - {DateTime.Now}"));
    return Results.Ok();
});

app.MapGet("sendrequest", async (IMediator mediator) =>
{
    var result = await mediator.Send(new MyRequest("hello from the request"));
    return Results.Ok(result);
});

app.Run();