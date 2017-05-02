using UnityEngine;
/// <summary>
/// Base Interface For Projectile
/// </summary>
public interface IProjectile
{
	string Name {get;}
	float BaseDamage{get;}
	object Prefab {get;}
	void Fire(Vector2 direction);
	void OnCollisionEnter2D(Collision2D col);
	void LivedTooLong();
}
