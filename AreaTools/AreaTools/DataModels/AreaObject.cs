using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaTools.DataModels
{
    class AreaObject
    {
        private Area _area;
        private IList<CurveLoop> _curveLoopList;
        private List<SpatialElement> _rooms;

        public AreaObject(Area area)
        {
            _area = area;
            _rooms = new List<SpatialElement>();

            SpatialElementBoundaryOptions opt = new SpatialElementBoundaryOptions();
            opt.StoreFreeBoundaryFaces = true;
            opt.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Center;

            _curveLoopList = new List<CurveLoop>();
            var loopList = area.GetBoundarySegments(opt);
            foreach (var boundarySegList in loopList)
            {
                CurveLoop curves = new CurveLoop();
                foreach (var boundarySeg in boundarySegList)
                {
                    curves.Append(boundarySeg.GetCurve());
                }
                _curveLoopList.Add(curves);
            }
        }

        public Area Area { get { return _area; } }
        public IList<CurveLoop> CurveLoopList { get { return _curveLoopList; } }
        public List<SpatialElement> Rooms
        {
            get { return _rooms; }
            set { _rooms = value; }
        }
    }
}
