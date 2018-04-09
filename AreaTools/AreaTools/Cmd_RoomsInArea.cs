using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AreaTools.DataModels;
using AreaTools.Utils;

namespace AreaTools
{
    [Transaction(TransactionMode.Manual)]
    public class Cmd_RoomsInArea : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            ElementCategoryFilter roomFilter = new ElementCategoryFilter(BuiltInCategory.OST_Rooms);
            var rooms = new FilteredElementCollector(doc, uidoc.ActiveView.Id)
                .WherePasses(roomFilter)
                .WhereElementIsNotElementType()
                .Cast<SpatialElement>();

            AreaFilter areaFilter = new AreaFilter();
            var areaObjects = new FilteredElementCollector(doc, uidoc.ActiveView.Id)
                .WherePasses(areaFilter)
                .WhereElementIsNotElementType()
                .Cast<Area>()
                .Select(x => new AreaObject(x));

            StringBuilder sb = new StringBuilder();

            foreach (AreaObject areaobject in areaObjects)
            {
                List<SpatialElement> roomObjects = new List<SpatialElement>();
                foreach (SpatialElement roomElement in rooms)
                {
                    LocationPoint lp = roomElement.Location as LocationPoint;
                    if (areaobject.Area.AreaContains(lp.Point))
                    {
                        roomObjects.Add(roomElement);
                        sb.AppendLine(areaobject.Area.Name + " ;" + roomElement.Name);
                    }
                }
                areaobject.Rooms = roomObjects;
                sb.AppendLine(areaobject.Area.Name + " ; Room Count:" + areaobject.Rooms.Count);
            }

            TaskDialog.Show("Area Objects", sb.ToString());

            return Result.Succeeded;
        }
    }
}
