using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todolist.Entities
{
    public static class DbInitializer
    {
        public static IHost InitData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                InitializeAsync(serviceProvider).GetAwaiter().GetResult();
            }
            return host;
        }

        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<TodoDbContext>();

            context.Database.EnsureCreated();

            // Seed Todo
            if (!context.Todos.Any())
            {
                var todos = new List<Todo>
                {
                    new Todo
                    {
                        Title = "Become a rock star developer",
                        Description = "Do it now!!!"
                    },
                   new Todo
                   {
                       Title = "Take fucking action",
                       Description = "Do it now!!!"
                   }
                };
                context.Todos.AddRange(todos);
                await context.SaveChangesAsync();
            };
        }
    }
}
