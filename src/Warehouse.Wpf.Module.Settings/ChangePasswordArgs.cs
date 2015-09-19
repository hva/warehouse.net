using System.Windows.Controls;

namespace Warehouse.Wpf.Module.Settings
{
    public class ChangePasswordArgs
    {
        public ChangePasswordArgs(PasswordBox old, PasswordBox @new, PasswordBox new2)
        {
            Old = old;
            New = @new;
            New2 = new2;
        }

        public PasswordBox Old { get; private set; }
        public PasswordBox New { get; private set; }
        public PasswordBox New2 { get; private set; }
    }
}
