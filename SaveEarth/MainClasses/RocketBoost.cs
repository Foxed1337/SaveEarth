using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveEarth.MainClasses
{
    class RocketBoost : IBoost
    {
        public double Direction { get; private set; }
        private double Velocity;
        private double TurnVelocity;
        public double LocationX { get; private set; }
        public double LocationY { get; private set; }
        public bool isTaken { get; private set; }
        private double Radius;
        private BoostType Boost;

        public RocketBoost(double radius, double direction, double turnVelocity)
        {
            Radius = radius;
            Direction = direction;
            LocationX = -Radius * Math.Sin(Direction);
            LocationY = Radius * Math.Cos(Direction);
            TurnVelocity = turnVelocity;
            Boost = BoostType.RocketBoost;
            Velocity = 5;
        }

        public void Move()
        {
            if (!isTaken)
            {
                if (Radius >= 50)
                {
                    Radius -= Velocity * 0.4;
                    LocationX = -Radius * Math.Sin(Direction);
                    LocationY = Radius * Math.Cos(Direction);
                    Direction += TurnVelocity * 0.007;
                }
                else isTaken = true;
            }
        }

        public void Take()
        {
            isTaken = true;
        }

        public Image GetFrameForAnimation()
        {
            return Image.FromFile("../../image/Sprites/Boosts/RocketBoost/RocketBoost_1.png");
        }

        public BoostType GiveBoost() => Boost;
    }
}
