using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Sakura.View.Behaviors
{
	public class AutomaticUnloadIndivisualBehavior : Behavior<ListView>
	{
		#region フィールド

		/// <summary>
		/// 
		/// </summary>
		Timer _ScrollingTimer;

		ScrollViewer scrollViewer;

		#endregion フィールド


		#region メソッド

		public void Scrolling()
		{
			this._ScrollingTimer.Stop();
			this._ScrollingTimer.Start();
		}

		protected override void OnAttached()
		{
			base.OnAttached();

			this._ScrollingTimer = new Timer();
			this._ScrollingTimer.Interval = 5000; // TODO: 適切な値を設定
			this._ScrollingTimer.Elapsed += OnScrollingTimer_Elapsed;
			this._ScrollingTimer.AutoReset = false;

			AssociatedObject.Loaded += AssociatedObjectOnLoaded;
			AssociatedObject.Unloaded += AssociatedObjectOnUnloaded;
		}

		protected override void OnDetaching()
		{
			this._ScrollingTimer.Stop();
			this._ScrollingTimer = null;

			AssociatedObject.Loaded -= AssociatedObjectOnLoaded;
			AssociatedObject.Unloaded -= AssociatedObjectOnUnloaded;
		}

		private static ScrollViewer GetScrollViewer(DependencyObject root)
		{
			int childCount = VisualTreeHelper.GetChildrenCount(root);
			for (int i = 0; i < childCount; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(root, i);
				ScrollViewer sv = child as ScrollViewer;
				if (sv != null)
					return sv;

				return GetScrollViewer(child);
			}
			return null;
		}

		private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			scrollViewer = GetScrollViewer(AssociatedObject);
			if (scrollViewer != null)
			{
				scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;
			}

			this._ScrollingTimer.Start();
		}

		private void AssociatedObjectOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
		{
			if (scrollViewer != null)
			{
				scrollViewer.ScrollChanged -= ScrollViewerOnScrollChanged;
			}
		}


		async void OnScrollingTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			try
			{
				Dispatcher.Invoke(new Action(() =>
				{
					if (AssociatedObject != null)
					{
						IItemContainerGenerator generator = AssociatedObject.ItemContainerGenerator;
						for (int index = 0; index < AssociatedObject.Items.Count; index++)
						{
							GeneratorPosition position = generator.GeneratorPositionFromIndex(index);
							if (position.Offset != 0)
							{
								dynamic d = AssociatedObject.Items[index];
								d.Unload();
							}
						}
					}
				}));
			}
			catch (Exception expr)
			{

			}
		}

		private void ScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			Scrolling();
		}

		#endregion メソッド
	}
}
