using Elements.Geometry;
using Elements.Geometry.Solids;

namespace Elements
{
    public partial class House
    {
        public override void UpdateRepresentations()
        {
            var extrude = new Extrude(this.Perimeter, this.Height, Vector3.ZAxis);
            Representation = new Representation(new List<SolidOperation> { extrude });
        }

        public House(Site lot, double height, Color color, double targetArea)
        {
            this.Height = height;
            var perimeter = TrimPerimeterFromOneSideRecursively(lot.Perimeter.Offset(-0.7).First(), targetArea, lot.Perimeter.Segments().OrderBy(s => s.Length()).First());
            this.Perimeter = perimeter;
            this.Color = color;

            this.Material = new Material(Guid.NewGuid().ToString(), Color.Value);
        }

        private Polygon TrimPerimeterFromOneSideRecursively(Polygon perimeter, double targetArea, Line cuttingEdge, int maxDepth = 100)
        {
            if (maxDepth == 0)
            {
                return perimeter;
            }
            var distanceToStep = 0.1;

            var directionOfStep = perimeter.Centroid() - perimeter.Centroid().ClosestPointOn(cuttingEdge);
            var step = directionOfStep * distanceToStep;

            var profile = new Profile(perimeter);
            cuttingEdge = cuttingEdge.TransformedLine(new Transform(step)).ExtendTo(profile);
            var remaining = Profile.Split(new[] { profile }, new Polyline(new[] { cuttingEdge.Start, cuttingEdge.End })).OrderByDescending(p => p.Area()).First();

            var area = remaining.Area();
            if (area <= targetArea)
            {
                return remaining.Perimeter;
            }
            else
            {
                return TrimPerimeterFromOneSideRecursively(remaining.Perimeter, targetArea, cuttingEdge, maxDepth - 1);
            }
        }
    }
}