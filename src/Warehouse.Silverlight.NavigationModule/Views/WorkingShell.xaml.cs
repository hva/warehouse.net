using System.Reflection;

namespace Warehouse.Silverlight.NavigationModule.Views
{
    public partial class WorkingShell
    {
        public WorkingShell()
        {
            InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = new AssemblyName(assembly.FullName);
            var version = assemblyName.Version.ToString().TrimEnd(".0".ToCharArray());
            VersionTextBlock.Text = string.Concat("client v.", version);
        }
    }
}
