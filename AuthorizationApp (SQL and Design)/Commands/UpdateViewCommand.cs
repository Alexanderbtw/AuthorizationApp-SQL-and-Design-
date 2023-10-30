using AuthorizationApp__SQL_and_Design_.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AuthorizationApp__SQL_and_Design_.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter?.ToString() == "Home")
                viewModel.SelectedViewModel = new HomeViewModel();
            else if (parameter?.ToString() == "Settings")
                viewModel.SelectedViewModel = new SettingsViewModel();
            else if (parameter?.ToString() == "News")
                viewModel.SelectedViewModel = new NewsViewModel();
        }
    }
}
