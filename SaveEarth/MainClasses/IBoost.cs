using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveEarth.MainClasses
{
    public interface IBoost
    {
        double Direction { get; }
        double LocationX { get; }
        double LocationY { get; }
        bool isTaken { get; }
        void Move();
        void Take();
        Image GetFrameForAnimation();
        BoostType GiveBoost();
    }
}
