using FreeDOW.API.Core.Abstract;
using FreeDOW.API.DataAccess.Data;
using FreeDOW.API.DataAccess.Repositries;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Negotiate;
using FreeDOW.API.WebHost.Authentication;
using FreeDOW.API.Core.Helpers;
using FreeDOW.API.Core.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OrgDbContext>(x =>
{
    x.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});
builder.Services.AddDbContext<WorkTimeManageDbContext>(x =>
{
    x.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});
builder.Services.AddScoped(typeof(IOrgRepository<>), typeof(OrgRepository<>));
builder.Services.AddScoped<IOrgRepository<Employee>, EmployeeRepository>();
builder.Services.AddScoped(typeof(IWorkTimeManageRepository<>), typeof(WorkTimeManageRepository<>));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate()
    .AddScheme<JwtSchemeOptions, JwtSchemeHandler>(
        AuthSchemas.Jwt, options => { 
            options.IsActive = true; 
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
