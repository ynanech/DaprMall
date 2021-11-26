global using DaprMall.Share;
using DaprMall.Product.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddDapr();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//dapr
builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<StockActor>();
    options.Actors.RegisterActor<ScoreActor>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();


//dapr
app.MapActorsHandlers();

app.MapControllers();


app.Run();
