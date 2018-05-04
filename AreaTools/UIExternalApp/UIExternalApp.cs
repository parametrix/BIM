using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UIExternalApp
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class UIExternalApp : IExternalApplication
    {
        static AddInId m_appId = new AddInId(new Guid("F11C6704-00F0-42CC-805D-DCD8EC6AA7DD"));

        // get the absolute path of this assembly
        static string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

        public Result OnStartup(UIControlledApplication uic_app)
        {
            // get existing SOM Ribbon Tab
            var panels = uic_app.GetRibbonPanels("SOM Tools");
            if (null == panels)
            {
                uic_app.CreateRibbonTab("SOM Tools");
            }
            // get or create panel
            RibbonPanel panel = panels.Where(x=>x.Name=="SOM-NY").FirstOrDefault();
            if (null == panel)
            {
                panel = uic_app.CreateRibbonPanel("SOM Tools", "SOM NY");
            }

            // pull down button
            PulldownButtonData pdBtnData = new PulldownButtonData("SOMNY_Utils", "UTILITIES");
            RibbonItem ribbonItem = panel.AddItem(pdBtnData);
            PulldownButton pdBtn = ribbonItem as PulldownButton;

            // get assembly
            string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string ExecutingAssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            string ExecutingAssemblyDirectory = Path.GetDirectoryName(ExecutingAssemblyPath);


            // add items
            pdBtn.AddPushButton(new PushButtonData("RenumberAreasByPickSeq", "Re-Number Areas By Pick Order", Path.Combine(ExecutingAssemblyDirectory, "RenumberAreasByPickSeq.dll"), "RenumberAreasByPickSeq.Cmd_PickAreasBySeq"));

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication uic_app)
        {
            return Result.Succeeded;
        }
    }
}
