using System.Reflection;
using Elements.Geometry;
using Elements.Geometry.Solids;
using LineCase;
using Newtonsoft.Json;

namespace Elements.Millwork
{
    public class Shelving : Millwork
    {
        public int ShelfCount { get; set; }
        public bool Open { get; set; }

        public Shelving(Line line, MillworkOverride millworkOverride) : base(line, millworkOverride)
        {
            this.ID = millworkOverride.Identity.ID;
            this.Guide = line;
            this.Width = millworkOverride.Value.Width ?? 1;
            this.Height = millworkOverride.Value.Height ?? 1;
            this.Depth = millworkOverride.Value.Depth ?? 1;
            this.Type = millworkOverride.Value.MillworkType ?? "Shelving";

            this.ShelfCount = millworkOverride.Value.ShelfCount;
            this.Open = millworkOverride.Value.Open;

            GenerateGeometry();
            SetMaterial();
            SetTransform();
        }

        public override void GenerateGeometry()
        {
            var rep = new Representation();

            // Define the dimensions of the shelving components
            var baseThickness = Units.InchesToMeters(1);
            var sideThickness = Units.InchesToMeters(1);
            var shelfThickness = Units.InchesToMeters(0.5); // Adjust as needed
            var supportDepth = Open ? 0 : Units.InchesToMeters(0.5);   // Adjust as needed

            var shelfCount = ShelfCount == -1 ? Math.Max((int)(Height / Units.InchesToMeters(6)), 3) : ShelfCount + 1; // Adjust shelf spacing as needed

            // Create the main structure of the shelving

            var sideProfile = new Polygon(
                new List<Vector3>(){
                    Vector3.Origin,
                    new Vector3(0,Height-(baseThickness/2),0),
                    new Vector3(sideThickness,Height-(baseThickness/2),0),
                    new Vector3(sideThickness,baseThickness,0),
                    new Vector3(sideThickness/2,baseThickness,0),
                    new Vector3(sideThickness/2,0,0),
                    Vector3.Origin
                }
            );
            sideProfile.Transform(
                new Transform()
                    .Rotated(Vector3.XAxis, 90)
                    .Rotated(Vector3.ZAxis, 90));
            // var sideProfile = Polygon.Rectangle(Vector3.Origin, new Vector3(Depth, sideThickness, 0));
            var mLeft = new Extrude(sideProfile, Depth, Vector3.XAxis, false);
            // var rightTransform = new Transform(0, Width - sideThickness, 0);
            var rightTransform = new Transform()
                .RotatedAboutPoint(new Vector3(Depth / 2, Width / 2, 0), Vector3.ZAxis, 180)
                .Moved(new Vector3(-Depth, 0, 0));
            var mRight = new Extrude(sideProfile.TransformedPolygon(rightTransform), Depth, Vector3.XAxis, false);

            rep.SolidOperations.Add(mLeft);
            rep.SolidOperations.Add(mRight);

            var topProfile = new Polygon(
                        new List<Vector3>(){
                new Vector3(baseThickness/2,0,0),
                new Vector3(baseThickness,0,0),
                new Vector3(baseThickness,Width,0),
                new Vector3(baseThickness/2,Width,0),
                new Vector3(baseThickness/2,Width-sideThickness,0),
                new Vector3(0,Width-sideThickness,0),
                new Vector3(0,sideThickness,0),
                new Vector3(baseThickness/2,sideThickness,0),
                new Vector3(baseThickness/2,0,0),
                }
            );
            var baseProfile = Polygon.Rectangle(Vector3.Origin, new Vector3(Depth, Width - sideThickness, 0));
            var mBase = new Extrude(baseProfile.TransformedPolygon(new Transform().Moved(0, sideThickness / 2, 0)), baseThickness, Vector3.ZAxis, false);
            var topTransform = new Transform().Rotated(Vector3.YAxis, -90).Moved(0, 0, Height - baseThickness);
            var mTop = new Extrude(topProfile.TransformedPolygon(topTransform), Depth, Vector3.XAxis, false);

            rep.SolidOperations.Add(mBase);
            rep.SolidOperations.Add(mTop);

            // Create the shelves
            for (int i = 1; i < shelfCount; i++)
            {
                var shelfHeight = Height * i / shelfCount;
                var startPoint = Vector3.Origin + new Vector3(0, 0, shelfHeight - shelfThickness / 2);
                var shelfProfile = Polygon.Rectangle(Vector3.Origin, new Vector3(Depth - supportDepth, Width - (2 * sideThickness), 0));

                shelfProfile.Transform(new Transform(supportDepth, sideThickness, shelfHeight - shelfThickness / 2));
                var shelf = new Extrude(shelfProfile, shelfThickness, Vector3.ZAxis);

                rep.SolidOperations.Add(shelf);
            }

            // Create the vertical supports
            var supportCount = Open ? 0 : shelfCount;
            for (int i = 0; i < supportCount; i++)
            {
                var supportHeight = Height * (i + 0.5) / shelfCount;
                var supportProfile = Polygon.Rectangle(Vector3.Origin, new Vector3(supportDepth, Width - (2 * sideThickness), 0));
                supportProfile.Transform(new Transform().Moved(0, sideThickness, 0));
                // var support = new Box(Vector3.Origin + new Vector3(0, 0, supportHeight),
                //   Width, Depth - supportDepth, supportDepth);
                var support = new Extrude(supportProfile, supportHeight, Vector3.ZAxis);
                rep.SolidOperations.Add(support);
            }

            this.Representation = rep;
        }


        public override void SetMaterial()
        {
            // Implementation for shelf material...

            // var materialName = this.Name + "Shelving_MAT";
            // var materialColor = new Color(0.388235294, 0.250980392, 0.57254902, 1.0); // 633F92 with alpha 1
            // var material = new Material(materialName)
            // {
            //     Color = materialColor,
            //     Unlit = true
            // };
            // this.Material = material;

            var wood = new Material("Wood", Colors.White, texture: "./Textures/Wood050_1K_Color.jpg");
            this.Material = wood;
        }
    }
}