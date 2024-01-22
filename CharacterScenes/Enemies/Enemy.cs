using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public Player Player {get; set;}
	public bool Dead { get; set; }
	public virtual void OnDeath()
	{
		QueueFree();
	}
	public virtual void Hurt()
	{

	}
	
}
