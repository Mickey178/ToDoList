using System.Data.Entity;

namespace ToDoList
{
    class ApplicationContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public ApplicationContext() : base("DefaultConnection") { }
    }
}
