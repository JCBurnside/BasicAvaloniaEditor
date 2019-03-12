using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynamicData.Annotations;
using ReactiveUI;

namespace BasicAvaloniaEditor.ViewModels
{
	public class TextEditorViewModel : ViewModelBase
	{
		public FileInfo File { get; set; }
		//public FileTypes Type => Enum.TryParse<FileTypes>(File?.Extension, out var type) ? type : FileTypes.txt;

		public string FileName => File?.Name ?? "Untitled";

		public bool IsSaved { get; private set; } = true;

		private string content = null;

		public string Content
		{
			get
			{
				if(content == null && File != null)
				{
					using (var fs = File.OpenRead())
					using (var sr = new StreamReader(fs))
					{
						content = sr.ReadToEnd();
					}
				}
				content = content ?? string.Empty;
				return content;
			}
			set
			{
				if(content != value)
				{
					IsSaved = false;
				}
				this.RaiseAndSetIfChanged(ref content, value);
			}
		}

		public async Task SaveAsync()
		{
			if(!IsSaved)
			{
				using (var fs = File.OpenWrite())
				using (var sw = new StreamWriter(fs))
				{
					await sw.WriteAsync(Content);
				}
			}
		}

		public void Save()
		{
			SaveAsync().GetAwaiter().GetResult();
		}

		public void SaveAs(string dir)
		{
			File = new FileInfo(dir);
			Save();
		}

		public void Rename(string dir)
		{
			
		}

		public void Delete()
		{
			File.Delete();
			this.Content = null;
			this.File = null;
			this.IsSaved = true;
		}
	}

	//public enum FileTypes
	//{
	//	txt,
	//	text = txt,
	//	cs,
	//	csharp = cs,
	//	cpp,
	//	cc = cpp,
	//	hpp = cpp,
	//	c,
	//	h = cpp,
	//}
}
