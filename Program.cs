using PorfolioNETBackEnd.Services.Context;
using PorfolioNETBackEnd.Models;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PortafolioContextService>(
    options => options.UseMySQL(
        builder.Configuration.GetConnectionString("MySql")??""));
builder.Services.AddScoped<IValidator<JobExperience>, JobExperienceValidator>();
builder.Services.AddScoped<IValidator<Category>, CategoryValidator>();
builder.Services.AddScoped<IValidator<Image>, ImageValidator>();
builder.Services.AddScoped<IValidator<User>, UserValidator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//JobExperiences
//Get all job experiences
app.MapGet("/JobExperience", async (PortafolioContextService context) => 
{
    IList<JobExperience> JobExperience = await context.JobExperiences.ToArrayAsync();
    return JobExperience;
});
//Get job experience by id
app.MapGet("/JobExperience/{id}", async (int id, PortafolioContextService context) => 
{
    JobExperience? JobExperience = await context.JobExperiences.FirstOrDefaultAsync(x => x.Id == id);
    return JobExperience;
});
//Put new/edited job experience
app.MapPost("/JobExperience", async (HttpRequest request, PortafolioContextService context, IValidator<JobExperience> validator) => {
    var postData = request.Form.ToDictionary( x => x.Key.ToString(), x => x.Value);
    JobExperience? jobExperience = null;
    if(!postData.ContainsKey("id")){
        jobExperience = new JobExperience();
    }else{
        jobExperience = await context.JobExperiences.FirstOrDefaultAsync(x => x.Id == int.Parse(postData["id"].ToString()));
    }
    if(jobExperience==null){
        return Results.NotFound();
    }
    jobExperience.Title = postData["title"].ToString();
    jobExperience.Description = postData["description"].ToString();
    jobExperience.CategoryId = postData.ContainsKey("categoryId") ? int.Parse(postData["categoryId"].ToString()) : null;
    var valid = await validator.ValidateAsync(jobExperience);
    if(valid.IsValid){
        context.Attach(jobExperience).State = EntityState.Modified;
        await context.SaveChangesAsync();
        _rebuildGitPage();
    } else {
        return Results.BadRequest();
    }
    return Results.Ok();
});
//Categories
//Get all categories
app.MapGet("/Category", async (PortafolioContextService context) => 
{
    IList<Category> JobExperience = await context.Categories.ToArrayAsync();
    return JobExperience;
});
//Get category by id
app.MapGet("/Category/{id}", async (int id, PortafolioContextService context) => 
{
    Category? Category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
    return Category;
});
//Get category-job experiences by id
app.MapGet("/Category/{id}/JobExperience", async (int id, PortafolioContextService context) => 
{
    IList<JobExperience>? jobExperiences = await context.JobExperiences.Where(x => x.CategoryId == id).ToArrayAsync();
    return jobExperiences;
});
//Post new/edited category
app.MapPost("/Category", async (HttpRequest request, PortafolioContextService context, IValidator<Category> validator) => {
    var postData = request.Form.ToDictionary( x => x.Key.ToString(), x => x.Value);
    Category? category = null;
    if(!postData.ContainsKey("id")){
        category = new Category();
    }else{
        category = await context.Categories.FirstOrDefaultAsync(x => x.Id == int.Parse(postData["id"].ToString()));
    }
    if(category==null){
        return Results.NotFound();
    }
    category.Name = postData["name"].ToString();
    if(postData.ContainsKey("parentId") && !postData["parentId"].IsNullOrEmpty() && postData["parentId"] != ""){
           category.ParentId = int.Parse(postData["parentId"].ToString());
    }
    var valid = await validator.ValidateAsync(category);
    if(valid.IsValid){
        if(postData.ContainsKey("id")){
        context.Attach(category).State = EntityState.Modified;
        }else{
            context.Add(category);
        }
        await context.SaveChangesAsync();
        _rebuildGitPage();
    } else {
        return Results.BadRequest();
    }
    return Results.Ok();
});
//Images
//Get all images
app.MapGet("/Image", async (PortafolioContextService context) => 
{
    IList<Image> images = await context.Images.ToArrayAsync();
    return images;
});
//Get image by id
app.MapGet("/Image/{id}", async (int id, PortafolioContextService context) => 
{
    Image? image = await context.Images.FirstOrDefaultAsync(x => x.Id == id);
    return image;
});
//Post new/edited image
app.MapPost("/Image", async (HttpRequest request, PortafolioContextService context, IValidator<Image> validator) => {
    var postData = request.Form.ToDictionary( x => x.Key.ToString(), x => x.Value);
    Image? image = null;
    if(!postData.ContainsKey("id")){
        image = new Image();
    }else{
        image = await context.Images.FirstOrDefaultAsync(x => x.Id == int.Parse(postData["id"].ToString()));
    }
    if(image==null){
        return Results.NotFound();
    }
    image.Path = "";
    var valid = await validator.ValidateAsync(image);
    if(valid.IsValid){
        context.Attach(image).State = EntityState.Modified;
        await context.SaveChangesAsync();
        _rebuildGitPage();
    } else {
        return Results.BadRequest();
    }
    return Results.Ok();
});
//Users
//Get all images
app.MapGet("/Image", async (PortafolioContextService context) => 
{
    IList<User> users = await context.Users.ToArrayAsync();
    return users;
});
//Get user by id
app.MapGet("/User/{id}", async (int id, PortafolioContextService context) => 
{
    User? user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
    return user;
});
//Post new/edited user
app.MapPost("/User", async (HttpRequest request, PortafolioContextService context, IValidator<User> validator) => {
    var postData = request.Form.ToDictionary( x => x.Key.ToString(), x => x.Value);
    User? user = null;
    if(!postData.ContainsKey("id")){
        user = new User();
    }else{
        user = await context.Users.FirstOrDefaultAsync(x => x.Id == int.Parse(postData["id"].ToString()));
    }
    if(user==null){
        return Results.NotFound();
    }
    var valid = await validator.ValidateAsync(user);
    if(valid.IsValid){
        context.Attach(user).State = EntityState.Modified;
        await context.SaveChangesAsync();
        _rebuildGitPage();
    } else {
        return Results.BadRequest();
    }
    return Results.Ok();
});
//App run and recurrent functions
app.Run();


