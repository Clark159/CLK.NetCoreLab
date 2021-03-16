using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TemplateLab
{
    public class QuartzLab
    {
        // Methods
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((services) =>
                {
                    // Quartz
                    services.AddQuartz(options =>
                    {
                        // Setting
                        options.UseMicrosoftDependencyInjectionScopedJobFactory();

                        // Job
                        {
                            // Create a "key" for the job
                            var jobKey = new JobKey("HelloWorldJob");

                            // Register the job with the DI container
                            options.AddJob<HelloWorldJob>(opts => opts.WithIdentity(jobKey));

                            // Create a trigger for the job
                            options.AddTrigger(trigger => trigger
                                   .ForJob(jobKey) 
                                   .WithIdentity("HelloWorldJob-trigger")
                                   .WithCronSchedule("0/5 * * * * ?"));
                        }
                    });
                    services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
                });


        // Class
        [DisallowConcurrentExecution]
        public class HelloWorldJob : IJob
        {
            // Fields
            private readonly ILogger<HelloWorldJob> _logger = null;
                       

            // Constructors
            public HelloWorldJob(ILogger<HelloWorldJob> logger)
            {
                #region Contracts

                if (logger == null) throw new ArgumentException(nameof(logger));

                #endregion

                // Default
                _logger = logger;
            }


            // Methods
            public Task Execute(IJobExecutionContext context)
            {
                #region Contracts

                if (context == null) throw new ArgumentException(nameof(context));

                #endregion

                // Display
                _logger.LogWarning("Hello World!");

                // Return
                return Task.CompletedTask;
            }
        }
    }
}