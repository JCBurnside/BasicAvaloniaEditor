using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using Serilog;

namespace BasicAvaloniaEditor
{
	public class App : Application
	{
		public override void Initialize()
		{
			InitializeLogging();
			AvaloniaXamlLoader.Load(this);
		}

		private void InitializeLogging()
		{
			SerilogLogger.Initialize(new LoggerConfiguration()
			.MinimumLevel.Information()
			.WriteTo.Debug()
			.CreateLogger());
		}
	}
}
