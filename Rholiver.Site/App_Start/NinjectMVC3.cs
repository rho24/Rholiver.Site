[assembly: WebActivator.PreApplicationStartMethod(typeof(Rholiver.Site.App_Start.NinjectMVC3), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Rholiver.Site.App_Start.NinjectMVC3), "Stop")]

namespace Rholiver.Site.App_Start
{
    using System.Reflection;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Mvc;
    using System.Collections.Generic;
    using Rholiver.Site.Models;
    using System;
    using Rholiver.Site.Infrastructure;

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
    }
}
