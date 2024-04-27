using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMessagePrinter();
builder.Services.AddMessagePrinter<FooFighter>(
    (cfg) => cfg.UseMassageBag<FooMessageBag>());

builder.Services.AddMessagePrinter<NonFooFighter>(
    (cfg) => cfg.UseMassageBag<NonFooMessageBag>());

builder.Services.AddMessagePrinter<IFooWorker, FooWorker>(
    (cfg) => cfg.UseMassageBag<FooWorkerMessageBag>()
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/default", ([FromServices] IMessagePrinter printer) =>
    {
        return printer.Print("Magic");
    })
    .WithName("default")
    .WithOpenApi();

app.MapGet("/foo", ([FromServices] FooFighter fooFighter) =>
    {
        return fooFighter.DoMagic();
    })
    .WithName("foo")
    .WithOpenApi();

app.MapGet("/non-foo", ([FromServices] NonFooFighter nonFooFighter) =>
    {
        return nonFooFighter.DoSomethingElse();
    })
    .WithName("non-foo")
    .WithOpenApi();

app.MapGet("/foo-worker", ([FromServices] IFooWorker worker) =>
    {
        return worker.DoWork();
    })
    .WithName("foo-worker")
    .WithOpenApi();

app.Run();
