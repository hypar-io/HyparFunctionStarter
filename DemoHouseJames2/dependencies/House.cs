using Elements.Geometry;

namespace Elements
{
    public partial class House
    {
        public override void UpdateRepresentations()
        {
            var extrude = new Elements.Geometry.Solids.Extrude(this.Perimeter, this.Height, Vector3.ZAxis, false);
            Representation = new Representation(new[] { extrude });
        }

        public House(Site site, double height, Color color)
        {
            var perimeter = site.Perimeter.Offset(-0.7).First();

            this.Perimeter = perimeter;
            Height = height;
            Color = color;

            Material = new Material(Guid.NewGuid().ToString(), color);
        }
    }
}