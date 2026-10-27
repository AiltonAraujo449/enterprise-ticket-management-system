using ETMS.Tickets.Application.Tickets.CreateTicket;
using ETMS.Tickets.Application.Tickets.GetTicketById;
using ETMS.Tickets.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddTicketsInfrastructure(
    builder.Configuration);
builder.Services.AddScoped<CreateTicketHandler>();
builder.Services.AddScoped<GetTicketByIdHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


var tickets = app.MapGroup("/api/tickets");

tickets.MapPost("/", async (
    CreateTicketCommand command,
    CreateTicketHandler handler,
    CancellationToken cancellationToken) =>
{
    var ticketId = await handler.HandleAsync(
        command,
        cancellationToken);

    return Results.Created(
        $"/api/tickets/{ticketId}",
        new { id = ticketId });
});

tickets.MapGet("/{id:guid}", async (
    Guid id,
    GetTicketByIdHandler handler,
    CancellationToken cancellationToken) =>
{
    var ticket = await handler.HandleAsync(
        new GetTicketByIdQuery(id),
        cancellationToken);

    return ticket is null
        ? Results.NotFound()
        : Results.Ok(ticket);
});

app.Run();
