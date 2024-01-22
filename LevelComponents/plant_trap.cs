using Godot;
using System;

public partial class plant_trap : Node2D
{
	[Export]
	private int atkDmg = 10;
	[Export]
	private float snapCooldown = 3;
	private float timer;
	public override void _Ready()
	{
		GetNode<CollisionShape2D>("AttackBox/CollisionShape2D").Disabled = true;
		timer = snapCooldown;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timer += (float)delta;
	}
	public void OnEnemyEntered(Area2D area)
	{
		if (area.GetParent().IsInGroup("player"))
		{
			var play = area.GetParent() as Player;
			play.GetNode<HitBoxComponent>("HitBoxComponent").ResetImmunity();
		}
		if (timer > snapCooldown)
		{
            GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("Snap");
            GetNode<CollisionShape2D>("AttackBox/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
            timer = 0;
        }
    }
	public void OnAnimationFinished()
	{
        GetNode<CollisionShape2D>("AttackBox/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }
	public void OnAttackBoxEntered(Area2D area)
	{
        if (area.HasMethod("TakeDmg"))
        {
            var hitbox = area as HitBoxComponent;
            hitbox.TakeDmg(atkDmg);
        }
    }
}
