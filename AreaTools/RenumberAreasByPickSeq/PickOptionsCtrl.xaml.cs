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
    public partial class PickOptionsCtrl : UserControl
    {
        AreaCollectionVM m_areaCollectionVm;

        public PickOptionsCtrl(AreaCollectionVM areaCollectionVM)
        {
            InitializeComponent();
            this.DataContext = m_areaCollectionVm = areaCollectionVM;
        }

        private void btnEndSelection_Click(object sender, RoutedEventArgs e)
        {
            m_areaCollectionVm.StopSelection = true;
            Window thisWindow = this.Parent as Window;
            thisWindow.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            m_areaCollectionVm.StopSelection = true;
            Window thisWindow = this.Parent as Window;
            thisWindow.Close();
        }
    }
}
