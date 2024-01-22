using Godot;
using System;

public partial class character_selection : Control
{
    [Export(PropertyHint.File)]
    public string NextPath = "";

    [Export(PropertyHint.File)]
    public string Char1 = "";
    [Export(PropertyHint.File)]
    public string Char2 = "";
    [Export(PropertyHint.File)]
    public string Char3 = "";
    [Export(PropertyHint.File)]
    public string Char4 = "";
    [Export(PropertyHint.File)]
    public string Char5 = "";
    [Export(PropertyHint.File)]
    public string Char6 = "";
    [Export(PropertyHint.File)]
    public string Char7 = "";
    [Export(PropertyHint.File)]
    public string Char8 = "";
    [Export(PropertyHint.File)]
    public string Char9 = "";

    public String configSavePath = "res://GameSaveData.cfg";
    private ConfigFile config;
    private int _currentChar;
    private string _selectedCharPath;
    private Sprite2D[] _previewSprites;
    private TextureProgressBar _hpBar;
    private TextureProgressBar _speedBar;
    private TextureProgressBar _atkBar;
    public override void _Ready()
    {
        config = new ConfigFile();
        config.Load(configSavePath);
        _previewSprites = new Sprite2D[9]
        {
            GetNodeOrNull<Sprite2D>("CharButtons/Ninja/Preview"),
            GetNodeOrNull<Sprite2D>("CharButtons/Silver/Preview"),
            GetNodeOrNull<Sprite2D>("CharButtons/U"),
            GetNodeOrNull<Sprite2D>("CharButtons/U"),
            GetNodeOrNull<Sprite2D>("CharButtons/U"),
            GetNodeOrNull<Sprite2D>("CharButtons/U"),
            GetNodeOrNull<Sprite2D>("CharButtons/U"),
            GetNodeOrNull<Sprite2D>("CharButtons/U"),
            GetNodeOrNull<Sprite2D>("CharButtons/U")
        };
        _hpBar = GetNode<TextureProgressBar>("Health");
        _speedBar = GetNode<TextureProgressBar>("Speed");
        _atkBar = GetNode<TextureProgressBar>("Attack");
        _currentChar = (int)config.GetValue("CharSelection", "CurrentChar", 0);
        _previewSprites[_currentChar].Visible = true;
        switch (_currentChar)
        {
            case 0:
                Char1Select();
                break;
            case 1:
                Char2Select();
                break;
            case 2:
                
                break;
            case 3:

                break;
            case 4:

                break;
            case 5:

                break;
            case 6:

                break;
            case 7:

                break;
            case 8:

                break;
        }
    }

    public void NextScreen()
    {
        config.SetValue("CharSelection", "CurrentChar", _currentChar);
        config.SetValue("Game", "Player", _selectedCharPath);
        config.Save(configSavePath);
        if (NextPath != "")
            GetTree().ChangeSceneToFile(NextPath);
    }

    public void Char1Select()
    {
        _previewSprites[_currentChar].Visible = false;
        _currentChar = 0;
        _previewSprites[0].Visible = true;
        _selectedCharPath = Char1;
        _hpBar.Value = 30;
        _speedBar.Value = 80;
        _atkBar.Value = 90;
    }
    public void Char2Select()
    {
        _previewSprites[_currentChar].Visible = false;
        _currentChar = 1;
        _previewSprites[1].Visible = true;
        _selectedCharPath = Char2;
        _hpBar.Value = 90;
        _speedBar.Value = 60;
        _atkBar.Value = 30;
    }
}
