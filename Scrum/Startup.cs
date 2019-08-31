using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scrum.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Scrum.Services;
using Scrum.Repositories;

namespace Scrum
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ScrumContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ScrumUser, ScrumRole>()
                .AddEntityFrameworkStores<ScrumContext>().AddDefaultUI().AddDefaultTokenProviders();

            
            services.AddScoped<UserRepository>();
            services.AddScoped<ProductRepository>();

            services.AddScoped<IAuthorizationHandler, ProductAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, BacklogItemAuthorizationHandler>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            CreateRoles(serviceProvider).Wait(); //nO suitable constructor found
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //adding custom roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<ScrumRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ScrumUser>>();

            IdentityResult roleResult;
            foreach (var role in Roles.getRoles())
            {
               var roleExist = await RoleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new ScrumRole(role));
                }
            }
            //creating a super user who could maintain the web app
            var poweruser = new ScrumUser
            {
                UserName = Configuration.GetSection("UserSettings")["UserEmail"],
                Email = Configuration.GetSection("UserSettings")["UserEmail"]
            };
            string UserPassword = Configuration.GetSection("UserSettings")["UserPassword"];
            var _user = await UserManager.FindByEmailAsync(Configuration.GetSection("UserSettings")["UserEmail"]);
            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, UserPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the "Admin" role 
                    await UserManager.AddToRoleAsync(poweruser, Roles.Admin);
                }
            }

            await InjectSampleData(serviceProvider);
        }

        private async Task InjectSampleData(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<ScrumRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ScrumUser>>();

            var jane = Configuration.GetSection("SampleData")["UserEmail"];
            var janepass = Configuration.GetSection("SampleData")["UserPassword"];
            var prodname = Configuration.GetSection("SampleData")["ProductName"];
            var proddesc = Configuration.GetSection("SampleData")["ProdDesc"];

            var janeuser = new ScrumUser
            {
                UserName = jane,
                Email = jane
            };

            var _user = await UserManager.FindByEmailAsync(jane);
            if (_user == null)
            {
                var createUser = await UserManager.CreateAsync(janeuser, janepass);

                if (createUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(janeuser, Roles.Product_Owner);

                    var product = new Product
                    {
                        Name = prodname,
                        Description = proddesc,
                        ProductManager = janeuser
                    };

                    var context = serviceProvider.GetService<ScrumContext>();
                    context.Products.Add(product);
                    context.SaveChanges();
                } else
                {
                    janeuser = null;
                }
            } else
            {

                var context = serviceProvider.GetService<ScrumContext>();
                var team = new ScrumTeam()
                {
                    TeamName = "Sample Team"
                };

                var exists = context.ScrumTeams.Where(sc => sc.TeamName == team.TeamName).Include(sc=> sc.UserTeams).ToList();

                if (exists == null || exists.Count == 0)
                {



                    var user_team = new ScrumUserTeam()
                    {
                        User = _user,
                        Team = team
                    };
                    context.ScrumTeams.Add(team);
                    context.ScrumUserTeams.Add(user_team);
                    context.SaveChanges();
                }
            }

        }
    }
}
