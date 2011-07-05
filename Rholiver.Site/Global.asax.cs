using System;
using System.Configuration;
using System.Data;
using System.Web.Mvc;
using System.Web.Routing;
using PocoDb;
using PocoDb.Persistence.SqlServer;
using Rholiver.Site.Infrastructure;
using Rholiver.Site.Models;

namespace Rholiver.Site
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            InitiateDbIfNotExists();
        }

        static void InitiateDbIfNotExists() {
            var connectionFactory = DependencyResolver.Current.GetService<IDbConnectionFactory>();

            using (var connection = connectionFactory.CreateOpenConnection()) {
                using (var command = connection.CreateCommand()) {
                    command.CommandText = @"Select * from sys.tables where name = 'SqlCommits'";

                    using (var result = command.ExecuteReader()) {
                        if (result.Read())
                            return;
                    }
                }

                using (var trans = connection.BeginTransaction(IsolationLevel.Serializable)) {
                    using (var command = connection.CreateCommand()) {
                        command.Transaction = trans;
                        command.CommandText =
                            @"CREATE TABLE [SqlCommits] ([Id] nvarchar(128) NOT NULL, [Value] nvarchar(MAX) NOT NULL);";

                        if (command.ExecuteNonQuery() == 0)
                            throw new InvalidOperationException("Failed to build db");
                    }
                    using (var command = connection.CreateCommand()) {
                        command.Transaction = trans;
                        command.CommandText =
                            @"ALTER TABLE [SqlCommits] ADD CONSTRAINT [PK__SqlCommits__Id] PRIMARY KEY ([Id]);";

                        if (command.ExecuteNonQuery() == 0)
                            throw new InvalidOperationException("Failed to build db");
                    }
                    using (var command = connection.CreateCommand()) {
                        command.Transaction = trans;
                        command.CommandText =
                            @"CREATE UNIQUE INDEX [UQ__SqlCommits__Id] ON [SqlCommits] ([Id] ASC);";

                        if (command.ExecuteNonQuery() == 0)
                            throw new InvalidOperationException("Failed to build db");
                    }

                    trans.Commit();
                }
            }

            InitialiseData();
        }

        static void InitialiseData() {
            var pocoDb = DependencyResolver.Current.GetService<PocoDbClient>();

            using (var session = pocoDb.BeginWritableSession()) {
                session.Add(new User {Id = GetAdminOpenId(), NickName = "Admin"});

                session.Add(new BlogPost() {
                                               Id = "first-post",
                                               Title = "First post",
                                               Body = "<p>This is my first post!</p>",
                                               CreatedAt = new DateTime(2011, 05, 14, 19, 42, 00)
                                           });

                session.Add(new BlogPost() {
                                               Id = "second-post",
                                               Title = "Second post",
                                               Body = "<p>This is my second post.</p><p>Lets try <b>styling</b>.</p>",
                                               CreatedAt = new DateTime(2011, 05, 14, 20, 01, 00)
                                           });

                session.SaveChanges();
            }
        }

        static string GetAdminOpenId() {
            var openid = ConfigurationManager.AppSettings["openid"];
            if (string.IsNullOrWhiteSpace(openid))
                throw new ApplicationException("No admin id found.");

            return openid;
        }
    }
}