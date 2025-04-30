using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace ManejoPresupuesto.Servicios.Extencion
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyServices(this IServiceCollection services)
        {
            var politicaUsuariosAutenticados = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            // Add services to the container.
            services.AddControllersWithViews(opciones =>
            {
                opciones.Filters.Add(new AuthorizeFilter(politicaUsuariosAutenticados));
            });
            services.AddTransient<IRepositorioTiposCuentas, RepositorioTiposCuentas>();
            services.AddTransient<IServiciousuarios, ServicioUsuarios>();
            services.AddTransient<IRepositorioCuentas, RepositorioCuentas>();
            services.AddTransient<IRepositorioCategoria, RepositorioCategoria>();
            services.AddTransient<IRepositorioTransacciones, RepositorioTransacciones>();
            services.AddTransient<IServicioReportes, ServicioReportes>();
            services.AddTransient<IRepositorioUsuarios, RepositorioUsuarios>();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(Program));
            services.AddTransient<IUserStore<Usuario>, UsuarioStore>();
            services.AddTransient<SignInManager<Usuario>>();
            services.AddIdentityCore<Usuario>(opciones =>
            {
                opciones.Password.RequireDigit = false;
                opciones.Password.RequireLowercase = false;
                opciones.Password.RequireUppercase = false;
                opciones.Password.RequireNonAlphanumeric = false;
            }).AddErrorDescriber<MensajesErrorIdentity>().AddDefaultTokenProviders();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
            }).AddCookie(IdentityConstants.ApplicationScheme, opciones =>
            {
                opciones.LoginPath = "/usuarios/login";
            });

            services.AddTransient<IServicioEmail, ServicioEmail>();


            return services;
        }
    }
}
