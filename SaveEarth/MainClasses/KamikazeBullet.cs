using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveEarth.MainClasses
{
    class KamikazeBullet : IBullet
    {
        public KamikazeBullet(double locationX, double locationY, double directoin, BulletType typeBullet)
        {
            LocationX = locationX;
            LocationY = locationY;
            Direction = directoin;
            TypeBullet = typeBullet;
            Damage = 50;
        }
        public double LocationX { get; private set; }

        public double LocationY { get; private set; }
        public double Direction { get; private set; }
        public int Damage { get; private set; }

        public BulletType TypeBullet { get; private set; }

        public bool isHit { get; private set; }

        public Image GetFrameForAnimation()
        {
            return new Bitmap(1, 1);
        }

        public void Hit()
        {
            isHit = true;
        }

        public void Move()
        {
        }
    }
}
