using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RenumberAreasByPickSeq.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenumberAreasByPickSeq
{
    class SelectionUtils
    {
        internal static void GetAreasBySelection(AreaCollectionVM collectionVM, UIDocument uidoc, ref int counter)
        {
            while (!collectionVM.StopSelection)
            {
                Reference pickedRef;
                try
                {
                    // try catch to capture escape options
                    pickedRef = uidoc.Selection.PickObject(ObjectType.Element, new AreaSelectionFilter());
                }
                catch { break; }

                if (collectionVM.StopSelection) { break; }

                var refeId = pickedRef.ElementId;
                Element areaElement = uidoc.Document.GetElement(refeId);
                Area area = areaElement as Area;


                collectionVM.AddArea(area, counter);
                counter++;
            }
        }
    }
}
