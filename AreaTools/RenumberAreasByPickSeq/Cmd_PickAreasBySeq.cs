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
        static AreaCollectionVM m_AreaCollectionVM;
        static PickOptionsCtrl m_PickOptionsCtrl;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;


            // set up UI
            // check to see if an open window already exists 

            if (null == m_AreaCollectionVM)
            {
                m_AreaCollectionVM = new AreaCollectionVM();
                m_PickOptionsCtrl = new PickOptionsCtrl(m_AreaCollectionVM);
            }
            Window win = new Window();
            win.Width = 300;
            win.Content = m_PickOptionsCtrl;
            win.SizeToContent = SizeToContent.WidthAndHeight;
            win.Topmost = true;
            win.Show();

            int counter = 0;
            // make area interior fill visible
            Category interiorFillVisibility = Category.GetCategory(doc, BuiltInCategory.OST_AreaInteriorFillVisibility);
            Category areaRef = Category.GetCategory(doc, BuiltInCategory.OST_AreaReferenceVisibility);
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Showing Area Interior Fill");
                doc.ActiveView.SetCategoryHidden(interiorFillVisibility.Id, false);
                doc.ActiveView.SetCategoryHidden(areaRef.Id, false);
                t.Commit();
            }

            // works for individual selection  - no highlighting of selected area
            //SelectionUtils.GetAreasByIndividualSelection(m_AreaCollectionVM, uidoc, ref counter);

            // works for mulitple selection - highlights, but does not update on interface
            while (!m_AreaCollectionVM.StopSelection)
            {
                SelectionUtils.GetAreasByMultipleSelection(m_AreaCollectionVM, uidoc, ref counter);
            }
            

            if (m_AreaCollectionVM.UpdateParameters)
            {
                // run process to update parameters according to the view model
                // TODO.............................

            }

            return Result.Succeeded;
        }
    }
}
