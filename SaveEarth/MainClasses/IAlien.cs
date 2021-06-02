using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveEarth.MainClasses
{
    public interface IAlien
    {
        double LocationX { get; }
        double LocationY { get; }
        double Direction { get; }
        double Radius { get; }
        int KillPoints { get; }
        bool isDead { get; }
        int HitBox { get; }
        bool isDropBoost { get; }
        bool CanDeleteAlien { get; }
        AttackType attackType { get; }
        bool CanDoAttack();
        void ChangeBoostStatus();
        void Move(double dt);
        Image GetFrameForAnimation();
        void AnimateAlien();
        void TakeDamage(int damage);

    }

}
