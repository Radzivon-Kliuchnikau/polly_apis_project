var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/getresponse", (int id) => // id is between 1 and 100
{
    Random rnd = new Random();
    var rndInteger = rnd.Next(1, 101);

    if (rndInteger >= id)
    {
        Console.WriteLine("--> Endpoint failed with 500");

        return Results.StatusCode(500);
    }

    Console.WriteLine("--> Endpoint succeded with 200");

    return Results.Ok();
})
.WithName("SimpleResponseService")
.WithOpenApi();

app.Run();
