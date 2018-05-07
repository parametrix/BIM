using RenumberAreasByPickSeq.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RenumberAreasByPickSeq
{
    /// <summary>
    /// Interaction logic for PickOptionsCtrl.xaml
    /// </summary>
    public partial class PickOptionsCtrl : System.Windows.Controls.UserControl
    {
        AreaCollectionVM m_areaCollectionVm;

        public PickOptionsCtrl(AreaCollectionVM areaCollectionVM)
        {
            InitializeComponent();
            this.DataContext = m_areaCollectionVm = areaCollectionVM;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            m_areaCollectionVm.StopSelection = true;
            Window thisWindow = this.Parent as Window;
            thisWindow.Close();
        }

        private void btnStopSelection_Click(object sender, RoutedEventArgs e)
        {
            m_areaCollectionVm.StopSelection = true;
        }

        /// <summary>
        /// to facilitate re-ordering
        /// from: https://stackoverflow.com/questions/3350187/wpf-c-rearrange-items-in-listbox-via-drag-and-drop?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(e.LeftButton==MouseButtonState.Pressed && sender is System.Windows.Controls.ListViewItem)
            {
                System.Windows.Controls.ListViewItem draggedItem = sender as System.Windows.Controls.ListViewItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, System.Windows.DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        /// <summary>
        /// to facilitate re-ordering
        /// from: https://stackoverflow.com/questions/3350187/wpf-c-rearrange-items-in-listbox-via-drag-and-drop?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_Drop(object sender, System.Windows.DragEventArgs e)
        {
            AreaVM droppedData = e.Data.GetData(typeof(AreaVM)) as AreaVM;
            AreaVM target = ((System.Windows.Controls.ListViewItem)(sender)).DataContext as AreaVM;

            int removedIdx = listView.Items.IndexOf(droppedData);
            int targetIdx = listView.Items.IndexOf(target);

            if (removedIdx < targetIdx)
            {
                m_areaCollectionVm.AreaVMs.Insert(targetIdx + 1, droppedData);
                m_areaCollectionVm.AreaVMs.RemoveAt(removedIdx);
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (m_areaCollectionVm.AreaVMs.Count + 1 > remIdx)
                {
                    m_areaCollectionVm.AreaVMs.Insert(targetIdx, droppedData);
                    m_areaCollectionVm.AreaVMs.RemoveAt(remIdx);
                }
            }
        }
    }
}
