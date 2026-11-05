using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_cherevan.Models;

namespace wt_lab7_7postgres_efcore_cherevan.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    // =========================================================
    // ПУНКТ 23. Начальные данные (Seed Data)
    // =========================================================
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskItem>().HasData(
            new TaskItem
            {
                Id = 10, // Задаем ID вручную, чтобы не пересекалось с теми, что вы создавали
                Title = "Изучить Entity Framework Core",
                Description = "Прочитать документацию и сделать лабу",
                Status = "In Progress",
                Priority = "High",
                ProjectId = 1,
                AssigneeId = 1,
                DueDate = new DateTime(2026, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) // Указываем дату создания
            },
            new TaskItem
            {
                Id = 11,
                Title = "Настроить PostgreSQL",
                Description = "Установить, настроить и подключить к проекту",
                Status = "Done",
                Priority = "Medium",
                ProjectId = 1,
                AssigneeId = 2,
                DueDate = new DateTime(2026, 5, 20, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}