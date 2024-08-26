using api.avantco.ActionFilter;
using api.avantco.Mapper;
using AutoMapper;
using dal.avantco.Services.Logger;
using biz.avantco.Services.ILogger;
using Microsoft.EntityFrameworkCore;
using biz.avantco.Models.Email;
using biz.avantco.Services.Email;
using dal.avantco.Services.Email;
using biz.avantco.Repository.ScheduleAppointment;
using dal.avantco.Repository.User;
using dal.avantco.Repository.Users;
using biz.avantco.Repository.NewLetter;
using dal.avantco.Repository.NewsLetter;
using biz.avantco.Repository.IUsers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "api.avantco", Version = "v1" });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<dal.avantco.DBContext.AvantcoContext>(options => options.UseSqlServer(connectionString));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailConfigurations"));
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
    builder =>
    {
        builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost")
            .WithOrigins("http://localhost:4200")
            .WithOrigins("http://localhost:8100")
            .WithOrigins("http://demo-minimalist.com")
            .WithOrigins("http://34.237.214.147")
            .WithOrigins("https://my.premierds.com/")
            .WithOrigins("Ionic://localhost")
            .WithOrigins("capacitor://localhost")
            .WithOrigins("http://localhost:63410")
            .AllowCredentials();
    }));


builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddScoped<ValidationFilterAttribute>();


#region MyRegion
builder.Services.AddTransient<IScheduleAppointmentrRepository, ScheduleAppointmentRepository>();
builder.Services.AddTransient<INewsLetter, NewsLetterRepository>();
builder.Services.AddTransient<IUsers, UsersRepository>();
#endregion 


//CONFIGURACION DE MAPPER//
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MapperProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors("CorsPolicy");
app.UseSwaggerUI(c =>
{
    app.UseSwagger().UseDeveloperExceptionPage();
#if DEBUG
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "api.avantco v1");
#else
    c.SwaggerEndpoint("/back/api_avantco/swagger/v1/swagger.json", "api.avantco v1");
#endif
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
