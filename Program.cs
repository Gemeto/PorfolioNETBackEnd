using PorfolioWeb.Services.Context;
using PorfolioWeb.Models;
using Microsoft.EntityFrameworkCore;

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

app.MapPost("/", async (PortafolioContextService context, JobExperience JobExperience) => {
    context.JobExperiences.Add(JobExperience);
    await context.SaveChangesAsync();
    _rebuildGitStaticPage();
});

app.Run();


void _rebuildGitStaticPage(){
    //Launch git command to rebuild frontend repo
    System.Diagnostics.Process.Start(@"C:\\Users\\User\\Desktop\\PorfolioWorkspace\\PorfolioAstroFrontEnd\\Rebuild.cmd");
}
