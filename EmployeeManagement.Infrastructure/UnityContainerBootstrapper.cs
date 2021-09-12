using EmployeeManagement.Application;
using EmployeeManagement.BusinessModel;
using EmployeeManagement.ServiceGateway;
using Unity;


namespace EmployeeManagement.Infrastructure
{
    public sealed class UnityContainerBootstrapper
    {
        private static IUnityContainer _container;
        private static object syncRoot = new object();


        /// <summary>
        /// Create Singleton instance for Unity Container
        /// </summary>
        public static IUnityContainer Container
        {
            get
            {
                if (_container == null)
                {
                    lock (syncRoot)
                    {
                        if (_container == null)
                        {
                            _container = new UnityContainer();
                            RegisterInterfacesWithConcreteImplementations();
                        }
                    }
                }
                return _container;
            }
        }

        private static void RegisterInterfacesWithConcreteImplementations()
        {
            _container.RegisterType<IEmployeeViewModel, EmployeeViewModel>(TypeLifetime.ContainerControlled);
            _container.RegisterType<IEmployeeServiceGateway, EmployeeServiceGateway>(TypeLifetime.ContainerControlled);
            _container.RegisterType<IAddEmployeeViewModel, AddEmployeeViewModel>(TypeLifetime.Transient);
            _container.RegisterType<IUpdateEmployeeViewModel, UpdateEmployeeViewModel>(TypeLifetime.ContainerControlled);
            _container.RegisterType<IDialogService, DialogService>(TypeLifetime.ContainerControlled);
        }
    }

}
