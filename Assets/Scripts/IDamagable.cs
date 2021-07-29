using UnityEngine;

public interface IDamagable
{
    public float CurrentHealth { get; }
    public float MaxHealth { get; }

    public void Damage(float damage, GameObject damager, Vector2 point, Vector2 direction);
    public void DamageOverTime(float totalDamage, float time, GameObject damager, Vector2 point, Vector2 direction);
    public void Kill(float damage, GameObject killer, Vector2 point, Vector2 direction);
}

public delegate void DamageAction(float damage, GameObject damager, Vector2 point, Vector2 direction);