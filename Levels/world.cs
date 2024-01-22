using Godot;
using System;

public partial class world : Node2D
{
    [Export]
    private int requiredKills = 10;
    private int kills;
    [Export(PropertyHint.File)]
    private String nextLevel = "";
    Player player;
    private Label doorLabel;
	private int enemiesExist;
    private PackedScene _coin;
    // Called when the node enters the scene tree for the first time.
    public String configSavePath = "res://GameSaveData.cfg";
    private ConfigFile config;
    public override void _Ready()
	{
        config = new ConfigFile();
        config.Load(configSavePath);
        var playerPath = (String)config.GetValue("Game","Player","res://CharacterScenes/Players/Rogues/Ninja");
        player = ResourceLoader.Load<PackedScene>(playerPath).Instantiate<Player>();
        player.Position = GetNode<Marker2D>("PlayerStartPos").Position;
        AddChild(player);

        GetTree().CallGroup("spawners", EnemySpawner.MethodName.SetPlayer, player);
        _coin = ResourceLoader.Load<PackedScene>("res://LevelComponents/coin.tscn");
        kills = 0;
		enemiesExist = 0;
        doorLabel = GetNodeOrNull<Label>("ExitDoor/Label");
        if (doorLabel != null)
            doorLabel.Text = $"{kills}/{requiredKills} Kills";
        //SpawnAll();
    }
	public void SpawnAll()
	{
		if (enemiesExist < 5)
	        GetTree().CallGroup("spawners", EnemySpawner.MethodName.Spawn);
    }
    public void OnChildEntered(Node child)
    {
        if (child.IsInGroup("mobs"))
        {
            enemiesExist++;
        }
    }
    public void OnChildExit(Node child)
    {
        if (child.IsInGroup("mobs"))
        {
            var enemy = child as Enemy;
            if (enemy.Dead)
            {
                var coin = _coin.Instantiate<Area2D>();
                coin.Position = enemy.GlobalPosition;
                AddChild(coin);
                kills++;
                if (doorLabel != null)
                    doorLabel.Text = $"{kills}/{requiredKills} Kills";
            }
            enemiesExist--;
        }
    }
    public void DoorEntered(Area2D area)
    {
        if (kills >= requiredKills && nextLevel != "")
        {
            player.Save();
            GetTree().CallDeferred(SceneTree.MethodName.ChangeSceneToFile, nextLevel);
        }
    }
}
