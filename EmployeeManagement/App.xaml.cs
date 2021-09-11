using System.ComponentModel.Composition;
using EmployeeManagement.Infrastructure;
using System.Windows;
using EmployeeManagement.Application;
using Unity;
using System.ComponentModel.Composition.Hosting;

namespace EmployeeManagement.Presentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App()
        {
            ShutdownMode = ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var container = UnityContainerBootstrapper.Container;
            var mainWindowViewModel = container.Resolve<IEmployeeViewModel>();
            var window = new MainWindow
            {
                DataContext = mainWindowViewModel
            };
            window.Show();
        }
        public void Compose()
        {
            AssemblyCatalog catalog = new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly());
            CompositionContainer container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
    }
}
