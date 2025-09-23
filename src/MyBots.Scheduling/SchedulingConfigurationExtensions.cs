using Microsoft.Extensions.DependencyInjection;
using MyBots.Scheduling.Jobs;
using Quartz;

namespace MyBots.Scheduling
{
    public static class SchedulingConfigurationExtensions
    {
        /// <summary>
        /// Adds scheduling services to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <returns>The service collection for chaining.</returns>
        public static IServiceCollection AddScheduling(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                // Register the job
                var jobKey = new JobKey("SendMessageJob");
                q.AddJob<SendMessageJob>(opts => opts.WithIdentity(jobKey));
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            services.AddScoped<IScheduleManager, QuartzScheduleManager>();

            return services;
        }
    }
}