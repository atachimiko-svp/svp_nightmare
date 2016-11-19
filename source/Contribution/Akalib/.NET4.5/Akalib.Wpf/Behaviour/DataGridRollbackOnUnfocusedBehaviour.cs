﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Input;

namespace Akalib.Wpf.Behaviour
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// http://social.msdn.microsoft.com/Forums/en-US/wpf/thread/187b2b8f-d403-4bf3-97ad-7f93b4385cdf/#fef4d2ca-d6ea-4043-a2ea-3e54fa218068
	/// 
	/// http://www.srcw.net/wiki/index.php?WPF%2FDataGrid#ca582bcf
	/// </remarks>
	public class DataGridRollbackOnUnfocusedBehaviour
	{
		#region DataGridRollbackOnUnfocusedBehaviour

		public static bool GetDataGridRollbackOnUnfocused(DataGrid datagrid)
		{
			return (bool)datagrid.GetValue(DataGridRollbackOnUnfocusedProperty);
		}

		public static void SetDataGridRollbackOnUnfocused(
		 DataGrid datagrid, bool value)
		{
			datagrid.SetValue(DataGridRollbackOnUnfocusedProperty, value);
		}

		public static readonly DependencyProperty DataGridRollbackOnUnfocusedProperty =
			DependencyProperty.RegisterAttached(
			"DataGridRollbackOnUnfocused",
			typeof(bool),
			typeof(DataGridRollbackOnUnfocusedBehaviour),
			new UIPropertyMetadata(false, OnDataGridRollbackOnUnfocusedChanged));

		static void OnDataGridRollbackOnUnfocusedChanged(
		 DependencyObject depObj, DependencyPropertyChangedEventArgs e)
		{
			DataGrid datagrid = depObj as DataGrid;
			if (datagrid == null)
				return;

			if (e.NewValue is bool == false)
				return;

			if ((bool)e.NewValue)
			{
				datagrid.LostKeyboardFocus += RollbackDataGridOnLostFocus;
				datagrid.DataContextChanged += RollbackDataGridOnDataContextChanged;
			}
			else
			{
				datagrid.LostKeyboardFocus -= RollbackDataGridOnLostFocus;
				datagrid.DataContextChanged -= RollbackDataGridOnDataContextChanged;
			}
		}

		static void RollbackDataGridOnLostFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			DataGrid senderDatagrid = sender as DataGrid;

			if (senderDatagrid == null)
				return;

			UIElement focusedElement = Keyboard.FocusedElement as UIElement;
			if (focusedElement == null)
				return;

			DataGrid focusedDatagrid = GetParentDatagrid(focusedElement); //let's see if the new focused element is inside a datagrid
			if (focusedDatagrid == senderDatagrid)
			{
				return;
				//if the new focused element is inside the same datagrid, then we don't need to do anything;
				//this happens, for instance, when we enter in edit-mode: the DataGrid element loses keyboard-focus, which passes to the selected DataGridCell child
			}

			//otherwise, the focus went outside the datagrid; in order to avoid exceptions like ("DeferRefresh' is not allowed during an AddNew or EditItem transaction")
			//or ("CommitNew is not allowed for this view"), we undo the possible pending changes, if any
			IEditableCollectionView collection = senderDatagrid.Items as IEditableCollectionView;

			if (collection.IsEditingItem)
			{
				collection.CancelEdit();
			}
			else if (collection.IsAddingNew)
			{
				collection.CancelNew();
			}
		}

		private static DataGrid GetParentDatagrid(UIElement element)
		{
			UIElement childElement; //element from which to start the tree navigation, looking for a Datagrid parent

			if (element is ComboBoxItem) //since ComboBoxItem.Parent is null, we must pass through ItemsPresenter in order to get the parent ComboBox
			{
				ItemsPresenter parentItemsPresenter = VisualTreeFinder.FindParentControl<ItemsPresenter>((element as ComboBoxItem));
				ComboBox combobox = parentItemsPresenter.TemplatedParent as ComboBox;
				childElement = combobox;
			}
			else
			{
				childElement = element;
			}

			DataGrid parentDatagrid = VisualTreeFinder.FindParentControl<DataGrid>(childElement); //let's see if the new focused element is inside a datagrid
			return parentDatagrid;
		}

		static void RollbackDataGridOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			DataGrid senderDatagrid = sender as DataGrid;

			if (senderDatagrid == null)
				return;

			IEditableCollectionView collection = senderDatagrid.Items as IEditableCollectionView;

			if (collection.IsEditingItem)
			{
				collection.CancelEdit();
			}
			else if (collection.IsAddingNew)
			{
				collection.CancelNew();
			}
		}

		#endregion DataGridRollbackOnUnfocusedBehaviour
	}
	class VisualTreeFinder
	{

		/// <summary>
		/// Find a specific parent object type in the visual tree
		/// </summary>
		public static T FindParentControl<T>(DependencyObject outerDepObj) where T : DependencyObject
		{
			DependencyObject dObj = VisualTreeHelper.GetParent(outerDepObj);
			if (dObj == null)
				return null;

			if (dObj is T)
				return dObj as T;

			while ((dObj = VisualTreeHelper.GetParent(dObj)) != null)
			{
				if (dObj is T)
					return dObj as T;
			}

			return null;
		}

	}
}
