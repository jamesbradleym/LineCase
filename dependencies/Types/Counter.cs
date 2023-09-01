using System.Diagnostics.Metrics;
using System.Reflection;
using Elements.Geometry;
using Elements.Geometry.Solids;
using LineCase;
using Newtonsoft.Json;

namespace Elements.Millwork
{
    public class Counter : Millwork
    {
        public double Overhang { get; set; }

        public Counter(Line line, MillworkOverride millworkOverride) : base(line, millworkOverride)
        {
            this.ID = millworkOverride.Identity.ID;
            this.Guide = line;
            this.Width = millworkOverride.Value.Width ?? 1;
            this.Height = millworkOverride.Value.Height ?? 1;
            this.Depth = millworkOverride.Value.Depth ?? 1;
            this.Type = millworkOverride.Value.MillworkType ?? "Counter";

            this.Overhang = millworkOverride.Value.CounterOverhang;

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
            var counterThickness = Units.InchesToMeters(2); // Adjust as needed
            var overhang = Overhang == -1 ? Units.InchesToMeters(6) : Overhang;
            var coverThickness = Units.InchesToMeters(0.5);

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
            var mLeft = new Extrude(sideProfile, Depth, Vector3.XAxis, false);
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
            var baseProfile = Polygon.Rectangle(Vector3.Origin, new Vector3(Depth - coverThickness, Width - sideThickness, 0));
            var mBase = new Extrude(baseProfile.TransformedPolygon(new Transform().Moved(0, sideThickness / 2, 0)), baseThickness, Vector3.ZAxis, false);
            var topTransform = new Transform()
                                    .Rotated(Vector3.YAxis, -90)
                                    .Moved(0, 0, Height - baseThickness - counterThickness);
            var mTop = new Extrude(topProfile.TransformedPolygon(topTransform), Depth - coverThickness, Vector3.XAxis, false);

            rep.SolidOperations.Add(mBase);
            rep.SolidOperations.Add(mTop);

            var coverProfile = Polygon.Rectangle(Vector3.Origin, new Vector3(coverThickness, Width, 0));
            var mCover = new Extrude(coverProfile.TransformedPolygon(new Transform().Moved(Depth - coverThickness, 0, 0)), Height, Vector3.ZAxis, false);

            rep.SolidOperations.Add(mCover);

            var counterProfile = Polygon.Rectangle(Vector3.Origin, new Vector3(Depth + overhang, Width, 0));
            var mCounter = new Extrude(counterProfile.TransformedPolygon(new Transform().Moved(0, 0, Height - counterThickness)), counterThickness, Vector3.ZAxis, false);

            rep.SolidOperations.Add(mCounter);

            this.Representation = rep;
        }


        public override void SetMaterial()
        {
            // Implementation for shelf material...

            // var materialName = this.Name + "Counter_MAT";
            // var materialColor = new Color(0.435294118, 0.68627451, 0.854901961, 1.0); // 6FB0DA with alpha 1
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