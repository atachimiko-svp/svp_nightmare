using Sakura.Core;
using Sakura.Core.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sakura.Data.Infrastructures
{
	public abstract class MDFViewModelBase : PaneViewModelBase
	{

		#region フィールド

		DataTemplate _PropertyDataTemplate;

		MDFPropertyViewModelBase _PropertyViewModel;

		#endregion フィールド


		#region プロパティ

		public MDFPropertyViewModelBase Property
		{
			get { return _PropertyViewModel; }
			private set
			{
				_PropertyViewModel = value;
				UpdatePropertyTemplate();
				RaisePropertyChanged();
			}
		}

		public DataTemplate PropertyDataTemplate
		{
			get { return _PropertyDataTemplate; }
			private set
			{
				_PropertyDataTemplate = value;
				RaisePropertyChanged();
			}
		}

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// 表示するDataTemplateを更新します
		/// </summary>
		private void UpdatePropertyTemplate()
		{
			if (this.Property == null)
			{
				this.PropertyDataTemplate = null;
			}
			else
			{
				var name = this.Property.GetType().Name;
				this.PropertyDataTemplate = ApplicationContext.MainWindow.FindResource(name) as DataTemplate;
			}
		}

		#endregion メソッド
	}
}
