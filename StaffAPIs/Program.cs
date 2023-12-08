using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffAPIs;
using StaffLib.Services;
using StaffManagement.Models;
using StaffManagement.Repositories;
using StaffManagement.Services;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddDbContext<SqlServerDbContext ,APIDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:5001/",
                                                  "http://localhost:5001")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});

builder.Services.AddTransient<IStaffService, StaffService>();
builder.Services.AddTransient<IStaffRepository, StaffRepository>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();







var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
MapStaffEndpoint(app,"Staffs");


app.Run();
[EnableCors]
void MapStaffEndpoint(WebApplication app, string tag)
{
    app.MapGet("api/v1/staffs", async ([FromServices] IStaffService staffService) =>
    {
        return await staffService.GetStaffs();
    }).WithTags(tag);

    app.MapGet("api/v1/staffs/{id}", async ([FromServices] IStaffService staffService, int id) =>
    {
        return await staffService.GetStaffById(id);
    }).WithTags(tag);

    app.MapPost("api/v1/staffs", async ([FromServices] IStaffService staffService, Staff staff) =>
    {
        return await staffService.AddStaff(staff);
    }).WithTags(tag);
    
    app.MapPut ("api/v1/staffs/{id}", async ([FromServices] IStaffService staffService, int id,Staff staff) =>
    {
        return await staffService.UpdateStaff(staff);
    }).WithTags(tag);

    app.MapDelete("api/v1/staffs/{id}", async ([FromServices] IStaffService staffService, int id) =>
    {
        return await staffService.DeleteStaff(id);
    }).WithTags(tag);

}
