using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;


namespace SaveEarth.MainClasses
{
    public class Level
    {
        public Planet CurrentPlanet { get; private set; }
        public AirPlane AirPlane { get; private set; }
        public int maxAliensInBattle { get; private set; }
        public int Score { get; private set; }


        private int currentNumberAliensInBattle = 0;
        private Size ClientSize;
        static private SoundPlayer hurtSound = new SoundPlayer("../../Sounds/Hurt_1.wav");
        static private Random rnd = new Random(DateTime.Now.Millisecond);


        private HashSet<IAlien> aliensInBattle = new HashSet<IAlien>();
        private HashSet<IBullet> bulletsInBattle = new HashSet<IBullet>();
        private HashSet<IBoost> BoostsInBattle = new HashSet<IBoost>();

        public int ChanceBoostDrop { get; private set; }

        public Level(int numberAliensInBattle, int numberOfAvailableRocket, int chanceBoostDrop, int earthHP, Size clientSize)
        {
            maxAliensInBattle = numberAliensInBattle;
            CurrentPlanet = new Planet(0, 0, earthHP);
            AirPlane = new AirPlane(150, Math.PI, numberOfAvailableRocket);
            ClientSize = clientSize;
            Score = 0;
            ChanceBoostDrop = chanceBoostDrop;
            AddNewAlienInBattle();
        }

        //public bool IsCompleted => totalAliensOnLevel == 0;
        public bool IsLost => CurrentPlanet.isCanEnd;

        public IEnumerable<IBullet> GetBullets()
        {
            foreach (var bullet in bulletsInBattle)
            {
                yield return bullet;
            }
        }

        public IEnumerable<IAlien> GetAliens()
        {
            foreach (var alien in aliensInBattle)
            {
                yield return alien;
            }
        }

        public IEnumerable<IBoost> GetBosts()
        {
            foreach (var boost in BoostsInBattle)
            {
                yield return boost;
            }
        }


        private void AddBoostToBattle(IAlien alien)
        {
            int boostType = rnd.Next(1, ChanceBoostDrop);
            switch (boostType)
            {
                case 1:
                    BoostsInBattle.Add(new HealthPointsBoost(alien.Radius, alien.Direction, rnd.Next(-3, 3)));
                    break;
                case 3:
                    BoostsInBattle.Add(new RocketBoost(alien.Radius, alien.Direction, rnd.Next(-3, 3)));
                    break;
                default:
                    break;
            }
        }


        public void AddAirPlaneBulletToBattle()
        {
            bulletsInBattle.Add(new AirPlaneBullet(AirPlane.Radius, AirPlane.Direction));
        }

        public void AddRocketToBattleForAirPlane()
        {
            if (!(AirPlane.NumberOfAvailableRocket == 0))
            {
                bulletsInBattle.Add(new AirPlaneRocket(AirPlane.Radius, AirPlane.Direction));
                AirPlane.DeleteRocketFromTotalNumber();
            }
        }

        public void AlienAttack(IAlien alien)
        {
            switch (alien.attackType)
            {
                case AttackType.Bullet:
                    bulletsInBattle.Add(new AlienBullet(alien.LocationX, alien.LocationY, alien.Direction, BulletType.AlienBullet));
                    break;
                case AttackType.Rocket:
                    bulletsInBattle.Add(new AlienRocket(alien.LocationX, alien.LocationY, alien.Direction));
                    break;
                case AttackType.Kamikaze:
                    bulletsInBattle.Add(new KamikazeBullet(alien.LocationX, alien.LocationY, alien.Direction, BulletType.AlienBullet));
                    break;
                default:
                    break;
            }
        }


        public void DeleteBulletFromBattle(Size gameAreaSize)
        {
            bulletsInBattle = bulletsInBattle.Where(b =>
            Math.Abs(b.LocationY) < gameAreaSize.Height
            && Math.Abs(b.LocationX) < gameAreaSize.Width
            && !b.isHit).ToHashSet();
        }

        public void DeleteDeadAlienFromBattle()
        {
            var tempHash = new HashSet<IAlien>();
            foreach (var alien in aliensInBattle)
            {
                if (alien.isDead)
                {
                    if (!alien.isDropBoost)
                    {
                        AddBoostToBattle(alien);
                        alien.ChangeBoostStatus();
                        Score += alien.KillPoints;
                    }
                }
                if (!alien.CanDeleteAlien)
                    tempHash.Add(alien);
            }
            aliensInBattle = tempHash;
            currentNumberAliensInBattle = aliensInBattle.Count();
            if (currentNumberAliensInBattle < maxAliensInBattle)
                AddNewAlienInBattle();

        }

        public void DeleteTakenBoosts()
        {
            BoostsInBattle = BoostsInBattle.Where(b => !b.isTaken).ToHashSet();
        }


        private void AddNewAlienInBattle()
        {
            while (maxAliensInBattle > currentNumberAliensInBattle)
            {
                int alienTipe = rnd.Next(1, 6);
                switch (alienTipe)
                {
                    case 1:
                    case 4:
                        aliensInBattle.Add(new AttackAlien(rnd.Next(ClientSize.Width / 2 + 50, ClientSize.Width / 2 + 70), rnd.Next(0, 100), rnd.Next(-3, 3)));
                        break;
                    case 2:
                    case 5:
                        aliensInBattle.Add(new KamikazeAlien(rnd.Next(ClientSize.Width / 2 + 50, ClientSize.Width / 2 + 70), rnd.Next(0, 100), rnd.Next(-4, 4)));
                        break;
                    case 3:
                    case 6:
                        aliensInBattle.Add(new RocketAlien(rnd.Next(ClientSize.Width / 2 + 50, ClientSize.Width / 2 + 70), rnd.Next(0, 100), rnd.Next(-4, 4)));
                        break;
                    default:
                        break;
                }
                currentNumberAliensInBattle++;
            }
        }


        public void CheckForBoost()
        {
            foreach (var boost in BoostsInBattle)
            {
                if ((Math.Abs(boost.LocationX - AirPlane.LocationX) < 50 && Math.Abs(boost.LocationY - AirPlane.LocationY) < 50))
                    switch (boost.GiveBoost())
                    {
                        case BoostType.RocketBoost:
                            {
                                AirPlane.AddRocketToTotalNumber();
                                boost.Take();
                            }
                            break;
                        case BoostType.HealthPointsBoost:
                            {
                                CurrentPlanet.TakeDamage(-50);
                                boost.Take();
                            }
                            break;
                        default:
                            break;
                    }
            }
        }


        public void CheckForHits()
        {
            foreach (var bullet in bulletsInBattle)
            {
                if (bullet.TypeBullet == BulletType.AirPlaneBullet)
                {
                    foreach (var alien in aliensInBattle)
                    {
                        if (Math.Abs(bullet.LocationX - alien.LocationX) < alien.HitBox + 10 && Math.Abs(bullet.LocationY - alien.LocationY) < alien.HitBox)
                        {
                            if (alien.isDead)
                                break;
                            bullet.Hit();
                            alien.TakeDamage(bullet.Damage);
                            hurtSound.Play();
                            break;
                        }
                    }
                }
                if (bullet.TypeBullet == BulletType.AlienBullet)
                {
                    if ((Math.Abs(bullet.LocationX - CurrentPlanet.LocationX) < CurrentPlanet.HitBox && Math.Abs(bullet.LocationY - CurrentPlanet.LocationY) < CurrentPlanet.HitBox))
                    {
                        CurrentPlanet.TakeDamage(bullet.Damage);
                        bullet.Hit();
                    }
                }
            }
        }
    }
}
