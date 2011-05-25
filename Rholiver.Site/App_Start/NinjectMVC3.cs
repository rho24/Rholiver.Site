[assembly: WebActivator.PreApplicationStartMethod(typeof(Rholiver.Site.App_Start.NinjectMVC3), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Rholiver.Site.App_Start.NinjectMVC3), "Stop")]

namespace Rholiver.Site.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Web.Hosting;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Mvc;
    using Rholiver.Site.Infrastructure;
    using Rholiver.Site.Models;
    using DotNetOpenAuth.OpenId.RelyingParty;

    public static class NinjectMVC3 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestModule));
            DynamicModuleUtility.RegisterModule(typeof(HttpApplicationInitializationModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IUserProvider>().ToConstant(new InMemoryUserProvider(new[] { new User { Id = GetAdminOpenId(), NickName = "Admin" } }));

            kernel.Bind<OpenIdRelyingParty>().ToSelf().InSingletonScope();

            kernel.Bind<BlogRepository>().ToConstant(new BlogRepository(new List<BlogPost>()
            {
                new BlogPost(){
                    Id = "first-post",
                    Title = "First post",
                    Body = "<p>This is my first post!</p>",
                    CreatedAt = new DateTime(2011, 05, 14, 19, 42, 00)
                },
                new BlogPost(){
                    Id = "second-post",
                    Title = "Second post",
                    Body = "<p>This is my second post.</p><p>Lets try <b>styling</b>.</p>",
                    CreatedAt = new DateTime(2011, 05, 14, 20, 01, 00)
                }
            })).InTransientScope();

        }

        static string GetAdminOpenId()
        {
            var openid = ConfigurationManager.AppSettings["openid"];
            if (!string.IsNullOrWhiteSpace(openid))
                return openid;
            else
            {
                openid = File.ReadAllText(HostingEnvironment.MapPath("~/App_Data/openid.txt"));
                if (string.IsNullOrWhiteSpace(openid))
                    throw new ApplicationException("AdminOpenId is not set.");

                return openid;
            }
        }
    }
}
