using Godot;
using System;

public partial class HealthComponent : Node2D
{
	[Export]
	int maxHealth = 10;
	int health;
	private TextureProgressBar healthBar;
	public override void _Ready()
	{
		health = maxHealth;
		healthBar = GetNodeOrNull<TextureProgressBar>("TextureProgressBar");
		if (healthBar != null)
		{
			healthBar.MaxValue = maxHealth;
			healthBar.Value = health;
        }
	}
	public void TakeDamage(int damage)
	{
		health -= damage;
        if (healthBar != null)
            healthBar.Value = health;
        if (health <= 0)
		{
            if (GetParent().IsInGroup("player"))
			{
				Player parent = GetParent() as Player;
				parent.Death();
			}
			else if (GetParent().IsInGroup("mobs"))
			{
				Enemy e = GetParent() as Enemy;
				e.OnDeath();
			}
			else
			{
				GetParent().QueueFree();
			}
		}
    }
	public void Heal(int healAmount)
	{
		health += healAmount;
		if (health >  maxHealth)
			health = maxHealth;
        if (healthBar != null)
            healthBar.Value = health;
    }
}
