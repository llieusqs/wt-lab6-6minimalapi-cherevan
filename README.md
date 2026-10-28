# Лабораторная работа № 6. REST API на ASP.NET Core Minimal API
Предметная область: Управление IT-проектами.

## Таблица API

| Метод | URL | Назначение |
| :--- | :--- | :--- |
| GET | `/api/tasks` | получить список задач |
| GET | `/api/tasks/{id}` | получить задачу по ID |
| POST | `/api/tasks` | создать новую задачу |
| PUT | `/api/tasks/{id}` | изменить задачу по ID |
| DELETE | `/api/tasks/{id}` | удалить задачу по ID |

## Пример JSON для создания (POST)
```json
{
  "title": "Новая задача",
  "description": "Описание",
  "status": "To Do",
  "priority": "High",
  "projectId": 1,
  "assigneeId": 1,
  "dueDate": "2026-05-01T00:00:00"
}