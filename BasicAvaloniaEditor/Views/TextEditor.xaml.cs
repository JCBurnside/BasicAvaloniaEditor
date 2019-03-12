using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BasicAvaloniaEditor.Views
{
	public class TextEditor : UserControl
	{
		public TextEditor()
		{
			this.InitializeComponent();
			
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
