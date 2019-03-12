using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactivity;
using ReactiveUI;
using System;

namespace DialogAction
{
	public class ConfirmationAction : AvaloniaObject, IAction
	{
		[Content]
		public string Message
		{
			get => GetValue(MessageProperty);
			set => SetValue(MessageProperty, value);
		}

		private ActionCollection yesActions;
		public ActionCollection Yes
		{
			get
			{
				if (yesActions == null)
					yesActions = new ActionCollection();
				return yesActions;
			}
		}

		private ActionCollection noActions;
		public ActionCollection No
		{
			get
			{
				if (noActions == null)
					noActions = new ActionCollection();
				return noActions;
			}
		}

		private ActionCollection cancelActions;
		public ActionCollection Cancel
		{
			get
			{
				if (cancelActions == null)
					cancelActions = new ActionCollection();

				return cancelActions;
			}
		}

		private Popup popup;

		public static readonly AvaloniaProperty<string> MessageProperty =
			AvaloniaProperty.Register<ConfirmationAction, string>(nameof(Message));

		public static readonly AvaloniaProperty<ActionCollection> YesProperty =
			AvaloniaProperty.RegisterDirect<ConfirmationAction, ActionCollection>(nameof(Yes), t => t.Yes);

		public static readonly AvaloniaProperty<ActionCollection> NoProperty =
			AvaloniaProperty.RegisterDirect<ConfirmationAction, ActionCollection>(nameof(No), t => t.No);

		public static readonly AvaloniaProperty<ActionCollection> CancelProperty =
			AvaloniaProperty.RegisterDirect<ConfirmationAction, ActionCollection>(nameof(Cancel), t => t.Cancel);

		public object Execute(object sender, object parameter)
		{
			//if(popup == null)
			//{
			//	popup = new Popup()
			//	{
			//		PlacementMode = PlacementMode.Pointer,
			//		PlacementTarget = sender as Control,
			//		StaysOpen = false
			//	};
			//}
			var dialog = new ConfirmationDialogWindow
			{
				Message = Message
			};
			IControl c = sender as Control;
			while (c != null && !(c is Window))
			{
				c = c.Parent;
			}
			if (c != null)
			{ 
				switch (dialog.ShowDialog<ConfirmationTypes>(c as Window).ConfigureAwait(false).GetAwaiter().GetResult())
				{
					case ConfirmationTypes.Yes:
						Interaction.ExecuteActions(sender, yesActions, parameter);
						break;
					case ConfirmationTypes.No:
						Interaction.ExecuteActions(sender, noActions, parameter);
						break;
					default:
						Interaction.ExecuteActions(sender, cancelActions, parameter);
						break;
				}
			}
			//popup.Open();
			return null;
		}
	}
}
