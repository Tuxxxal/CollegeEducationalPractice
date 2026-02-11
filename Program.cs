using CollegeSchedule.Data;
//using CollegeSchedule.Middlewares;
using CollegeSchedule.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Загрузка переменных окружения из .env файла
DotNetEnv.Env.Load();

var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                       $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")}";

// Регистрация DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Регистрация сервисов
builder.Services.AddScoped<IScheduleService, ScheduleService>();

// Добавление контроллеров
builder.Services.AddControllers();

// Настройка Swagger/OpenAPI
// Подробнее: https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Настройка HTTP-конвейера
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();