using Sakura.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sakura.View
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	[CLSCompliant(false)] // Because MetroWindow is not CLSCompliant[CLSCompliant(false)] // Because MetroWindow is not CLSCompliant
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			var hwnd = PresentationSource.FromVisual(this) as HwndSource;
			ApplicationContext.MainWindowSourceInitialize(hwnd);
		}

	}
}
