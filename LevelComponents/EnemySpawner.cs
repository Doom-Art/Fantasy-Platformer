using Godot;
using System;

public partial class EnemySpawner : Marker2D
{
	[Export]
	private PackedScene enemy;
	private Player player;
	private bool _isOffScreen;
    public override void _Ready()
    {
		_isOffScreen = true;
    }
	public void SetPlayer(Player player)
	{
		this.player = player;
	}
	public void Spawn()
	{
		//GD.Print(player);
		if (_isOffScreen)
		{
            var e = enemy.Instantiate<Enemy>();
            e.GlobalPosition = GlobalPosition;
            e.Player = player;
            GetParent().AddChild(e);
        }
	}
	public void ScreenExited()
	{
		_isOffScreen = true;
		//GD.Print("Offscreen");
	}
	public void ScreenEntered()
	{
		_isOffScreen = false;
        //GD.Print("OnScreen");
    }
}
