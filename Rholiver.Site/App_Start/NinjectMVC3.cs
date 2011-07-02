using System;
using System.Configuration;
using DotNetOpenAuth.OpenId.RelyingParty;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Mvc;
using PocoDb;
using PocoDb.Persistence.SqlServer;
using Rholiver.Site.Infrastructure;

[assembly: WebActivator.PreApplicationStartMethod(typeof (Rholiver.Site.App_Start.NinjectMVC3), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof (Rholiver.Site.App_Start.NinjectMVC3), "Stop")]

namespace Rholiver.Site.App_Start
{
    public static class NinjectMVC3
    {
        static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof (OnePerRequestModule));
            DynamicModuleUtility.RegisterModule(typeof (HttpApplicationInitializationModule));
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
        static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        static void RegisterServices(IKernel kernel)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["pocodb"].ConnectionString;

            kernel.Bind<IDbConnectionFactory>().ToConstant(new SqlServerConnectionFactory(connectionString));

            kernel.Bind<PocoDbClient>().ToSelf().InSingletonScope();

            kernel.Bind<OpenIdRelyingParty>().ToSelf().InSingletonScope();

            kernel.Bind<IUserProvider>().To<PocoDbUserProvider>().InSingletonScope();
        }
    }
}