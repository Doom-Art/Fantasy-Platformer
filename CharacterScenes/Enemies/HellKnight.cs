using Godot;
using System;

public partial class HellKnight : Enemy
{
	[Export]
	public float Speed = 120.0f;
	[Export]
	public float JumpHeight = -510.0f;
	private AnimatedSprite2D sprite;
    [Export]
    private int atkDmg = 5;
    [Export]
    private int maxRange = 75;
	[Export]
	float atkLag = 0.5f;
    private float _attackTimer, _jumpTimer;
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public override void _Ready()
	{
		//GD.Print(IsInGroup("mobs"));
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sprite.Play("Walk");
		_attackTimer = 0;
		GetNode<CollisionShape2D>("AttackBox/CollisionShape2D").Disabled = true;
		Dead = false;
	}
	public void OnAttackBoxEntered(Area2D area)
	{
		if (area.HasMethod("TakeDmg"))
		{
			var hitbox = area as HitBoxComponent;
			hitbox.TakeDmg(atkDmg);
		}
	}
	public override void OnDeath()
	{
        Dead = true;
        GetNode<CollisionShape2D>("HitBoxComponent/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        sprite.Play("Death");
    }
	public void AnimationFinished()
	{
		if (sprite.Animation != "Death")
		{
			sprite.Play("Walk");
			GetNode<CollisionShape2D>("AttackBox/CollisionShape2D").Disabled = true;
			_attackTimer = 0;
		}
		else
		{
			QueueFree();
		}
	}
	public override void Hurt()
	{
		sprite.Play("Hurt");
		var velocity = Velocity;
		velocity.Y = JumpHeight/1.5f;
		Velocity = velocity;
		GetNode<CollisionShape2D>("AttackBox/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		if (!Dead)
		{
			var distance = Mathf.Abs(GlobalPosition.X - Player.GlobalPosition.X);
			if (!IsOnFloor())
			{
				velocity.Y += gravity * (float)delta;
				_jumpTimer = 0;
			}
			else if (sprite.Animation == "Walk")
			{
				if (Player.GlobalPosition.Y < GlobalPosition.Y && IsOnFloor() && _jumpTimer > 1.5f)
				{
					velocity.Y = JumpHeight;
				}
				_jumpTimer += (float)delta;
			}
			else if (distance < 15)
			{
				velocity.Y = -10;
			}

			if (sprite.Animation == "Hurt")
			{
				if (GlobalPosition.X > Player.GlobalPosition.X)
				{
					velocity.X = Speed*2;
				}
				else
				{
					velocity.X = -Speed*2;
				}
			}
			else if (distance > 1200)
			{
				QueueFree();
			}
			else if (distance < maxRange || sprite.Animation == "Attack")
			{
				velocity.X = 0;
				sprite.Play("Attack");
				if (_attackTimer > atkLag)
					GetNode<CollisionShape2D>("AttackBox/CollisionShape2D").Disabled = false;
				else
					_attackTimer += (float)delta;
				_jumpTimer = 0;
			}
			else if (Player.GlobalPosition.X > GlobalPosition.X)
			{
				velocity.X = Speed;
				var scale = Transform;
				scale.X.X = 1;
				Transform = scale;
			}
			else if (Player.GlobalPosition.X < GlobalPosition.X)
			{
				velocity.X = -Speed;
				var scale = Transform;
				scale.X.X = -1;
				Transform = scale;
			}
			Velocity = velocity;
			MoveAndSlide();
		}
	}
}
