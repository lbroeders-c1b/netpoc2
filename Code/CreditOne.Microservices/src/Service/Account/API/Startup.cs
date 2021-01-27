using System.Collections.Generic;

using AutoMapper;

using CreditOne.Microservices.Account.Domain;
using CreditOne.Microservices.Account.IDataProvider;
using CreditOne.Microservices.Account.OracleDataProvider;
using CreditOne.Microservices.Account.Repository;
using CreditOne.Microservices.Account.Service;
using CreditOne.Microservices.BuildingBlocks.Common.Configuration;
using CreditOne.Microservices.BuildingBlocks.Common.Configuration.Swagger;
using CreditOne.Microservices.BuildingBlocks.ExceptionFilters;
using CreditOne.Microservices.BuildingBlocks.LoggingFilter;
using CreditOne.Microservices.BuildingBlocks.OracleProvider.Core;
using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Factory;
using CreditOne.Microservices.BuildingBlocks.RequestValidationFilter.Filter;
using CreditOne.Microservices.BuildingBlocks.ServiceCollectionExtensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Swashbuckle.AspNetCore.Swagger;

namespace CreditOne.Microservices.Account.API
{
    /// <summary>
    /// Implements the startup class
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///		<term>Date</term>
    ///		<term>Who</term>
    ///		<term>BR/WO</term>
    ///		<description>Description</description>
    /// </listheader>
    /// <item>
    ///		<term>5/27/2019</term>
    ///		<term>Armando Soto</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>
    ///	</item>
    /// </list>
    /// </remarks>
    public class Startup
    {
        #region Private Members

        private readonly List<SwaggerConfiguration> _swaggerConfiguration;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for the Startup class.
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _swaggerConfiguration = Configuration.GetSection(nameof(SwaggerConfiguration)).Get<List<SwaggerConfiguration>>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        #endregion

        #region Public Methods 

        /// <summary>
        /// Gets called by the runtime. Use this method to add services to the container
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy-public",
                    builder => builder.AllowAnyOrigin()
                    .Build());
            });

            services.AddAutoMapper();
            services.AddDbConnectionStrings(Configuration);

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(InputParametersValidationFilter));
                options.Filters.Add(typeof(LogRequestResponseFilter));
                options.Filters.Add(typeof(BaseExceptionFilter));
            }).AddControllersAsServices()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbPerformanceMonitor(Configuration);

            services.AddSingleton<DatabaseFunctions>();
            services.AddApiVersioning();

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.IncludeXmlComments(SwaggerConfiguration.GetXmlCommentsPath("CreditOne.Microservices.Account.API.xml"));

                foreach (var swaggerConfiguration in _swaggerConfiguration)
                {
                    options.SwaggerDoc(swaggerConfiguration.Version,
                        new Info
                        {
                            Version = swaggerConfiguration.Version,
                            Title = "Account API",
                            Description = "Provides Account operations."
                        });
                }

                options.OperationFilter<CustomOperationFilter>();
                options.DocumentFilter<CustomDocumentFilter>();
                options.DocInclusionPredicate(SwaggerConfiguration.MapApiVersionAttributes());

                SwaggerConfiguration.SetBearerToken(options);
            });

            services.AddTransient<IAccountDataProvider, AccountOracleDataProvider>();
            services.AddTransient<IAddressDataProvider, AddressOracleDataProvider>();
            services.AddTransient<ICardDataProvider, CardOracleDataProvider>();
            services.AddTransient<IEmailDataProvider, EmailOracleDataProvider>();
            services.AddTransient<ICreditAccountDataProvider, CreditAccountOracleDataProvider>();
            services.AddTransient<ICustomerDataProvider, CustomerOracleDataProvider>();
            services.AddTransient<IPrimaryCardholderDataProvider, PrimaryCardholderOracleDataProvider>();
            services.AddTransient<ISecondaryCardholderDataProvider, SecondaryCardholderOracleDataProvider>();

            services.AddTransient<ICreditAccountRepository, CreditAccountRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();

            services.AddTransient<IAccountService, AccountService>();

            services.AddTransient<IValidatorFactory, ValidatorFactory>();
        }

        /// <summary>
        /// Gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Test"))
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger(option =>
                {
                    option.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.Host = httpReq.Host.Value;
                        swaggerDoc.Schemes = new List<string>() { httpReq.Scheme };
                        swaggerDoc.BasePath = httpReq.PathBase.HasValue ? httpReq.PathBase.Value : "/";
                    });
                });

                app.UseSwaggerUI(option =>
                {
                    foreach (var swaggerConfiguration in _swaggerConfiguration)
                    {
                        option.SwaggerEndpoint(swaggerConfiguration.UIEndpoint, swaggerConfiguration.Version);
                    }
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy-public");
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        #endregion
    }
}
