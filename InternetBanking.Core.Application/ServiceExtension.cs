using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InternetBanking.Core.Application
{
    public static class ServiceExtension
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            #region Services
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<IBeneficiaryService, BeneficiaryService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<ISavingAccountService, SavingAccountService>();
            services.AddSingleton<SerialGenerator, SerialGenerator>();
            #endregion
        }
    }
}