using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace SaveEarth.MainClasses
{
    public class AttackAlien : IAlien
    {
        public AttackAlien(double radius, double direction, double turnVeloncity, int healthPoint = 40, string imageName = "AttackAlien")
        {

            HealthPoint = healthPoint;
            Radius = radius;
            Direction = direction;
            LocationX = -Radius * Math.Sin(Direction);
            LocationY = Radius * Math.Cos(Direction);
            TurnVelocity = turnVeloncity;
            HitBox = 30;
            KillPoints = 15;
            AttackRange = 350;
            isDropBoost = false;
            attackType = AttackType.Bullet;
        }

        static SoundPlayer sound = new SoundPlayer("../../Sounds/Взрыв.wav");

        public double LocationX { get; private set; }
        public double LocationY { get; private set; }
        public double Direction { get; private set; }
        public double AttackRange { get; private set; }
        public bool isDropBoost { get; private set; }
        public int HitBox { get; private set; }
        public double Radius { get; private set; }
        public int KillPoints { get; private set; }
        public bool isDead { get { return (HealthPoint <= 0); } }
        public bool CanDeleteAlien { get; private set; }
        public AttackType attackType { get; private set; }

        private int currenAlienFrame = 1;
        private int currenAlienFrameDead = 1;
        private int HealthPoint;
        private double Velocity = 20;
        private double TurnVelocity;
        private int attackCounter;// если счетчик атаки равен нулю, то Alien может сделать свою атаку

        public void Move(double dt)
        {
            if (!(HealthPoint <= 0))
            {
                if (Radius >= 300)
                {
                    Radius -= Velocity * dt;
                    LocationX = -Radius * Math.Sin(Direction);
                    LocationY = Radius * Math.Cos(Direction);
                }
                else
                {
                    Direction += TurnVelocity * 0.0005;
                    LocationX = -Radius * Math.Sin(Direction);
                    LocationY = Radius * Math.Cos(Direction);
                }
            }
        }

        public Image GetFrameForAnimation()
        {
            if (!(HealthPoint <= 0))
                return Image.FromFile("../../image/Sprites/Aliens/AttackAlien/AttackAlien_" + currenAlienFrame.ToString() + ".png");
            else return Image.FromFile("../../image/Sprites/Bang/bang_" + currenAlienFrameDead.ToString() + ".png");
        }

        public void AnimateAlien()
        {
            if (!(HealthPoint <= 0))
            {
                if (currenAlienFrame > 3)
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

        private void PlaySound(int Frame)
        {
            if (Frame == 1)
                sound.Play();
        }

        public void TakeDamage(int damage)
        {
            HealthPoint -= damage;
        }


        public void ChangeBoostStatus()
        {
            isDropBoost = true;
        }


        public bool CanDoAttack()
        {
            //т.к. Земля всегда находится в центре игрового поля, то координаты цели инопланетян всегда (0,0)
            if (Radius <= AttackRange && !isDead)
            {
                if (attackCounter == 0)
                {
                    attackCounter = 40;
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

    }
}
