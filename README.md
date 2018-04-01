# BIM
Collection of BIM Tools (mostly Autodesk Revit)
## COMMANDS
1. Cmd_DeleteDuplicateSpatialElementTags.cs
  This Deletes any Room, Area, Space, or Zone Tags that are overlapping. The sequence typically followed by the API is to go from the lowest order of the Element ID to the highest order - meaning a tag that was placed initially will be preserved whereas tags placed later will be deleted. This does not check to see if a given tag is hidden in a view
2. Cmd_DeleteDuplicateIndependentTags.cs
   Same as #1 except that this command acts on category-specific tags such as door and window tags
