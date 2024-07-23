When creating a new activity and its designer:
1) Follow the best practices in activity creation
2) Follow the same folder structure and namespaces as existing ones
	e.g.	a) Get rid of the folder namespace Visual Studio adds to source files
			b) If wrapping another activity in FWBS.WF.ActivityLibrary, put it under 'Wrappers' folder
3) Ensure icons/bitmaps are of correct size
4) Do not forget to mark the toolbox icon as 'Embedded Resource' and designer icon as 'Resource' in Visual Studio
5) Make the ActivityDesigner class 'internal sealed' with public constructor - should not be exposed to customers!
6) Get rid of any 'using' statements that are NOT needed.
