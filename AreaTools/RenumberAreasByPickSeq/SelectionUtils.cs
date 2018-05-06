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
        /// <summary>
        /// method to select multiple areas 
        /// selected areas shows up to gether
        /// selected objects are highlighted
        /// </summary>
        /// <param name="collectionVM"></param>
        /// <param name="uidoc"></param>
        /// <param name="counter"></param>
        internal static void GetAreasByMultipleSelection(AreaCollectionVM collectionVM, UIDocument uidoc, ref int counter)
        {
            IList<Reference> areasReferences = null;
            ICollection<ElementId> selectedIds = null;
            try
            {
                areasReferences = uidoc.Selection.PickObjects(ObjectType.Element, new AreaSelectionFilter());
            }
            catch
            {
                if (collectionVM.StopSelection) { return; }
                /*operation cancelled*/
                TaskDialog.Show("Error", "Please select 'Finish' in the 'Options Bar' of the interface to complete the selection");
                // exit command and close window
                return;
            }

            if (null == areasReferences && null==selectedIds)
            {
                return;
            }
            // if no references were found, but selected items were retrieved
            if (null==selectedIds && null != areasReferences)
            {
                selectedIds = areasReferences.Select(x => x.ElementId).ToList();
            }

            // use selected ids
            var areasElements = selectedIds.Select(x => uidoc.Document.GetElement(x));

            foreach(Element areaElement in areasElements)
            {
                Area area = areaElement as Area;
                collectionVM.AddArea(area, counter);
                counter++;
            }

        }

        /// <summary>
        /// Method to select multiple areas...
        /// Selected areas show up one by one...
        /// No highlighting of selected objects
        /// </summary>
        /// <param name="collectionVM"></param>
        /// <param name="uidoc"></param>
        /// <param name="counter"></param>
        internal static void GetAreasByIndividualSelection(AreaCollectionVM collectionVM, UIDocument uidoc, ref int counter)
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
