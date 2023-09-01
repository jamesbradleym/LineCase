using System.Reflection;
using Elements.Geometry;
using Elements.Geometry.Solids;
using LineCase;
using Newtonsoft.Json;

namespace Elements.Millwork
{
    public class Bar : Millwork
    {
        public Bar(Line line, MillworkOverride millworkOverride) : base(line, millworkOverride)
        {
            this.ID = millworkOverride.Identity.ID;
            this.Guide = line;
            this.Width = millworkOverride.Value.Width ?? 1;
            this.Height = millworkOverride.Value.Height ?? 1;
            this.Depth = millworkOverride.Value.Depth ?? 1;
            this.Type = millworkOverride.Value.MillworkType ?? "Bar";

            GenerateGeometry();
            SetMaterial();
            SetTransform();
        }

        public override void GenerateGeometry()
        {
            // Implementation for shelf geometry...

            var rep = new Representation();
            var solidRep = new Solid();

            // Create a rectangular profile for the Millwork
            var profile = Polygon.Rectangle(Vector3.Origin, new Vector3(Depth, Width, 0));

            // Extrude the profile to height
            var extrusion = new Extrude(profile, Height, Vector3.ZAxis, false);

            rep.SolidOperations.Add(extrusion);

            var consol = new ConstructedSolid(solidRep);
            rep.SolidOperations.Add(consol);

            this.Representation = rep;
        }

        public override void SetMaterial()
        {
            // Implementation for shelf material...

            var wood = new Material("Wood", Colors.White, texture: "./Textures/Wood050_1K_Color.jpg");
            this.Material = wood;
        }
    }
}