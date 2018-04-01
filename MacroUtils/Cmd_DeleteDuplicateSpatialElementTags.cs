using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Utils;

namespace MacroUtils
{
    [Transaction(TransactionMode.Manual)]
    public class Cmd_DeleteDuplicateSpatialElementTags : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            DeleteDuplicateSpatialTags(uidoc);

            return Result.Succeeded;
        }


        private bool DeleteDuplicateSpatialTags(UIDocument uidoc)
        {
            Document doc = uidoc.Document;
            ElementId viewId = uidoc.ActiveView.Id;

            ElementClassFilter filter = new ElementClassFilter(typeof(SpatialElementTag));

            var roomTagCollector = new FilteredElementCollector(doc, viewId)
                .WherePasses(filter)
                .WhereElementIsNotElementType()
                .Cast<SpatialElementTag>();

            if (null == roomTagCollector || roomTagCollector.Count() < 1) { return false; }

            List<ElementId> idsToDelete = new List<ElementId>();
            List<XYZ> TagCoords = new List<XYZ>();
            foreach (var e in roomTagCollector)
            {
                var tag = e as SpatialElementTag;
                if (null == tag) { continue; }
                XYZ tagPos = tag.TagHeadPosition;

                // check if dictionary has a key for tagposition key
                // if key exists, add tag for deletion
                if (XYZUtils.DoesCollectionContainPoint(TagCoords, tagPos))
                {
                    idsToDelete.Add(e.Id);
                }
                else // add coord signalling skipping of tag for that position
                {
                    TagCoords.Add(tagPos);
                }
            }


            if (idsToDelete.Count < 1)
            {
                TaskDialog.Show("Completing Command", "No Overlapping Room or Area Tags found. Exiting Command.");
                return false;
            }

            TaskDialog taskDialog = new TaskDialog("Confirm Deletion");
            taskDialog.MainInstruction = String.Format("About to delete {0} Overlapping Room or Area Tags.", idsToDelete.Count.ToString());
            taskDialog.AllowCancellation = true;
            taskDialog.FooterText = "Select \'Cancel\' if you changed your mind. Would you like to proceed with deletion?";
            taskDialog.CommonButtons = TaskDialogCommonButtons.Cancel | TaskDialogCommonButtons.Yes; 

            if (taskDialog.Show() == TaskDialogResult.Cancel)
            {
                return false;
            }


            using (Transaction t = new Transaction(doc, "Deleting Duplicate Elements via Macro"))
            {
                t.Start();
                doc.Delete(idsToDelete);
                t.Commit();
            }

            return true;
        }
    }
}
