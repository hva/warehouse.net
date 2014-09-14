using System.Reflection;

namespace Warehouse.Silverlight
{
    public partial class Shell
    {
        public Shell()
        {
            InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = new AssemblyName(assembly.FullName);
            var version = assemblyName.Version.ToString().TrimEnd(".0".ToCharArray());
            VersionTextBlock.Text = string.Concat("v.", version);
        }
    }
}
