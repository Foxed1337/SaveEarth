using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveEarth.MainClasses
{
    public class Planet
    {
        public Planet(double locationX, double locationy, int healthPoint)
        {
            LocationX = locationX;
            LocationY = locationy;
            HealthPoint = healthPoint;
            MaxHealthPoint = healthPoint;
            HitBox = 100;
        }

        public double LocationX { get; private set; }
        public double LocationY { get; private set; }
        public int HealthPoint { get; private set; }
        public int MaxHealthPoint { get; private set; }
        public int HitBox { get; private set; }
        private int currentEarthFrame = 1;
        private int currenAlienFrameDead = 1;
        public bool isDead { get { return HealthPoint <= 0; } }
        public bool isCanEnd { get; private set; }


        public void TakeDamage(int damege)
        {
            HealthPoint -= damege;
            if (HealthPoint < 0) HealthPoint = 0;
            if (HealthPoint > MaxHealthPoint) HealthPoint = MaxHealthPoint;
        }

        public Image GetFrameForAnimation()
        {
            if (!isDead)
                return Image.FromFile("../../image/Sprites/Earth/LifeOfEarth/Earth_" + currentEarthFrame.ToString() + ".png");
            else return Image.FromFile("../../image/Sprites/Earth/DeadOfEarth/Earth_" + currenAlienFrameDead.ToString() + ".png");
        }

        public void AnimateEarth()
        {
            if (!isDead)
            {
                if (currentEarthFrame > 10)
                    currentEarthFrame = 1;
                currentEarthFrame++;
            }
            else
            {
                if (currenAlienFrameDead > 9)
                {
                    isCanEnd = true;
                    currenAlienFrameDead = 9;
                }
                currenAlienFrameDead++;
            }
        }

    }
}
