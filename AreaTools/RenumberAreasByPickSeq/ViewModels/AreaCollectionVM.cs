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
                _areaVMs = value;
                OnPropertyChanged("AreaVMs");
            }
        }

        public bool StopSelection
        {
            get { return _stopSelection; }
            set
            {
                _stopSelection = value;
                OnPropertyChanged("StopSelection");
            }
        }

        public bool UpdateParameters
        {
            get { return _updateParameters; }
            set
            {
                _updateParameters = value;
                OnPropertyChanged("UpdateParameters");
            }
        }


        public void AddArea(Area area, int pickOrder)
        {
            AreaVMs.Add(new AreaVM(area, pickOrder));
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
