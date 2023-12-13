using Elements.Geometry;
using Elements.Geometry.Solids;
using Houses;

namespace Elements
{
    public partial class House
    {
        public override void UpdateRepresentations()
        {
            var extrude = new Extrude(Perimeter, Height, Vector3.ZAxis);
            Representation = new Representation(new List<SolidOperation> { extrude });
            if (Color.HasValue)
            {
                Material = new Material(Guid.NewGuid().ToString(), Color.Value);
            }
        }

        public House(Site site, double max, HousesInputsMaximumFootprintStrategy strategy, double height)
        {
            Console.WriteLine($"Creating house with {max} {strategy} and {height} height.");
            Height = height;
            var targetArea = strategy switch
            {
                HousesInputsMaximumFootprintStrategy.Area => max,
                HousesInputsMaximumFootprintStrategy.Ratio => site.Perimeter.Area() * max,
                _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null)
            };
            Console.WriteLine($"Target area: {targetArea}");

            // First simple offset strategy
            // OffsetUntilAreaIsLessThan(site.Perimeter, targetArea);

            // More advanced strategy offsets from the smallest side of perimeter
            var cuttingEdge = site.Perimeter.Segments().OrderBy(l => l.Length()).ElementAt(1);
            Polygon newPerimeter = TrimPerimeterFromOneSideRecursively(site.Perimeter.Offset(-1).FirstOrDefault(), targetArea, cuttingEdge);
            Perimeter = newPerimeter;
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
            Console.WriteLine($"Area: {area}, Target: {targetArea}");
            if (area <= targetArea)
            {
                return remaining.Perimeter;
            }
            else
            {
                return TrimPerimeterFromOneSideRecursively(remaining.Perimeter, targetArea, cuttingEdge, maxDepth - 1);
            }
        }

        private void OffsetUntilAreaIsLessThan(Polygon polygon, double targetArea)
        {
            var area = polygon.Area();
            Console.WriteLine($"Area: {area}, Target: {targetArea}");
            if (area <= targetArea)
            {
                Perimeter = polygon;
                return;
            }

            var offset = polygon.Offset(-0.1).FirstOrDefault();
            if (offset == null)
            {
                Perimeter = polygon;
                return;
            }

            OffsetUntilAreaIsLessThan(offset, targetArea);
        }
    }
}