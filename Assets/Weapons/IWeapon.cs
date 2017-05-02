/// <summary>
/// Base interface for Weapon
/// </summary>
public interface IWeapon
{
	string Name {get;}
	void Fire(IProjectile projectile);
}
