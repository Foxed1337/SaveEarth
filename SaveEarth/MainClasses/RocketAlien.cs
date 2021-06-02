using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace SaveEarth.MainClasses
{
    class RocketAlien : IAlien
    {
        public RocketAlien(double radius, double direction, double turnVeloncity, int healthPoint = 30, string imageName = "RocketAlien")
        {

            HealthPoint = healthPoint;
            Radius = radius;
            Direction = direction;
            LocationX = -Radius * Math.Sin(Direction);
            LocationY = Radius * Math.Cos(Direction);
            TurnVeloncity = turnVeloncity;
            HitBox = 30;
            KillPoints = 20;
            AttackRange = 500;
            isDropBoost = false;
            Velocity = 25;
            attackType = AttackType.Rocket;
        }

        static SoundPlayer sound = new SoundPlayer("../../Sounds/Взрыв.wav");

        public double LocationX { get; private set; }
        public double LocationY { get; private set; }
        public double Direction { get; private set; }
        public double Radius { get; private set; }
        public int KillPoints { get; private set; }
        public bool isDead { get { return (HealthPoint <= 0); }}
        public int HitBox { get; private set; }
        public bool CanDeleteAlien { get; private set; }
        public bool isDropBoost { get; private set; }
        public AttackType attackType { get; private set; }

        private double AttackRange;
        private int currenAlienFrameDead = 1;
        private int currenAlienFrame = 1;
        private int HealthPoint;      
        private double Velocity;
        private double TurnVeloncity;     
        private int attackCounter;


        public void AnimateAlien()
        {
            if (!(HealthPoint <= 0))
            {
                if (currenAlienFrame > 5)
                    currenAlienFrame = 1;
                currenAlienFrame++;
            }
            else
            {
                PlaySound(currenAlienFrameDead);
                if (currenAlienFrameDead > 5)
                {
                    currenAlienFrameDead = 1;
                    CanDeleteAlien = true;
                }
                currenAlienFrameDead++;
            }
        }

        private void PlaySound(int frame)
        {
            if (frame == 1)
                sound.Play();
            sound.LoadAsync();
        }

        public bool CanDoAttack()
        {
            if (Radius <= AttackRange && !isDead)
            {
                if (attackCounter == 0)
                {
                    attackCounter = 200;
                    return true;
                }
                else
                {
                    attackCounter--;
                    return false;
                }
            }
            else return false;
        }

        public Image GetFrameForAnimation()
        {
            if (!(HealthPoint <= 0))
                return Image.FromFile("../../image/Sprites/Aliens/RocketAlien/RocketAlien_" + currenAlienFrame.ToString() + ".png");
            else return Image.FromFile("../../image/Sprites/Bang/bang_" + currenAlienFrameDead.ToString() + ".png");
        }


        public void Move(double dt)
        {
            if (!(HealthPoint <= 0))
            {
                if (Radius >= 350)
                {
                    Radius -= Velocity * dt;
                    LocationX = -Radius * Math.Sin(Direction);
                    LocationY = Radius * Math.Cos(Direction);
                    Direction += 0.002;
                }
                else
                {
                    Direction += TurnVeloncity * 0.0005;
                    LocationX = -Radius * Math.Sin(Direction);
                    LocationY = Radius * Math.Cos(Direction);
                }

            }
        }

        public void TakeDamage(int damage)
        {
            HealthPoint -= damage;
        }

        public void ChangeBoostStatus()
        {
            isDropBoost = true;
        }
    }
}

