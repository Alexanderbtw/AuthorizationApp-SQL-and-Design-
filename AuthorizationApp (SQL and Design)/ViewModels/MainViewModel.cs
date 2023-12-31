﻿using AuthorizationApp__SQL_and_Design_.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AuthorizationApp__SQL_and_Design_.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
		private BaseViewModel _selectedViewModel = new HomeViewModel();

		public BaseViewModel SelectedViewModel
		{
			get { return _selectedViewModel; }
			set 
			{ 
				_selectedViewModel = value; 
				OnPropertyChange(nameof(SelectedViewModel));
			}
		}

        public ICommand UpdateViewCommand { get; set; }

		public MainViewModel()
		{
			UpdateViewCommand = new UpdateViewCommand(this);
		}
    }
}
