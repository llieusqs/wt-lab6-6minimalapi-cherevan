using wt_lab6_6minimalapi_cherevan.Models;

namespace wt_lab6_6minimalapi_cherevan.Data;

public static class TaskStore
{
    public static List<TaskItem> Tasks { get; } = new()
    {
        new TaskItem
        {
            Id = 1, Title = "Спроектировать схему БД",
            Description = "ER-диаграмма для модуля задач",
            Status = "Done", Priority = "High",
            ProjectId = 1, AssigneeId = 1,
            DueDate = new DateTime(2026, 2, 1)
        },
        new TaskItem
        {
            Id = 2, Title = "Реализовать REST API",
            Description = "Endpoints CRUD для задач",
            Status = "In Progress", Priority = "High",
            ProjectId = 1, AssigneeId = 2,
            DueDate = new DateTime(2026, 3, 10)
        },
        new TaskItem
        {
            Id = 3, Title = "Написать unit-тесты",
            Description = "Покрытие не менее 60%",
            Status = "To Do", Priority = "Medium",
            ProjectId = 1, AssigneeId = 3,
            DueDate = new DateTime(2026, 3, 20)
        },
        new TaskItem
        {
            Id = 4, Title = "Настроить CI/CD",
            Description = "GitHub Actions + Render deploy",
            Status = "To Do", Priority = "High",
            ProjectId = 2, AssigneeId = 2,
            DueDate = new DateTime(2026, 4, 5)
        },
        new TaskItem
        {
            Id = 5, Title = "Обновить документацию",
            Description = "README + OpenAPI",
            Status = "To Do", Priority = "Low",
            ProjectId = 2, AssigneeId = 1,
            DueDate = new DateTime(2026, 4, 15)
        }
    };
}