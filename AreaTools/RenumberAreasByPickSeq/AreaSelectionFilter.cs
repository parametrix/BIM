using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace RenumberAreasByPickSeq
{
    internal class AreaSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem.GetType() == typeof(Area))
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}