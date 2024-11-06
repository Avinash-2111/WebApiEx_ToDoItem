using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
using WebApiEx1.Models;

namespace WebApiEx1.Context
{
    public class ToDoContext:DbContext
    {
        public ToDoContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
