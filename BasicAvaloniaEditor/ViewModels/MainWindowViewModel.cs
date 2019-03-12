using Avalonia.Collections;
using Avalonia.Controls;
using BasicAvaloniaEditor.Views;
using ReactiveUI;
using ReactiveUI.Legacy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;

namespace BasicAvaloniaEditor.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		public ReactiveCommand<TextEditorViewModel, Unit> Close { get; private set; }
		public MainWindowViewModel()
		{
			Close = ReactiveCommand.Create<TextEditorViewModel>(Close_impl);
			OpenFiles.CollectionChanged += OpenFiles_CollectionChanged;
			//CurrentTextBox_PropertyChanged = ReactiveCommand.Create<Avalonia.AvaloniaPropertyChangedEventArgs>();
		}

		public void New()
		{
			OpenFiles.Add(new TextEditorViewModel { });
		}

		public void Save()
		{
			CurrentFile.Save();
		}

		public async void SaveAll()
		{
			foreach (var file in OpenFiles)
			{
				if (file.File != null)
				{
					file.Save();
				}
				else
				{
					file.SaveAs(await new SaveFileDialog().ShowAsync(Avalonia.Application.Current.Windows.Where(w => ReferenceEquals(w.DataContext, this)).SingleOrDefault()));
				}
			}
		}

		public async void SaveAs()
		{
			string file = await new SaveFileDialog().ShowAsync(Avalonia.Application.Current.Windows.Where(w => ReferenceEquals(w.DataContext, this)).SingleOrDefault());
			CurrentFile.SaveAs(file);
		}

		private void OpenFiles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			this.RaisePropertyChanged(nameof(OpenFiles));
			if (currentFile is null ||
			e.OldItems?.Count == 0 ||
			e.NewItems?.Count == 0)
			{
				this.RaisePropertyChanged(nameof(CurrentFile));
			}
		}

		private void currentTextBox_PropertyChanged(Avalonia.AvaloniaPropertyChangedEventArgs args)
		{
			if (args.Sender is TextBox box && args.Property == TextBox.TextProperty && args.NewValue is int newIndex)
			{
				CurrentTextBox_CarretIndexChanged(box, newIndex);
			}
		}

		private TextEditorViewModel currentFile;
		public TextEditorViewModel CurrentFile
		{
			get
			{
				if (currentFile is null)
				{
					currentFile = OpenFiles.FirstOrDefault();
				}
				return currentFile;
			}
			set
			{
				this.RaiseAndSetIfChanged(ref currentFile, value);
			}
		}

		private async void Close_impl(TextEditorViewModel toClose)
		{
			if (!toClose.IsSaved)
			{
				Window target = Avalonia.Application.Current.Windows.Where(w => ReferenceEquals(w.DataContext, this)).Single();
				if (toClose.File is null)
				{

				}
			}
			OpenFiles.Remove(toClose);
		}

		public string Greeting => "Hello World!";

		public ReactiveCommand<Avalonia.AvaloniaPropertyChangedEventArgs, Unit> CurrentTextBox_PropertyChanged { get; }

		public AvaloniaList<TextEditorViewModel> OpenFiles { get; set; } = new AvaloniaList<TextEditorViewModel> { new TextEditorViewModel { Content = "Hello" } };

		public TextEditorViewModel SelectedFile { get; set; }

		public int CurrentIndex { get; set; }

		public int Line { get; set; }

		public int Col { get; set; }

		public string Pos { get; private set; }

		public void CurrentTextBox_CarretIndexChanged(TextBox textBox, int newCarret)
		{
			CurrentIndex = newCarret;
			Line = textBox.GetLineIndexFromCharacterIndex(newCarret);
			Col = newCarret - textBox.GetCharacterIndexFromLineIndex(Line);
			Pos = $"Line: {Line}, Col: {Col}";
		}
	}

	public static class TextBoxExtensions
	{
		public static int GetLineIndexFromCharacterIndex(this TextBox self, int index)
		{
			return self.Text.Substring(0, index).Count(c => c == '\n');
		}

		public static int GetCharacterIndexFromLineIndex(this TextBox self, int index)
		{
			return self.Text.IndexOf('\n', index);
		}
	}
}
