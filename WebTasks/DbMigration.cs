using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTasks.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebTasks
{
    public static class DbMigration
    {
        public static IWebHost MigrateDatabase(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();
            }

            return webHost;
        }
    }
}
