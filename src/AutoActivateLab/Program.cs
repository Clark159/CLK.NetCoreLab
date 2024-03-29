﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AutoActivateLab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)=>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<Autofac.ContainerBuilder>((container) =>
                {
                    // SettingContext
                    {
                        // Register
                        container.RegisterType<SettingContext>().As<SettingContext>()

                        // Start
                        .OnActivated((handler) =>
                        {
                            handler.Instance.Start();
                        })

                        // Lifetime
                        .AutoActivate().SingleInstance();
                    }
                });


        // Class
        public class SettingContext
        {
            // Methods
            public void Start()
            {
                // Execute
                Console.WriteLine("SettingContext Started");
            }
        }
    }
}