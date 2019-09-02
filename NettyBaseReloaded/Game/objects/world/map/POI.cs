using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.objects.world.map.pois;

namespace NettyBaseReloaded.Game.objects.world.map
{
    class POI
    {
        /// <summary>
        /// POI Id
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Type of POI
        /// </summary>
        public Types Type { get; set; }

        /// <summary>
        /// Design of POI
        /// </summary>
        public Designs Design { get; set; }

        /// <summary>
        /// Shape of POI
        /// </summary>
        public Shapes Shape { get; set; }

        public List<Vector> ShapeCords { get; set; }

        /// <summary>
        /// If POI is inverted
        /// </summary>
        public bool Inverted { get; set; }

        /// <summary>
        /// More details for POI
        /// </summary>
        public string TypeSpecification { get; set; }

        public bool Active { get; set; }

        public POI(string id, Types type, Designs design, Shapes shape, List<Vector> shapeCords, bool active = true, bool inverted = false, string poiTypeSpecification = "")
        {
            Id = id;
            Type = type;
            Design = design;
            Shape = shape;
            if (shapeCords.Count == 2)
            {
                shapeCords.Add(new Vector(shapeCords[0].X, shapeCords[1].Y));
                shapeCords.Add(new Vector(shapeCords[1].X, shapeCords[0].Y));
            }
            ShapeCords = shapeCords;
            Inverted = inverted;
            TypeSpecification = poiTypeSpecification;
            Active = active;
        }

        public List<int> ShapeCordsToInts()
        {
            List<int> cords = new List<int>();
            foreach (var cord in ShapeCords)
            {
                cords.Add(cord.X);
                cords.Add(cord.Y);
            }
            return cords;
        }

        public bool IsVectorInShape(Vector position)
        {
            switch (Shape)
            {
                case Shapes.RECTANGLE:
                    var lowestY = ShapeCords.OrderByDescending(x => x.Y).First();
                    var highestY = ShapeCords.OrderBy(x => x.Y).First();
                    var lowestX = ShapeCords.OrderByDescending(x => x.X).First();
                    var highestX = ShapeCords.OrderBy(x => x.X).First();

                    // in terms check
                    if (position.X > lowestX.X && position.X < highestX.X && position.Y > lowestY.Y &&
                        position.Y < highestY.Y)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        public Vector GetCenterVector()
        {
            switch (Shape)
            {
                case Shapes.RECTANGLE:
                    var lowestY = ShapeCords.OrderByDescending(x => x.Y).First();
                    var highestY = ShapeCords.OrderBy(x => x.Y).First();
                    var lowestX = ShapeCords.OrderByDescending(x => x.X).First();
                    var highestX = ShapeCords.OrderBy(x => x.X).First();

                    // in terms check
                    return new Vector((highestX.X + lowestX.X) / 2, (highestY.Y + lowestY.Y) / 2);
                    break;
            }

            return ShapeCords[0];
        }
    }
}