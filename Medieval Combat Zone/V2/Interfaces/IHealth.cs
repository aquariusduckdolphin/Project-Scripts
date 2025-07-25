using System.Collections;

namespace CombatZone.Interfaces
{
    public interface IHealth
    {

        public float maxHealth { get; }

        public float currentHealth { get; }

        public bool isDead { get; }

        public abstract void TakeDamage(float damage);

        public virtual void GiveHealth(float healthGained) { }

        public IEnumerator TickDamage(float damage, float delay, bool infiniteLoop, float loopAmount);

    }

}
