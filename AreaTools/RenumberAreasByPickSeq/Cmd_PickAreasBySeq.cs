using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Windows;
using RenumberAreasByPickSeq.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;


using ComponentManager = Autodesk.Windows.ComponentManager;
using IWin32Window = System.Windows.Forms.IWin32Window;
using Keys = System.Windows.Forms.Keys;

namespace RenumberAreasByPickSeq
{
    [Transaction(TransactionMode.Manual)]
    public class Cmd_PickAreasBySeq:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;


            // set up UI
            AreaCollectionVM ctrlVM = new AreaCollectionVM();
            PickOptionsCtrl ctrl = new PickOptionsCtrl(ctrlVM);
            Window win = new Window();
            win.Width = 300;
            win.Content = ctrl;
            win.SizeToContent = SizeToContent.WidthAndHeight;
            win.Topmost = true;
            win.Show();

            int counter = 0;
            List<Area> pickedAreas = new List<Area>();
            // override graphics settings for picked elements to differentiate from un-picked elements
            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
            ogs.SetProjectionFillColor(new Color(255, 255, 255));
            //ogs.SetProjectionFillColor(new Color(139, 166, 178));
            // make area interior fill visible
            Category interiorFillVisibility = Category.GetCategory(doc, BuiltInCategory.OST_AreaInteriorFillVisibility);
            Category interiorFill = Category.GetCategory(doc, BuiltInCategory.OST_AreaInteriorFill);
            Category areaRef = Category.GetCategory(doc, BuiltInCategory.OST_AreaReferenceVisibility);
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Showing Area Interior Fill");
                doc.ActiveView.SetCategoryHidden(interiorFillVisibility.Id, false);
                doc.ActiveView.SetCategoryHidden(areaRef.Id, false);
                t.Commit();
            }

            SelectionUtils.GetAreasBySelection(ctrlVM, uidoc, ref counter);

            //var areas = uidoc.Selection.PickObjects(ObjectType.Element, new AreaSelectionFilter());
            //foreach(var refeId in areas)
            //{
            //    Element areaElement = uidoc.Document.GetElement(refeId);
            //    Area area = areaElement as Area;

            //    ctrlVM.AddArea(area, counter);
            //    counter++;
            //}

            if (ctrlVM.UpdateParameters)
            {
                // run process to update parameters according to the view model
                // TODO.............................

            }

            return Result.Succeeded;
        }
    }
}
