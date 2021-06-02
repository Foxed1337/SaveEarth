using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveEarth.MainClasses
{
    class AlienRocket : IBullet
    {       
        public AlienRocket(double locationX, double locationY, double directoin)
        {
            LocationX = locationX;
            LocationY = locationY;
            Direction = directoin;
            Radius = Math.Sqrt(locationX * locationX + locationY * locationY);
            TypeBullet = BulletType.AlienBullet;
            Damage = 40;
        }


        public double LocationX { get; private set; }
        public double LocationY { get; private set; }
        public double Direction { get; private set; }
        public int Damage { get; private set; }
        public BulletType TypeBullet { get; private set; }
        public bool isHit { get; private set; }
        

        private double Radius;
        private int currentRocketFrame = 1;
        private double Velocity = 0;

        public Image GetFrameForAnimation()
        {
            AnimateRocket();
            return Image.FromFile("../../image/Sprites/Rockets/AlienRocket/Rocket_" + currentRocketFrame.ToString() + ".png");
        }

        public void AnimateRocket()
        {
            if (currentRocketFrame >= 3)
                currentRocketFrame = 1;
            currentRocketFrame++;
        }

        public void Hit()
        {
            isHit = true;
        }

        public void Move()
        {
            Radius -= Velocity;
            LocationX = -Radius * Math.Sin(Direction);
            LocationY = Radius * Math.Cos(Direction);
            Velocity += 0.5;
        }
    }
}
