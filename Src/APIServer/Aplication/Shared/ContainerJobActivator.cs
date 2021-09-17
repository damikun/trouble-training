
using System;
using Hangfire;

namespace APIServer.Aplication.Shared {

   public class ContainerJobActivator : JobActivator {
        private IServiceProvider _container;

        public ContainerJobActivator(IServiceProvider serviceProvider) {
            _container = serviceProvider;
        }

        public override object ActivateJob(Type type) {
            return _container.GetService(type);
        }
    }
}