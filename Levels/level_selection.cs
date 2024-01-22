using Godot;
using System;

public partial class level_selection : Control
{
	[Export(PropertyHint.File)]
	private String menu;
    [Export(PropertyHint.File)]
    private String level1;
    [Export(PropertyHint.File)]
    private String level2; 
    [Export(PropertyHint.File)]
    private String level3;
    public void OnLevel1ButtonPressed()
	{
		GetTree().ChangeSceneToFile(level1);
	}
    public void OnLevel2ButtonPressed()
    {
        GetTree().ChangeSceneToFile(level2);
    }
    public void OnLevel3ButtonPressed()
    {
        GetTree().ChangeSceneToFile(level3);
    }
    public void ReturnToMenu()
	{
		GetTree().ChangeSceneToFile(menu);
	}
}
