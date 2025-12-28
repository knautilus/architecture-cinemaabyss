using Yarp.ReverseProxy;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var moviesMigrationPercent = int.Parse(builder.Configuration.GetSection("MOVIES_MIGRATION_PERCENT").Value!);

Console.WriteLine($"migration percent={moviesMigrationPercent}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use((context, next) =>
    {
        if (context.GetEndpoint()?.DisplayName != "monolith")
        {
            var lookup = context.RequestServices.GetRequiredService<IProxyStateLookup>();

            if (lookup.TryGetCluster(ChooseCluster(context, moviesMigrationPercent), out var cluster))
            {
                context.ReassignProxyRequest(cluster);
            }
        }

        return next();
    });

});

app.MapControllers();

app.Run("http://*:8000");

string ChooseCluster(HttpContext context, int moviesMigrationPercent)
{
    return Random.Shared.Next(100) < moviesMigrationPercent ? "moviesservice" : "monolith";
}
