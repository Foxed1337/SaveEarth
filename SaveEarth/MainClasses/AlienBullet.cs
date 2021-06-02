using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveEarth.MainClasses
{
    class AlienBullet : IBullet
    {
        public double Direction { get; private set; }
        public int Damage { get; private set; }

        private double Velocity;
        public double LocationX { get; private set; }
        public double LocationY { get; private set; }
        public BulletType TypeBullet { get; private set; }
        public bool isHit { get; private set; }
        private double Radius;

       
        public AlienBullet(double locationX, double locationY, double directoin, BulletType typeBullet)
        {
            LocationX = locationX;
            LocationY = locationY;
            Direction = directoin;
            Radius = Math.Sqrt(locationX * locationX + locationY * locationY);
            TypeBullet = typeBullet;
            Damage = 10;
            Velocity = -20;
        }

        public void Move()
        {
            Radius += Velocity;
            LocationX = -Radius * Math.Sin(Direction);
            LocationY = Radius * Math.Cos(Direction);
        }

        public void Hit()
        {
            isHit = true;
        }

        public Image GetFrameForAnimation()
        {
            return Image.FromFile("../../image/Sprites/Bullets/AlienBullet.png");
        }
    }
}

