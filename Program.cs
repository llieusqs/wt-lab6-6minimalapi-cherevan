using wt_lab6_6minimalapi_cherevan.Models;
using wt_lab6_6minimalapi_cherevan.Data;

var builder = WebApplication.CreateBuilder(args);

// =========================================================
// БЛОК 6. Настройка Swagger UI
// =========================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

var app = builder.Build();

// =========================================================
// БЛОК 6. Включение Swagger (без проверки на Development)
// =========================================================
app.UseSwagger();      // Генерация swagger.json
app.UseSwaggerUI();    // Красивый интерфейс: /swagger

// =========================================================
// БЛОК 1. Базовые эндпоинты
// =========================================================
app.MapGet("/", () => "Minimal API работает!");

app.MapGet("/api/info", () =>
{
    return new
    {
        Application = "Web Technologies",
        Technology = "ASP.NET Core Minimal API",
        Version = "1.0"
    };
});

// =========================================================
// БЛОК 3. GET-запросы (Чтение)
// =========================================================
app.MapGet("/api/tasks", (string? search) =>
{
    var tasks = TaskStore.Tasks.AsEnumerable();

    if (!string.IsNullOrWhiteSpace(search))
    {
        tasks = tasks.Where(x =>
            x.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
            x.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
    }

    return Results.Ok(tasks); // 200 OK
});

app.MapGet("/api/tasks/{id}", (int id) =>
{
    var task = TaskStore.Tasks.FirstOrDefault(x => x.Id == id);
    if (task == null) return Results.NotFound(); // 404
    return Results.Ok(task); // 200 OK
});

// =========================================================
// БЛОК 4. POST-запрос (Создание)
// =========================================================
app.MapPost("/api/tasks", (TaskItem task) =>
{
    // БЛОК 6 (9.1). Проверка на 400 Bad Request
    if (string.IsNullOrWhiteSpace(task.Title))
        return Results.BadRequest(new { error = "Название задачи не может быть пустым" });

    var newId = TaskStore.Tasks.Count == 0 
        ? 1 
        : TaskStore.Tasks.Max(x => x.Id) + 1;

    task.Id = newId;
    TaskStore.Tasks.Add(task); 

    return Results.Created($"/api/tasks/{task.Id}", task); // 201 Created
});

// =========================================================
// БЛОК 5. PUT и DELETE
// =========================================================
app.MapPut("/api/tasks/{id}", (int id, TaskItem updatedTask) =>
{
    var task = TaskStore.Tasks.FirstOrDefault(x => x.Id == id);
    if (task == null) return Results.NotFound(); // 404

    task.Title = updatedTask.Title;
    task.Description = updatedTask.Description;
    task.Status = updatedTask.Status;
    task.Priority = updatedTask.Priority;
    task.ProjectId = updatedTask.ProjectId;
    task.AssigneeId = updatedTask.AssigneeId;
    task.DueDate = updatedTask.DueDate;

    return Results.Ok(task); // 200 OK
});

app.MapDelete("/api/tasks/{id}", (int id) =>
{
    var task = TaskStore.Tasks.FirstOrDefault(x => x.Id == id);
    if (task == null) return Results.NotFound(); // 404

    TaskStore.Tasks.Remove(task); 
    return Results.NoContent(); // 204 No Content
});

// =========================================================
// БЛОК 7. Настройка порта для Render
// =========================================================
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");