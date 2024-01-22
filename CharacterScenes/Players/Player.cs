using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export]
    public float Speed = 300.0f;
    public const float JumpVelocity = -550.0f;
    public bool attack, fallRight, death;
    [Export]
    public int attackDmg = 5;
    public Node2D Flip;
    public AnimatedSprite2D sprite;
    [Export(PropertyHint.File)]
    private String menuFilePath = "";
    public ConfigFile Config;
    public String ConfigSavePath = "res://GameSaveData.cfg";
    public int Coins { get; private set; }

    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    public virtual void AnimationFinished()
    {
        if (sprite.Animation == "Death")
        {
            GetTree().ReloadCurrentScene();
        }
        attack = false;
        sprite.Play("Idle");
    }
    public override void _Ready()
    {
        Config = new ConfigFile();
        Config.Load(ConfigSavePath);
        Coins = (int)Config.GetValue("Player", "Coins", 0);
        GetNode<Label>("Coin/Label").Text = " = " + Coins;

        sprite = GetNode<AnimatedSprite2D>("Flip/AnimatedSprite2D");
        sprite.Play("Idle");
        Flip = GetNode<Node2D>("Flip");
        death = false;
    }
    public void Save()
    {
        Config.SetValue("Player", "Coins", Coins);
        Config.Save(ConfigSavePath);
    }
    public void GetCoin()
    {
        Coins += 1;
        GetNode<Label>("Coin/Label").Text = " = " + Coins;
    }
    public void Death()
    {
        GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
        sprite.Play("Death");
        GetNode<CollisionShape2D>("HitBoxComponent/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        death = true;
        var pos = Position;
        pos.Y += 10;
        Position = pos;
    }
    public void Hurt()
    {
        sprite.Play("Hurt");
        GetNode<CollisionShape2D>("Flip/HurtBox/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        attack = false;
        var velocity = Velocity;
        velocity.Y = -400;
        Velocity = velocity;
        fallRight = Flip.Transform.X.X == -1;
    }
    public void OnClosedButtonPressed()
    {
        if (menuFilePath != "")
            GetTree().ChangeSceneToFile(menuFilePath);
    }
    //public override void _PhysicsProcess(double delta)
    //{
    //    Vector2 velocity = Velocity;

    //    // Add the gravity.
    //    if (!IsOnFloor())
    //        velocity.Y += gravity * (float)delta;
    //    Velocity = velocity;
    //    MoveAndSlide();
    //}
}
