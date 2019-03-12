using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace DialogAction
{
	internal class ConfirmationDialogWindow : Window
	{
		public string Message { get; set; }

		public ConfirmationDialogWindow()
		{
			this.InitializeComponent();
			this.DataContext = this;
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
	internal enum ConfirmationTypes
	{
		Yes,
		No,
		Cancel
	}
}
