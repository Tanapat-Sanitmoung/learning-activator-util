var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Fooer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/test", (IServiceProvider services) =>
{
    var fighter = (IFooFighter)ActivatorUtilities.CreateInstance(
        services, typeof(IFooFighter), ["hi", 1, "there", true]);

        fighter.CallFooer();

        /*
        OUTPUT:
            a => 1
            b => hi
            c => True
            d => there
        */
})
.WithName("test")
.WithOpenApi();

app.Run();

class Fooer
{
    public void DoItsJob()
    {
        Console.WriteLine("It is my duty");
    }
}

class IFooFighter
{
    private readonly Fooer _fooer;

    public IFooFighter(int a, string b, bool c, string d, Fooer fooer)
    {
        Console.WriteLine("a => {0}", a);
        Console.WriteLine("b => {0}", b);
        Console.WriteLine("c => {0}", c);
        Console.WriteLine("d => {0}", d);
        _fooer = fooer;
    }

    public void CallFooer()
    {
        _fooer.DoItsJob();
    }
}
