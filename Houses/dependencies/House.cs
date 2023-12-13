using Elements.Geometry;
using Elements.Geometry.Solids;

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
    }
}