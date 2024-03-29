using RequestService.Policies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("Test Polly").AddPolicyHandler(
    request => new ClientPolicy().ImmediateHttpRetry
);

builder.Services.AddSingleton<ClientPolicy>(new ClientPolicy());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/api/request", async (int id, IHttpClientFactory clientFactory) =>
{
    // var client = new HttpClient();
    var client = clientFactory.CreateClient("Test Polly");

    var response = await client.GetAsync($"http://localhost:5177/getresponse?id={id}");
    // var response = await clientPolicy.ImmediateHttpRetry.ExecuteAsync(
    //     () => client.GetAsync($"http://localhost:5177/getresponse?id={id}"));

    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("--> Response service returned SUCCESS");

        return Results.Ok();
    }

    Console.WriteLine("--> Response service returned FAILURE");

    return Results.StatusCode(500);
})
.WithName("RequestService")
.WithOpenApi();

app.Run();
