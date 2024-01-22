using Godot;
using System;

public partial class MeleePlayer : Player
{
    public override void AnimationFinished()
    {
        GetNode<CollisionShape2D>("Flip/HurtBox/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        base.AnimationFinished();
    }
    public override void _Ready()
    {
        GetNode<CollisionShape2D>("Flip/HurtBox/CollisionShape2D").Disabled = true;
        base._Ready();
    }
    public void HurtBoxAreaEntered(Area2D area)
    {
        if (area.HasMethod("TakeDmg"))
        {
            var hitbox = area as HitBoxComponent;
            hitbox.TakeDmg(attackDmg);
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        if (!death)
        {
            Vector2 velocity = Velocity;
            if (attack)
            {
                if (Input.IsActionJustPressed("Jump"))
                {
                    GetNode<CollisionShape2D>("Flip/HurtBox/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
                    attack = false;
                }
            }
            // Add the gravity.
            if (!IsOnFloor())
                velocity.Y += gravity * (float)delta;
            if (sprite.Animation != "Hurt")
            {
                // Handle Jump.
                if (Input.IsActionJustPressed("Jump") && IsOnFloor())
                {
                    velocity.Y = JumpVelocity;
                    sprite.Play("Jump");
                }

                if (Input.IsActionPressed("Right"))
                {
                    velocity.X = Speed;
                    var scale = Flip.Transform;
                    scale.X.X = 1;
                    Flip.Transform = scale;
                }
                else if (Input.IsActionPressed("Left"))
                {
                    velocity.X = -Speed;
                    var scale = Flip.Transform;
                    scale.X.X = -1;
                    Flip.Transform = scale;
                }
                else
                {
                    velocity.X = 0;
                }

                if (Input.IsActionJustPressed("Attack") && velocity.Y == 0 && !attack)
                {
                    attack = true;
                    sprite.Play("Attack");
                    GetNode<CollisionShape2D>("Flip/HurtBox/CollisionShape2D").Disabled = false;
                }
                if (!attack)
                {
                    if (velocity == Vector2.Zero)
                    {
                        sprite.Play("Idle");
                    }
                    else
                    {
                        if (sprite.Animation != "Jump" || velocity.Y == 0)
                        {
                            sprite.Play("Walk");
                        }
                    }
                }
            }
            else
            {
                if (fallRight)
                {
                    velocity.X = Speed;
                }
                else
                {
                    velocity.X = -Speed;
                }
            }

            Velocity = velocity;
            MoveAndSlide();
        }
    }
}
