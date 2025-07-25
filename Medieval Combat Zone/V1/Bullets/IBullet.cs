
namespace CombatZone.Bullet
{

    public interface IBullet
    {

        public enum ElementType
        {

            Fire = 0,
            Water = 1,
            Earth = 2,
            Air = 3

        }

        ElementType meleeElementWeapon { get; }

    }


}