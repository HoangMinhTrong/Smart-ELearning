using Microsoft.Extensions.DependencyInjection;
using Smart_ELearning.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_ELearning.Services
{
    public static class DependencyInjection
    {
        public static void AddCatalogService(this IServiceCollection services)
        {
            services.AddTransient<IClassService, ClassService>();
        }
    }
}