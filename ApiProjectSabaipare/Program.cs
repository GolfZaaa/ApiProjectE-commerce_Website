using ApiProjectSabaipare.Data;
using ApiProjectSabaipare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });


    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddDbContext<DataContext>();

#region Identity���ҧ������� User,Role (���ѧ������§�ӴѺ)
builder.Services.AddIdentityCore<ApplicationUser>(opt =>
{
    opt.User.RequireUniqueEmail = true;
}).AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DataContext>();


builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
#endregion


var app = builder.Build();

#region  //���ҧ�ҹ�������ѵ��ѵ�
using var scope = app.Services.CreateScope(); //using ��ѧ�ӧҹ���稨ж١����¨ҡMemory
var context = scope.ServiceProvider.GetRequiredService<DataContext>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();


try
{
    await context.Database.MigrateAsync();   //���ҧ DB ����ѵ��ѵԶ���ѧ�����
}
catch (Exception ex)
{
    logger.LogError(ex, "Problem migrating data");
}
#endregion


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

await app.RunAsync();
