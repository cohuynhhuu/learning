using System.Net.Mime;
using Microsoft.OpenApi.Models;
using VotingApp.Interfaces;
using VotingApp.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IVoteQueueClient>(s =>new VoteQueueClient(builder.Configuration["ConnectionStrings:sbConnectionString"], builder.Configuration["ConnectionStrings:queueName"]));

builder.Services.AddHttpClient<IVoteDataClient, VoteDataClient>(c =>
                                                                    {
                                                                        c.BaseAddress = new Uri(builder.Configuration["ConnectionStrings:VotingDataAPIBaseUri"]);
                                                                        c.DefaultRequestHeaders.Add(
                                                                            Microsoft.Net.Http.Headers.HeaderNames.Accept,
                                                                            MediaTypeNames.Application.Json);
                                                                    });

 builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VotingApp", Version = "v1" });
                
            });
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "app v1");    
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
