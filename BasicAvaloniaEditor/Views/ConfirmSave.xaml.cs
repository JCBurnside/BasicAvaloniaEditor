using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BasicAvaloniaEditor.Views
{
	public class ConfirmSave : Window
	{
		public ConfirmSave()
		{
			this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
