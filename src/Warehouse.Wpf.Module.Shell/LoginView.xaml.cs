﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Warehouse.Wpf.Module.Shell
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, login);
        }
    }
}
