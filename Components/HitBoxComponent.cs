using Godot;
using Godot.NativeInterop;
using System;

public partial class HitBoxComponent : Area2D
{
	[Export]
	HealthComponent health;
	[Export]
	private float immunityTime = 0.8f;
	private float timeSinceDmg;
    public override void _Ready()
    {
        health ??= GetParent().GetNodeOrNull<HealthComponent>("HealthComponent");
        timeSinceDmg = immunityTime;
    }
    public override void _Process(double delta)
    {
		timeSinceDmg += (float)delta;
        base._Process(delta);
    }
    public void ResetImmunity()
    {
        timeSinceDmg = immunityTime;
    }
    public void TakeDmg(int atk)
	{
		if (timeSinceDmg > immunityTime)
		{
            timeSinceDmg = 0;
            if (health != null)
            {
                var parent = GetParent();
                if (parent.IsInGroup("mobs"))
                {
                    var enemy = GetParent() as Enemy;
                    enemy.Hurt();
                }
                else if (parent.IsInGroup("player"))
                {
                    //GD.Print($"Player Hit with {atk} damage");
                    var player = GetParent() as Player;
                    player.Hurt();
                }
                health.TakeDamage(atk);
            }
            else
            {
                GetParent().QueueFree();
            }
        }
	}
}
