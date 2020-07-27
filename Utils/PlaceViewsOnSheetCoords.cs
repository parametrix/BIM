/*
 * Created by SharpDevelop.
 * User: rm
 * Date: 7/26/2020
 * Time: 10:40 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace ViewsOnSheet
{
	/// <summary>
	/// Description of PlaceViews.
	/// </summary>
	public class PlaceViews
	{
		public void PlaceViewOnSheetCoords(ViewSheet sheet, View viewToPlace, double xOffsetFromTitleBlockLowerLeft, double yOffsetFromTitleBlockLowerLeft)
		{
		  Document doc = sheet.Document;
		  XYZ titleBlockCorner = GetLowerLeftCornerOfTitleBlock(sheet);
		  
		  // Create viewport to get outline
		  Viewport viewport = null;
		  using(Transaction t = new Transaction(doc))
		  {
		    t.Start("Create Viewport");
		    viewport = Viewport.Create(doc, sheet.Id, viewToPlace.Id, new XYZ());
		    t.Commit();
		  }
		  
		  // reposition to user coordinates
		  XYZ vportCenterPos = GetViewportSetBoxCenter(xOffsetFromTitleBlockLowerLeft, yOffsetFromTitleBlockLowerLeft, viewport, titleBlockCorner);
		  using(Transaction t = new Transaction(doc))
		  {
		    t.Start("Move Viewport to sheet corner");
		    viewport.SetBoxCenter(vportCenterPos);
		    t.Commit();
		  }
		  
		}
		
		XYZ GetViewportSetBoxCenter(double xOffsetFromTitleBlLowerLeft, double yOffsetFromTitleBlkLowerLeft, Viewport viewport, XYZ titleBlockCorner)
		{
		  var outline = viewport.GetBoxOutline();
		  XYZ vportMin = outline.MinimumPoint;
		  XYZ vportToSheetCorner = titleBlockCorner.Subtract(vportMin);
		  return new XYZ(vportToSheetCorner.X+xOffsetFromTitleBlLowerLeft, vportToSheetCorner.Y+yOffsetFromTitleBlkLowerLeft,0);
		}
				
		View GetViewByName(Document doc,string viewName)
		{
		  return new FilteredElementCollector(doc)
		    .OfClass(typeof(View))
		    .WhereElementIsNotElementType()
		    .Cast<View>()
		    .Where(x=>x.Name==viewName)
		    .FirstOrDefault();
		}
				
		XYZ GetLowerLeftCornerOfTitleBlock(ViewSheet sheet)
		{
		  Document doc = sheet.Document;
		  
		  var titleBlock = new FilteredElementCollector(doc, sheet.Id)
		    .OfCategory(BuiltInCategory.OST_TitleBlocks)
		    .WhereElementIsNotElementType()
		    .Cast<FamilyInstance>()
		    .FirstOrDefault();
		  
		  // get bounding box
		  Options geometryOptions = new Options();
		  var bbx = titleBlock.get_BoundingBox(sheet);
		  
		  return bbx.Min;
		}

	}
}
