namespace Warehouse.Silverlight.UsersModule
{
    public partial class UsersView
    {
        public UsersView()
        {
            InitializeComponent();
        }

        public UsersView(UsersViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}
