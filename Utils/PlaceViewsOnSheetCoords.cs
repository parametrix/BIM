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
	/// Container class for Integration (static/singleton?)
	/// </summary>
	public class PlaceViews
	{
		/// <summary>
		/// Provide Sheet to place view on and the X & Y 
		/// coordinates of the Lower Left Corner of the View Port 
		/// from the Lower Left Corner of the Titleblock
		/// </summary>
		/// <param name="sheet">Sheet to place the view on</param>
		/// <param name="viewToPlace">View to place</param>
		/// <param name="xOffsetFromTitleBlockLowerLeft">Distance in Feet from the Lower Left Corner of Title Block to Lower Left Corner of Viewport along Horizontal axis</param>
		/// <param name="yOffsetFromTitleBlockLowerLeft">Distance in Feet from the Lower Left Corner of Title Block to Lower Left Corner of Viewport along Vertical axis</param>
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
		
		/// <summary>
		/// Center Point of Viewport
		/// </summary>
		XYZ GetViewportSetBoxCenter(double xOffsetFromTitleBlLowerLeft, double yOffsetFromTitleBlkLowerLeft, Viewport viewport, XYZ titleBlockCorner)
		{
		  var outline = viewport.GetBoxOutline();
		  XYZ vportMin = outline.MinimumPoint;
		  XYZ vportToSheetCorner = titleBlockCorner.Subtract(vportMin);
		  return new XYZ(vportToSheetCorner.X+xOffsetFromTitleBlLowerLeft, vportToSheetCorner.Y+yOffsetFromTitleBlkLowerLeft,0);
		}

				
		/// <summary>
		/// Get Lower Left Corner of Title block Geometry
		/// </summary>
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
