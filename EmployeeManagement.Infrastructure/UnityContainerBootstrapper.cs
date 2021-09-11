using EmployeeManagement.Application;
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
            //_container.RegisterType<IPropertyRepository, PropertyRepository>(TypeLifetime.ContainerControlled);
            //_container.RegisterType<IDatabaseGateway, DatabaseGateway>(TypeLifetime.ContainerControlled);
            //_container.RegisterType<IConnectionString, ConnectionString>(TypeLifetime.ContainerControlled);
            //_container.RegisterType<IAddEntityDialogViewModel, AddEntityDialogViewModel>(); //Transient design pattern as per design choice
            //_container.RegisterType<IPropertiesDialogViewModel, PropertiesDialogViewModel>();//Transient design pattern as per design choice
            //_container.RegisterType<IEventAggregator, EventAggregator>(TypeLifetime.Singleton); //Its always singleton as per design
            //_container.RegisterType<IEntityFactory, EntityFactory>(); //Transient
            //_container.RegisterType<IPropertyFactory, PropertyFactory>(); // Transient

        }
    }

}
