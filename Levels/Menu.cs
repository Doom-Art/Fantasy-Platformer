using Godot;
using System;

public partial class Menu : Control
{
    [Export(PropertyHint.File)]
    private String LevelSelect; 
    [Export(PropertyHint.File)]
    private String CharacterSelect;
    public void OnPlayPressed()
    {
        GetTree().ChangeSceneToFile(LevelSelect);
    }
    public void OnSettingsPressed()
    {
        GetTree().ChangeSceneToFile(CharacterSelect);
    }
}
