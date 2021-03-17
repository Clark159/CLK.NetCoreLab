using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuartzLab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((services) =>
                {
                    // Quartz
                    services.AddQuartz(options =>
                    {
                        // Setting
                        options.UseMicrosoftDependencyInjectionScopedJobFactory();

                        // ScheduleJob
                        options.ScheduleJob<HelloWorldJob001>((trigger) =>
                        {
                            // Trigger
                            trigger.WithCronSchedule("0/5 * * * * ?");
                        });
                    });
                    services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

                    // ScheduleJob
                    services.Configure<QuartzOptions>((options) =>
                    {
                        var jobKey = JobKey.Create(nameof(HelloWorldJob002));
                        options.AddJob<HelloWorldJob002>(job => job.WithIdentity(jobKey));
                        options.AddTrigger(trigger =>
                        {
                            trigger
                                .WithIdentity($"{nameof(HelloWorldJob002)}-trigger")
                                .ForJob(jobKey)
                                .WithCronSchedule("0/5 * * * * ?")
                            ;
                        });
                    });
                    services.AddTransient<HelloWorldJob002>();
                })
                .ConfigureContainer<Autofac.ContainerBuilder>((container) =>
                {
                    // ScheduleJob
                    container.RegisterInstance<IConfigureOptions<QuartzOptions>>
                    (
                        new ConfigureNamedOptions<QuartzOptions>(Options.DefaultName, (options) =>
                        {
                            var jobKey = JobKey.Create(nameof(HelloWorldJob003));
                            options.AddJob<HelloWorldJob003>(job => job.WithIdentity(jobKey));
                            options.AddTrigger(trigger =>
                            {
                                trigger
                                    .WithIdentity($"{nameof(HelloWorldJob003)}-trigger")
                                    .ForJob(jobKey)
                                    .WithCronSchedule("0/5 * * * * ?")
                                ;
                            });
                        })
                    );
                    container.RegisterType<HelloWorldJob003>();
                });


        // Class
        public class DisplayJob : IJob
        {
            // Fields
            private readonly ILogger _logger = null;


            // Constructors
            public DisplayJob(ILogger logger)
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

                // Execute
                return Task.Run(() =>
                {
                    // Display
                    _logger.LogWarning(this.GetType().Name);
                });
            }
        }

        [DisallowConcurrentExecution]
        public class HelloWorldJob001 : DisplayJob
        {
            // Constructors
            public HelloWorldJob001(ILogger<HelloWorldJob001> logger) : base(logger) { }
        }

        [DisallowConcurrentExecution]
        public class HelloWorldJob002 : DisplayJob
        {
            // Constructors
            public HelloWorldJob002(ILogger<HelloWorldJob002> logger) : base(logger) { }
        }

        [DisallowConcurrentExecution]
        public class HelloWorldJob003 : DisplayJob
        {
            // Constructors
            public HelloWorldJob003(ILogger<HelloWorldJob003> logger) : base(logger) { }
        }
    }
}