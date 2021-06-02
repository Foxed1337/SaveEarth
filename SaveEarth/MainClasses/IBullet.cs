using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveEarth.MainClasses
{
    public interface IBullet
    {
        double LocationX { get; }
        double LocationY { get; }
        double Direction { get; }
        int Damage { get; }
        BulletType TypeBullet { get; }
        bool isHit { get; }
        void Move();
        void Hit();
        Image GetFrameForAnimation();
    }
}
