﻿using System;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Pipeline;
using Rebus.Transport;

namespace Rebus.Workers.ThreadPoolBased
{
    /// <summary>
    /// Configuration extensions for experimental new thread pool-based worker factory
    /// </summary>
    public static class ThreadPoolWorkersConfigurationExtensions
    {
        /// <summary>
        /// Replaces the default worker factory with one that uses the .NET thread pool to dispatch messages
        /// </summary>
        [Obsolete("This configuration method is going away when thread pool-based workers become default")]
        public static void UseThreadPoolMessageDispatch(this OptionsConfigurer configurer)
        {
            if (configurer == null) throw new ArgumentNullException(nameof(configurer));

            configurer.Register<IWorkerFactory>(c =>
            {
                var transport = c.Get<ITransport>();
                var rebusLoggerFactory = c.Get<IRebusLoggerFactory>();
                var pipeline = c.Get<IPipeline>();
                var pipelineInvoker = c.Get<IPipelineInvoker>();
                var options = c.Get<Options>();
                var busLifetimeEvents = c.Get<BusLifetimeEvents>();
                var backoffStrategy = c.Get<ISyncBackoffStrategy>();
                return new ThreadPoolWorkerFactory(transport, rebusLoggerFactory, pipeline, pipelineInvoker, options, c.Get<RebusBus>, busLifetimeEvents, backoffStrategy);
            });
        }
    }
}