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
                SetField<int>(ref _pickOrder, value, "PickOrder");
            }
        }

        public string Prefix
        {
            get { return _prefix; }
            set
            {
                SetField<string>(ref _prefix, value, "Prefix");
            }
        }

        #region PROPERTY CHANGED NOTIFICATION
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}
