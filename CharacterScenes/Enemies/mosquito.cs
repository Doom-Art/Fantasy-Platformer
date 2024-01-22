using Godot;
using System;

public partial class mosquito : Enemy
{
    public const float VSpeed = 80.0f;
    public const float HSpeed = 150.0f;
    public const float AtkSpeed = 350f;
    private AnimatedSprite2D sprite;
    [Export]
    private int atkDmg = 3;
    private Vector2 target, prevLoc;
    private bool attack;

    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        sprite.Play("Flight");
        GetNode<CollisionShape2D>("AttackBox/CollisionShape2D").Disabled = true;
        Dead = false;
    }
    public void AnimationFinished()
    {
        if (sprite.Animation != "Death")
        {
            sprite.Play("Flight");
        }
        else
        {
            QueueFree();
        }
    }
    public override void Hurt()
    {
        sprite.Play("Hurt");
        GetNode<CollisionShape2D>("AttackBox/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        attack = false;
        Velocity = Vector2.Up * VSpeed;
    }
    public override void OnDeath()
    {
        Dead = true;
        GetNode<CollisionShape2D>("HitBoxComponent/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        sprite.Play("Death");
    }
    public override void _PhysicsProcess(double delta)
    {
        if (!Dead)
        {
            Vector2 velocity = Velocity;

            if (!attack && sprite.Animation == "Flight")
            {
                if (Player.GlobalPosition.Y - 150 > GlobalPosition.Y)
                {
                    velocity.Y = VSpeed;
                }
                else if (Player.GlobalPosition.Y - 60 < GlobalPosition.Y)
                {
                    velocity.Y = -VSpeed;
                }
                else
                {
                    velocity.Y = 0;
                }

                if (Player.GlobalPosition.X - 35 > GlobalPosition.X)
                {
                    velocity.X = HSpeed;
                    var scale = Transform;
                    scale.X.X = 1;
                    Transform = scale;
                }
                else if (Player.GlobalPosition.X + 35 < GlobalPosition.X)
                {
                    velocity.X = -HSpeed;
                    var scale = Transform;
                    scale.X.X = -1;
                    Transform = scale;
                }
                else
                    velocity.X = 0;

                if (Math.Abs(GlobalPosition.X - Player.GlobalPosition.X) <= 100 && GlobalPosition.Y + 60 < Player.GlobalPosition.Y)
                {
                    target = Player.GlobalPosition;
                    attack = true;
                    sprite.Play("Attack");
                    GetNode<CollisionShape2D>("AttackBox/CollisionShape2D").Disabled = false;
                    if (Transform.X.X == 1)
                    {
                        target.X += 35;
                    }
                    else
                    {
                        target.X += -35;
                    }
                }
                else if (GlobalPosition.DistanceSquaredTo(Player.GlobalPosition) >= 2100000)
                {
                    QueueFree();
                }
            }
            else if (attack)
            {
                float offset = 6;
                if ((GlobalPosition.X + offset >= target.X && GlobalPosition.X - offset <= target.X && GlobalPosition.Y + offset >= target.Y && GlobalPosition.Y - offset <= target.Y) || GlobalPosition.IsEqualApprox(prevLoc))
                {
                    attack = false;
                    sprite.Play("Flight");
                    GetNode<CollisionShape2D>("AttackBox/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
                }
                else
                {
                    velocity = GlobalPosition.DirectionTo(target);
                    velocity *= AtkSpeed;
                    prevLoc = GlobalPosition;
                }
            }


            Velocity = velocity;
            MoveAndSlide();
        }
    }
    public void OnAttackBoxEntered(Area2D area)
    {
        if (area.HasMethod("TakeDmg"))
        {
            var hitbox = area as HitBoxComponent;
            hitbox.TakeDmg(atkDmg);
            GetNode<HealthComponent>("HealthComponent").Heal(atkDmg);
        }
    }
}
