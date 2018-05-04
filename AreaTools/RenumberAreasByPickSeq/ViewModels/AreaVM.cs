using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenumberAreasByPickSeq.ViewModels
{
    public class AreaVM : INotifyPropertyChanged
    {
        readonly Area _area;
        private string _prefix;
        private int _pickOrder;

        public AreaVM(Area area, int pickOrder)
        {
            _area = area;
            _pickOrder = pickOrder;
        }

        public Area Area
        {
            get { return _area; }
        }

        public int PickOrder
        {
            get { return _pickOrder; }
            set
            {
                _pickOrder = value;
                OnPropertyChanged("PickOrder");
            }
        }

        public string Prefix
        {
            get { return _prefix; }
            set
            {
                _prefix = value;
                OnPropertyChanged("Prefix");
            }
        }

        #region PROPERTY CHANGED
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
