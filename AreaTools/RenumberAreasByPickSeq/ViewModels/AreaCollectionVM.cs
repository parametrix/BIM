using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenumberAreasByPickSeq.ViewModels
{
    public class AreaCollectionVM : INotifyPropertyChanged
    {
        private bool _stopSelection;
        private bool _updateParameters;
        ObservableCollection<AreaVM> _areaVMs;


        public AreaCollectionVM()
        {
            _areaVMs = new ObservableCollection<AreaVM>();
            _stopSelection = false;
            _updateParameters = false;
        }

        public ObservableCollection<AreaVM> AreaVMs
        {
            get { return _areaVMs; }
            set
            {
                SetField<ObservableCollection<AreaVM>>(ref _areaVMs, value, "AreaVMs");
            }
        }

        public bool StopSelection
        {
            get { return _stopSelection; }
            set
            {
                SetField<bool>(ref _stopSelection, value, "StopSelection");
            }
        }

        public bool UpdateParameters
        {
            get { return _updateParameters; }
            set
            {
                SetField<bool>(ref _updateParameters, value, "UpdateParameters");
            }
        }


        public void AddArea(Area area, int pickOrder)
        {
            AreaVMs.Add(new AreaVM(area, pickOrder));
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
