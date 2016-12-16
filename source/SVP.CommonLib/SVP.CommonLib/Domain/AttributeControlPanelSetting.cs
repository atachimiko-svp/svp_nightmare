using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVP.CommonLib.Domain
{
	/// <summary>
	/// 属性設定操作パネル情報
	/// </summary>
	[Serializable]
	public class AttributeControlPanelSetting
	{
		#region Private フィールド

		IList<TabItem> m_TabItems;
		IList<TabShortcutItem> m_TabShortcutItems;

		#endregion Private フィールド


		#region Public コンストラクタ

		public AttributeControlPanelSetting()
		{
			m_TabItems = new List<TabItem>();
			m_TabShortcutItems = new List<TabShortcutItem>();
		}

		#endregion Public コンストラクタ


		#region Public プロパティ

		public IList<TabItem> Tabs { get { return m_TabItems; } }

		public IList<TabShortcutItem> TabShortcutItems { get { return m_TabShortcutItems; } }

		#endregion Public プロパティ


		#region Public 内部クラス

		[Serializable]
		public class LabelItem
		{

			#region Public プロパティ

			public int Label_Id { get; set; }
			public string Name { get; set; }

			#endregion Public プロパティ
		}

		[Serializable]
		public class TabItem
		{

			#region Public プロパティ

			public string IdentifyKey { get; set; }
			public string Name { get; set; }
			public string TabShortcutKey { get; set; }
			public string TabShortcutKeyOnSelected { get; set; }

			#endregion Public プロパティ
		}

		[Serializable]
		public class TabShortcutItem
		{

			#region Public プロパティ

			public string IdentifyKey { get; set; }

			#endregion Public プロパティ
		}

		#endregion Public 内部クラス

	}
}
