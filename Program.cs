using PorfolioWeb.Services.Context;
using PorfolioWeb.Models;
using Microsoft.EntityFrameworkCore;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using MySqlX.XDevAPI.Common;
using System.Text.Json;
using Azure.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PortafolioContextService>(
    options => options.UseMySQL(
        builder.Configuration.GetConnectionString("MySql")??""));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", async (PortafolioContextService context) => 
{
    IList<JobExperience> JobExperience = await context.JobExperiences.ToArrayAsync();
    return JobExperience;
});

app.MapGet("/JobExperience/{id}", async (int id, PortafolioContextService context) => 
{
    JobExperience? JobExperience = await context.JobExperiences.FirstOrDefaultAsync(x => x.Id == id);
    return JobExperience;
});

app.MapPost("/", async (HttpRequest request, PortafolioContextService context) => {
    var postData = request.Form.ToDictionary( x => x.Key.ToString(), x => x.Value);
    Console.WriteLine(postData["id"]);
    JobExperience? jobExperience = await context.JobExperiences.FirstOrDefaultAsync(x => x.Id == int.Parse(postData["id"].ToString()));
    if(jobExperience==null){
        return Results.NotFound();
    }
    jobExperience.Title = postData["title"];
    jobExperience.Description = postData["description"];
    context.Attach(jobExperience).State = EntityState.Modified;
    await context.SaveChangesAsync();
    _rebuildGitStaticPage();
    return Results.Ok();
});

app.Run();


void _rebuildGitStaticPage(){
    //Launch git command to rebuild frontend repo
    System.Diagnostics.Process.Start(@"C:\\Users\\User\\Desktop\\PorfolioWorkspace\\PorfolioAstroFrontEnd\\Rebuild.cmd");
}