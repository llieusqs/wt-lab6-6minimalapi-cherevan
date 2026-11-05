using Microsoft.EntityFrameworkCore; 
using wt_lab7_7postgres_efcore_cherevan.Models;
using wt_lab7_7postgres_efcore_cherevan.Data;

var builder = WebApplication.CreateBuilder(args);

// =========================================================
// БЛОК 9 и 25-26. Подключение к БД (Локально и на Render)
// =========================================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var databaseUrl = builder.Configuration["DATABASE_URL"]; // Читаем переменную Render

// Если мы на Render, переделываем URL в строку подключения
if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2);
    
    // Берем порт из URL, если он есть. Если нет (как на Render) — ставим 5432.
    int port = uri.Port > 0 ? uri.Port : 5432; 

    connectionString =
        $"Host={uri.Host};" +
        $"Port={port};" + // <-- Вот здесь исправление
        $"Database={uri.AbsolutePath.TrimStart('/')};" +
        $"Username={Uri.UnescapeDataString(userInfo[0])};" +
        $"Password={Uri.UnescapeDataString(userInfo[1])};" +
        "SSL Mode=Disable";
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// =========================================================
// БЛОК 6. Настройка Swagger UI
// =========================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

var app = builder.Build();

// =========================================================
// БЛОК 12. Автоматическое применение миграций
// =========================================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); 
}

app.UseSwagger();      
app.UseSwaggerUI();    

// =========================================================
// БЛОК 1. Базовые эндпоинты
// =========================================================
app.MapGet("/", () => "Minimal API работает!");

app.MapGet("/api/info", () =>
{
    return new
    {
        Application = "Web Technologies",
        Technology = "ASP.NET Core Minimal API + PostgreSQL",
        Version = "1.0"
    };
});

// =========================================================
// БЛОК 13. Перенос CRUD на Entity Framework Core
// =========================================================

// 13.1. Получение списка (с поиском)
app.MapGet("/api/tasks", async (string? search, AppDbContext db) =>
{
    var query = db.Tasks.AsQueryable();

    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(x => 
            x.Title.ToLower().Contains(search.ToLower()) || 
            x.Description.ToLower().Contains(search.ToLower()));
    }

    var tasks = await query.ToListAsync(); // Запрос к БД
    return Results.Ok(tasks); 
});

// 14. Получение по ID
app.MapGet("/api/tasks/{id}", async (int id, AppDbContext db) =>
{
    var task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
    if (task == null) return Results.NotFound(); 
    return Results.Ok(task); 
});

// 16. Создание (POST)
app.MapPost("/api/tasks", async (TaskItem task, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(task.Title))
        return Results.BadRequest(new { error = "Название задачи не может быть пустым" });

    db.Tasks.Add(task);
    await db.SaveChangesAsync(); // Сохраняем в БД (INSERT)

    return Results.Created($"/api/tasks/{task.Id}", task); 
});

// 17. Изменение (PUT)
app.MapPut("/api/tasks/{id}", async (int id, TaskItem updatedTask, AppDbContext db) =>
{
    var task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
    if (task == null) return Results.NotFound(); 

    task.Title = updatedTask.Title;
    task.Description = updatedTask.Description;
    task.Status = updatedTask.Status;
    task.Priority = updatedTask.Priority;
    task.ProjectId = updatedTask.ProjectId;
    task.AssigneeId = updatedTask.AssigneeId;
    task.DueDate = updatedTask.DueDate;

    await db.SaveChangesAsync(); // Сохраняем изменения (UPDATE)
    return Results.Ok(task); 
});

// 18. Удаление (DELETE)
app.MapDelete("/api/tasks/{id}", async (int id, AppDbContext db) =>
{
    var task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
    if (task == null) return Results.NotFound(); 

    db.Tasks.Remove(task);
    await db.SaveChangesAsync(); // Удаляем из БД (DELETE)
    return Results.NoContent(); 
});

// =========================================================
// БЛОК 19. Использование LINQ
// =========================================================

// 1. Фильтрация (задачи с высоким приоритетом)
app.MapGet("/api/tasks/filter", async (AppDbContext db) =>
{
    var tasks = await db.Tasks
        .Where(x => x.Priority == "High")
        .ToListAsync();
    return Results.Ok(tasks);
});

// 2. Сортировка (по дате выполнения)
app.MapGet("/api/tasks/sort", async (AppDbContext db) =>
{
    var tasks = await db.Tasks
        .OrderBy(x => x.DueDate)
        .ToListAsync();
    return Results.Ok(tasks);
});

// 3. Выборка с условием (активные задачи, отсортированные по названию)
app.MapGet("/api/tasks/active", async (AppDbContext db) =>
{
    var tasks = await db.Tasks
        .Where(x => x.Status != "Done") 
        .OrderBy(x => x.Title)          
        .ToListAsync();
    return Results.Ok(tasks);
});

// 4. Собственный запрос (статистика: сколько задач в каждом статусе)
app.MapGet("/api/tasks/stats", async (AppDbContext db) =>
{
    var stats = await db.Tasks
        .GroupBy(x => x.Status)
        .Select(g => new { Status = g.Key, Count = g.Count() })
        .ToListAsync();
    return Results.Ok(stats);
});

// =========================================================
// БЛОК 7. Настройка порта для Render
// =========================================================
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");