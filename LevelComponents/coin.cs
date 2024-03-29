using Godot;

public partial class coin : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void OnAreaEntered(Area2D area)
	{
		if (area.GetParent().IsInGroup("player"))
		{
			var player = area.GetParent() as Player;
			player.GetCoin();
			QueueFree();
		}
	}
}
