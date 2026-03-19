using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
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

app.MapGet("/mygames", (ClaimsPrincipal user) =>
{
    ArgumentNullException.ThrowIfNull(user.Identity?.Name, "User identity is null or does not contain a name claim.");
    var username = user.Identity?.Name; // Get the username from the claims

    if (!gamesMap.ContainsKey(username))
    {
        return Results.NotFound("No games found for the authenticated user."); // Return 404 if no games are found for the user
    }

    return Results.Ok(gamesMap[username]); // Return the games for the authenticated user
})
.RequireAuthorization(policy =>
    {
        policy.RequireRole("player"); //Only users with the "player" role can access this endpoint
    }); //needed to add authentication and authorization to access this endpoint
app.Run();
