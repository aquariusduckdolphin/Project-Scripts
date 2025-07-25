
namespace CombatZone.Interfaces
{

    public interface IDamage
    {

        public float damageAmount { get; set; }

        public abstract void TakeDamage(float damage);

    }

}