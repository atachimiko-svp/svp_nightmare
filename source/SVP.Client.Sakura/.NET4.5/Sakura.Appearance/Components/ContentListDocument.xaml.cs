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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sakura.View.Components
{
	/// <summary>
	/// ContentListDocument.xaml の相互作用ロジック
	/// </summary>
	public partial class ContentListDocument : UserControl
	{
		public ContentListDocument()
		{
			InitializeComponent();

			var l = (ViewBase)(listView.TryFindResource("IconView")); ;
			listView.View = l;
		}
	}
}
