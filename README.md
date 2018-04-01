# BIM
Collection of BIM Tools (mostly Autodesk Revit)
## INSTALLATION
* Download the 'MacroUtils.bundle.zip' file and extract the contents (MacroUtils.bundle+MacroUtils.addin) and place them in the %appdata%\Autodesk\Revit\Addins\2018 (appdata - Roaming) folder. 
* Restart Revit if Revit was running while performing the previous step.
* You will recieve the notification to allow the app to run since it is unsigned.
* You may have to un-block the dll file depending on your security settings.
## COMMANDS
1. Cmd_DeleteDuplicateSpatialElementTags.cs
: This Deletes any Room, Area, Space, or Zone Tags that are overlapping. The sequence typically followed by the API is to go from the lowest order of the Element ID to the highest order - meaning a tag that was placed initially will be preserved whereas tags placed later will be deleted. 
: This does not check to see if a given tag is hidden in a view
2. Cmd_DeleteDuplicateIndependentTags.cs
: Same as #1 except that this command acts on category-specific tags such as door and window tags
