using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Core.Services;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Data.Repository;

namespace PizzeriaApi.Extensions
{
    public static class RegisterServicesExtensions
    {
        public static IServiceCollection AddServicesExtension(this IServiceCollection services)
        {
            //services and repos-------------

            services.AddScoped<IPizzeriaUserRepo, PizzeriaUserRepo>();
            services.AddScoped<IPizzeriaUserService, PizzeriaUserService>();

            services.AddScoped<IDishIngredientsRepo, DishIngredientsRepo>();
            services.AddScoped<IDishIngredientsService, DishIngredientsService>();

            services.AddScoped<IDishesRepo, DishesRepo>();
            services.AddScoped<IDishesService, DishesService>();

            services.AddScoped<IOrderItemsRepo, OrderItemsRepo>();
            services.AddScoped<IOrderItemsService, OrderItemsService>();

            services.AddScoped<IOrdersRepo, OrdersRepo>();
            services.AddScoped<IOrdersService, OrdersService>();

            services.AddScoped<IIngredientsRepo, IngredientsRepo>();
            services.AddScoped<IIngredientsService, IngredientsService>();

            services.AddScoped<ICategoriesRepo, CategoriesRepo>();
            services.AddScoped<ICategoriesService, CategoriesService>();

            services.AddTransient<ITokenGenerator, TokenGenerator>();

            //Services and repos-----------------

            return services;
        }
    }
}
