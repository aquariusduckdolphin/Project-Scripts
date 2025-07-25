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

        public abstract ElementType elementType { get; }

    }

    public interface IMeleeWeapon
    {

        public abstract void SwordIdle(string attacking, bool attackingState); 

        public abstract void SwordSwing(string attacking, bool attackingState);

    }

}