void _rebuildGitPage(){
    //Launch git command to rebuild frontend repo
    System.Diagnostics.Process.Start(@"C:\\Users\\User\\Desktop\\PorfolioWorkspace\\PorfolioAstroFrontEnd\\Rebuild.cmd");
}

/*async void _managePostRequest<T>(HttpRequest request, PortafolioContextService context, IValidator<T> validator, Func<T> retrieveModelFunc, T model){
    var postData = request.Form.ToDictionary( x => x.Key.ToString(), x => x.Value);
    if(!postData.ContainsKey("id")){
        //model = new T();
    }else{
        model = retrieveModelFunc();
    }
    if(model==null){
        //return Results.NotFound();
    }
    model = _setPostDataToModel(model, postData);
    var valid = await validator.ValidateAsync(model);
    if(valid.IsValid){
        if(postData.ContainsKey("id")){
        context.Attach(model).State = EntityState.Modified;
        }else{
            context.Add(model);
        }
        await context.SaveChangesAsync();
        _rebuildGitPage();
    } else {
        //return Results.BadRequest();
    }
    //return Results.Ok();
}

T _setPostDataToModel<T>(T model, Dictionary<string, Microsoft.Extensions.Primitives.StringValues> postData){
    foreach (var item in postData)
    {
        var prop = model.GetType().GetProperty(item.Key.ToString());
        if(prop != null){
            prop.SetValue(model, postData[item.Key.ToString()].ToString());
        }
    }
    return model;
}*/