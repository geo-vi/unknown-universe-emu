using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.objects.world
{
    class Vector
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public double DistanceTo(Vector point)
        {
            return Math.Sqrt(Math.Pow(point.X - X, 2) + Math.Pow(point.Y - Y, 2));
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static bool IsPositionInCircle(Vector position, Vector circleCenter, float radius)
        {
            return Math.Sqrt(((circleCenter.X - position.X) * (circleCenter.X - position.X)) + ((circleCenter.Y - position.Y) * (circleCenter.Y - position.Y))) < radius;
        }

        public static bool IsInRange(Vector position, Vector center, float radius)
        {
            return Math.Sqrt(((center.X - position.X) * (center.X - position.X)) + ((center.Y - position.Y) * (center.Y - position.Y))) < radius;
        }

        public string ToPacket()
        {
            return X + "|" + Y;
        }

        public Vector GetCloserVector(Vector position1, Vector position2)
        {
            if (DistanceTo(position1) > DistanceTo(position2))
                return position2;

            return position1;
        }

        public Character GetCloserCharacter(Character character1, Character character2)
        {
            if (character1 == null || character2 == null) return null;
            if (DistanceTo(character1.Position) > DistanceTo(character2.Position))
                return character2;
            return character1;
        }

        public static Vector Random(Spacemap map, int minX = 0, int maxX = 0, int minY = 0, int maxY = 0)
        {
            if (minX == 0) minX = map.Limits[0].X;
            if (minY == 0) minY = map.Limits[0].Y;
            if (maxX == 0) maxX = map.Limits[1].X;
            if (maxY == 0) maxY = map.Limits[1].Y;

            var posX = NettyBaseReloaded.Random.Next(minX, maxX);
            var posY = NettyBaseReloaded.Random.Next(minY, maxY);
            return new Vector(posX, posY);
        }

        public static Vector GetPosOnCircle(Vector circleCenter, int radius)
        {
            var a = NettyBaseReloaded.Random.Next(0, 360);
            var calculateX = circleCenter.X + Convert.ToInt32(radius * Math.Cos(a));
            var calculateY = circleCenter.Y + Convert.ToInt32(radius * Math.Sin(a));

            return new Vector(calculateX, calculateY);
        }

        public static Vector GetPosOnCircle(Vector circleCenter, int angle, int radius)
        {
            var calculateX = circleCenter.X + Convert.ToInt32(radius * Math.Cos(angle));
            var calculateY = circleCenter.Y + Convert.ToInt32(radius * Math.Sin(angle));

            return new Vector(calculateX, calculateY);
        }

        public static Vector FromVector(Vector origin, int xDist, int yDist)
        {
            return new Vector(origin.X + xDist, origin.Y + yDist);
        }

        public double GetAngle(Vector secPoint)
        {
            float xDiff = secPoint.X - X;
            float yDiff = secPoint.Y - Y;
            return Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;
        }
    }
}
