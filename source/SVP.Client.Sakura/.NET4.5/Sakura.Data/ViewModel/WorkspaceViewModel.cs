using log4net;
using Sakura.Core.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sakura.Core.Presentation;
using Sakura.Core;
using System.Windows;

namespace Sakura.Data.ViewModel
{
	public class WorkspaceViewModel : Livet.ViewModel, IWorkspaceViewModel
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(WorkspaceViewModel));

		/// <summary>
		/// ドキュメント部のViewModel
		/// </summary>
		SDocumentViewModelBase _ActiveDocument;

		DataTemplate _DataTemplate_DockLeft;

		DataTemplate _DataTemplate_DockRight;

		/// <summary>
		/// Flyout(Button_1)のDataTemplate
		/// </summary>
		DataTemplate _DataTemplate_FB1;

		/// <summary>
		/// Flyout(Button_2)のDataTemplate
		/// </summary>
		DataTemplate _DataTemplate_FB2;

		/// <summary>
		/// Flyout(Left_1)のDataTemplate
		/// </summary>
		DataTemplate _DataTemplate_FL1;

		/// <summary>
		/// Flyout(Left_2)のDataTemplate
		/// </summary>
		DataTemplate _DataTemplate_FL2;

		/// <summary>
		/// Flyout(Right_1)のDataTemplate
		/// </summary>
		DataTemplate _DataTemplate_FR1;

		/// <summary>
		/// Flyout(Right_2)のDataTemplate
		/// </summary>
		DataTemplate _DataTemplate_FR2;

		PaneViewModelBase _ViewModel_DockLeft;

		PaneViewModelBase _ViewModel_DockRight;

		PaneViewModelBase _ViewModel_FB1;

		PaneViewModelBase _ViewModel_FB2;

		PaneViewModelBase _ViewModel_FL1;

		PaneViewModelBase _ViewModel_FL2;

		PaneViewModelBase _ViewModel_FR1;

		PaneViewModelBase _ViewModel_FR2;

		/// <summary>
		/// ドキュメント部のDataTemplate
		/// </summary>
		DataTemplate _ViewTemplate;

		#endregion フィールド


		#region プロパティ

		public SDocumentViewModelBase ActiveDocument
		{
			get { return _ActiveDocument; }
			private set
			{
				_ActiveDocument = value;
				UpdateActiveViewTemplate();
				RaisePropertyChanged();
			}
		}

		public DataTemplate DockLeft
		{
			get
			{
				return _DataTemplate_DockLeft;
			}
			protected set
			{
				_DataTemplate_DockLeft = value;
				RaisePropertyChanged();
			}
		}

		public PaneViewModelBase DockLeftViewModel
		{
			get
			{
				return _ViewModel_DockLeft;
			}
			protected set
			{
				_ViewModel_DockLeft = value;
				UpdateDockLeftTemplate();
				RaisePropertyChanged();
			}
		}

		public DataTemplate DockRight
		{
			get
			{
				return _DataTemplate_DockRight;
			}
			protected set
			{
				_DataTemplate_DockRight = value;
				RaisePropertyChanged();
			}
		}

		public PaneViewModelBase DockRightViewModel
		{
			get
			{
				return _ViewModel_DockRight;
			}
			protected set
			{
				_ViewModel_DockRight = value;
				UpdateDockRightTemplate();
				RaisePropertyChanged();
			}
		}
		public DataTemplate FB1
		{
			get
			{
				return _DataTemplate_FB1;
			}
			protected set
			{
				_DataTemplate_FB1 = value;
				RaisePropertyChanged();
			}
		}

		public PaneViewModelBase FB1ViewModel
		{
			get { return _ViewModel_FB1; }
			private set
			{
				_ViewModel_FB1 = value;
				UpdateFB1Template();
				RaisePropertyChanged();
			}
		}

		public DataTemplate FB2
		{
			get
			{
				return _DataTemplate_FB2;
			}
			protected set
			{
				_DataTemplate_FB2 = value;
				RaisePropertyChanged();
			}
		}

		public PaneViewModelBase FB2ViewModel
		{
			get { return _ViewModel_FB2; }
			private set
			{
				_ViewModel_FB2 = value;
				UpdateFB2Template();
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// ドキュメントに表示するテンプレートを取得します。
		/// </summary>
		public DataTemplate FL1
		{
			get
			{
				return _DataTemplate_FL1;
			}
			protected set
			{
				_DataTemplate_FL1 = value;
				RaisePropertyChanged();
			}
		}

		public PaneViewModelBase FL1ViewModel
		{
			get { return _ViewModel_FL1; }
			private set
			{
				_ViewModel_FL1 = value;
				UpdateFL1Template();
				RaisePropertyChanged();
			}
		}

		public DataTemplate FL2
		{
			get
			{
				return _DataTemplate_FL2;
			}
			protected set
			{
				_DataTemplate_FL2 = value;
				RaisePropertyChanged();
			}
		}

		public PaneViewModelBase FL2ViewModel
		{
			get { return _ViewModel_FL2; }
			private set
			{
				_ViewModel_FL2 = value;
				UpdateFL2Template();
				RaisePropertyChanged();
			}
		}

		public DataTemplate FR1
		{
			get
			{
				return _DataTemplate_FR1;
			}
			protected set
			{
				_DataTemplate_FR1 = value;
				RaisePropertyChanged();
			}
		}

		public PaneViewModelBase FR1ViewModel
		{
			get { return _ViewModel_FR1; }
			private set
			{
				_ViewModel_FR1 = value;
				UpdateFR1Template();
				RaisePropertyChanged();
			}
		}

		public DataTemplate FR2
		{
			get
			{
				return _DataTemplate_FR2;
			}
			protected set
			{
				_DataTemplate_FR2 = value;
				RaisePropertyChanged();
			}
		}

		public PaneViewModelBase FR2ViewModel
		{
			get { return _ViewModel_FR2; }
			private set
			{
				_ViewModel_FR2 = value;
				UpdateFR2Template();
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// ドキュメントに表示するテンプレートを取得します。
		/// </summary>
		public DataTemplate ViewTemplate
		{
			get
			{
				return _ViewTemplate;
			}
			protected set
			{
				_ViewTemplate = value;
				RaisePropertyChanged();
			}
		}

		#endregion プロパティ


		#region メソッド

		public void AttachViewModel(string position, PerspectiveViewModelBase perspectiveVm)
		{
			switch (position)
			{
				case "Document":
					this.ActiveDocument = (SDocumentViewModelBase)perspectiveVm;
					break;
				case "DockLeft":
					this.DockLeftViewModel = perspectiveVm as PaneViewModelBase;
					break;
				case "DockRight":
					this.DockRightViewModel = perspectiveVm as PaneViewModelBase;
					break;
				case "FL1":
					this.FL1ViewModel = perspectiveVm as PaneViewModelBase;
					break;
				case "FL2":
					this.FL2ViewModel = perspectiveVm as PaneViewModelBase;
					break;
				case "FR1":
					this.FR1ViewModel = perspectiveVm as PaneViewModelBase;
					break;
				case "FR2":
					this.FR2ViewModel = perspectiveVm as PaneViewModelBase;
					break;
				case "FB1":
					this.FB1ViewModel = perspectiveVm as PaneViewModelBase;
					break;
				case "FB2":
					this.FB2ViewModel = perspectiveVm as PaneViewModelBase;
					break;
			}
		}

		public void OpenFlyout()
		{
			if (this.FL1ViewModel != null)
				this.FL1ViewModel.IsOpen = true;
		}

		public void Perspective1()
		{
			StartPerspective("P1");
		}

		public void Perspective2()
		{
			StartPerspective("P2");
		}

		void StartPerspective(string perspectiveName)
		{
			ApplicationContext.Ux.ChangePerspective(perspectiveName);
		}

		/// <summary>
		/// 表示するDataTemplateを更新します
		/// </summary>
		private void UpdateActiveViewTemplate()
		{
			if (this.ActiveDocument == null)
			{
				this.ViewTemplate = null;
			}
			else
			{
				var name = this.ActiveDocument.GetType().Name;
				this.ViewTemplate = ApplicationContext.MainWindow.FindResource(name) as DataTemplate;
			}
		}

		private void UpdateDockLeftTemplate()
		{
			if (this.DockLeftViewModel == null)
			{
				this.DockLeft = null;
			}
			else
			{
				var name = this.DockLeftViewModel.GetType().Name;
				this.DockLeft = ApplicationContext.MainWindow.FindResource(name) as DataTemplate;
			}
		}

		private void UpdateDockRightTemplate()
		{
			if (this.DockRightViewModel == null)
			{
				this.DockRight = null;
			}
			else
			{
				var name = this.DockRightViewModel.GetType().Name;
				this.DockRight = ApplicationContext.MainWindow.FindResource(name) as DataTemplate;
			}
		}

		/// <summary>
		/// 表示するDataTemplateを更新します
		/// </summary>
		private void UpdateFB1Template()
		{
			if (this.FB1ViewModel == null)
			{
				this.FB1 = null;
			}
			else
			{
				var name = this.FB1ViewModel.GetType().Name;
				this.FB1 = ApplicationContext.MainWindow.FindResource(name) as DataTemplate;
			}
		}

		/// <summary>
		/// 表示するDataTemplateを更新します
		/// </summary>
		private void UpdateFB2Template()
		{
			if (this.FB2ViewModel == null)
			{
				this.FB2 = null;
			}
			else
			{
				var name = this.FB2ViewModel.GetType().Name;
				this.FB2 = ApplicationContext.MainWindow.FindResource(name) as DataTemplate;
			}
		}

		/// <summary>
		/// 表示するDataTemplateを更新します
		/// </summary>
		private void UpdateFL1Template()
		{
			if (this.FL1ViewModel == null)
			{
				this.FL1 = null;
			}
			else
			{
				var name = this.FL1ViewModel.GetType().Name;
				this.FL1 = ApplicationContext.MainWindow.FindResource(name) as DataTemplate;
			}
		}

		/// <summary>
		/// 表示するDataTemplateを更新します
		/// </summary>
		private void UpdateFL2Template()
		{
			if (this.FL2ViewModel == null)
			{
				this.FL2 = null;
			}
			else
			{
				var name = this.FL2ViewModel.GetType().Name;
				this.FL2 = ApplicationContext.MainWindow.FindResource(name) as DataTemplate;
			}
		}

		/// <summary>
		/// 表示するDataTemplateを更新します
		/// </summary>
		private void UpdateFR1Template()
		{
			if (this.FR1ViewModel == null)
			{
				this.FR1 = null;
			}
			else
			{
				var name = this.FR1ViewModel.GetType().Name;
				this.FR1 = ApplicationContext.MainWindow.FindResource(name) as DataTemplate;
			}
		}

		/// <summary>
		/// 表示するDataTemplateを更新します
		/// </summary>
		private void UpdateFR2Template()
		{
			if (this.FR2ViewModel == null)
			{
				this.FR2 = null;
			}
			else
			{
				var name = this.FR2ViewModel.GetType().Name;
				this.FR2 = ApplicationContext.MainWindow.FindResource(name) as DataTemplate;
			}
		}

		#endregion メソッド

	}
}
