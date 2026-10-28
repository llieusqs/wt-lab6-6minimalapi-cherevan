using wt_lab6_6minimalapi_cherevan.Models;
using wt_lab6_6minimalapi_cherevan.Data;

var builder = WebApplication.CreateBuilder(args);

// =========================================================
// БЛОК 6 (9.2). Настройка OpenAPI и Swagger UI
// =========================================================
builder.Services.AddOpenApi(); // Подключение OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Подключение графического интерфейса Swagger

var app = builder.Build();

// =========================================================
// БЛОК 6 (9.2). Включение интерфейсов документации
// =========================================================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();      // Эндпоинт: /openapi/v1.json
    app.UseSwagger();      // Генерация swagger.json
    app.UseSwaggerUI();    // Красивый интерфейс: /swagger
}

// =========================================================
// БЛОК 1. Базовые эндпоинты
// =========================================================
// Уровень 2: Проверка работы
app.MapGet("/", () => "Minimal API работает!");

// Уровень 3: Информация о приложении (JSON)
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
// 6.1 и 6.3. Получение списка задач + Фильтрация (query-параметр search)
app.MapGet("/api/tasks", (string? search) =>
{
    var tasks = TaskStore.Tasks.AsEnumerable();

    // Фильтрация, если передан параметр search
    if (!string.IsNullOrWhiteSpace(search))
    {
        tasks = tasks.Where(x =>
            x.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
            x.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
    }

    return Results.Ok(tasks); // Статус 200 OK
});

// 6.2. Получение задачи по ID
app.MapGet("/api/tasks/{id}", (int id) =>
{
    var task = TaskStore.Tasks.FirstOrDefault(x => x.Id == id);
    
    // 404 Not Found, если объекта нет
    if (task == null)
        return Results.NotFound();
        
    return Results.Ok(task); // Статус 200 OK
});

// =========================================================
// БЛОК 4. POST-запрос (Создание)
// =========================================================
app.MapPost("/api/tasks", (TaskItem task) =>
{
    // БЛОК 6 (9.1). Проверка на некорректные данные (400 Bad Request)
    if (string.IsNullOrWhiteSpace(task.Title))
        return Results.BadRequest(new { error = "Название задачи не может быть пустым" });

    // 7.1. Автоматическая генерация ID
    var newId = TaskStore.Tasks.Count == 0 
        ? 1 
        : TaskStore.Tasks.Max(x => x.Id) + 1;

    task.Id = newId;
    TaskStore.Tasks.Add(task); // Добавление в коллекцию

    // 7.2. Возврат статуса 201 Created и URL созданного ресурса
    return Results.Created($"/api/tasks/{task.Id}", task);
});

// =========================================================
// БЛОК 5. PUT и DELETE
// =========================================================
// 8.1. Изменение задачи по ID (PUT)
app.MapPut("/api/tasks/{id}", (int id, TaskItem updatedTask) =>
{
    var task = TaskStore.Tasks.FirstOrDefault(x => x.Id == id);
    
    // 404 Not Found, если объекта нет
    if (task == null) 
        return Results.NotFound();

    // Обновление свойств
    task.Title = updatedTask.Title;
    task.Description = updatedTask.Description;
    task.Status = updatedTask.Status;
    task.Priority = updatedTask.Priority;
    task.ProjectId = updatedTask.ProjectId;
    task.AssigneeId = updatedTask.AssigneeId;
    task.DueDate = updatedTask.DueDate;

    return Results.Ok(task); // Статус 200 OK
});

// 8.2. Удаление задачи по ID (DELETE)
app.MapDelete("/api/tasks/{id}", (int id) =>
{
    var task = TaskStore.Tasks.FirstOrDefault(x => x.Id == id);
    
    // 404 Not Found, если объекта нет
    if (task == null) 
        return Results.NotFound();

    TaskStore.Tasks.Remove(task); // Удаление из коллекции
    
    return Results.NoContent(); // Статус 204 No Content
});


// БЛОК 7 (10.2). Настройка порта для публикации на Render
// =========================================================
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");