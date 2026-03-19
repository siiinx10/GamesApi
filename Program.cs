using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

Dictionary<string, List<string>> gamesMap= new ()
{
    {"player1",  new List<string>() { "Fortnite", "Apex Legends" }},
    {"player2",  new List<string>() { "Call of Duty", "Battlefield" ,"Valorant" }},
    {"player3",  new List<string>() { "Minecraft", "Roblox" }}
};

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer(); //needed to add authentication and authorization to access this endpoint

builder.Services.AddAuthorization();

var app = builder.Build();

// Add middleware
app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/playergames", () => gamesMap)
    .RequireAuthorization(policy =>
    {
        policy.RequireRole("Admin"); //Only users with the "Admin" role can access this endpoint
    }); //needed to add authentication and authorization to access this endpoint

app.Run();
