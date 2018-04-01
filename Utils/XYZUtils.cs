using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace Utils
{
    public class XYZUtils
    {
        public static bool DoesCollectionContainPoint(IEnumerable<XYZ> list, XYZ point)
        {
            if (list.Count() > 0)
            {
                foreach (XYZ item in list)
                {
                    if (item.IsAlmostEqualTo(point, 10e-5))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
