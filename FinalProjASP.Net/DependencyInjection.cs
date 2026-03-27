using Domain.Abstraction;
using Domain.Services.Applications;
using Domain.Services.Auth;
using Domain.Services.Jobs;
using Domain.Services.Users;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Storage;
using Storage.Repositories;

namespace FinalProjASP.Net;

public static class DependencyInjection
{
    public static IServiceCollection AddFinalProjServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<UsersService>();
        services.AddScoped<JobService>();
        services.AddScoped<ApplicationService>();
        services.AddTransient<JwtTokenGenerator>();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return services;
    }
}