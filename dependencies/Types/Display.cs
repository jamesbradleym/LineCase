using System.Reflection;
using Elements.Geometry;
using Elements.Geometry.Solids;
using IFC;
using LineCase;
using Newtonsoft.Json;

namespace Elements.Millwork
{
    public class Display : Millwork
    {
        public string Style { get; set; }
        public Display(Line line, MillworkOverride millworkOverride) : base(line, millworkOverride)
        {
            this.ID = millworkOverride.Identity.ID;
            this.Guide = line;
            this.Width = millworkOverride.Value.Width ?? 1;
            this.Height = millworkOverride.Value.Height ?? 1;
            this.Depth = millworkOverride.Value.Depth ?? 1;
            this.Style = "curved";
            this.Type = millworkOverride.Value.MillworkType ?? "Display";

            GenerateGeometry();
            SetMaterial();
            SetTransform();
        }

        public override void GenerateGeometry()
        {
            // Implementation for shelf geometry...

            var rep = new Representation();

            var baseThickness = Units.InchesToMeters(1);
            var sideThickness = Units.InchesToMeters(1);
            var standHeight = Height - Depth;

            if (standHeight > 0)
            {
                // Create a rectangular profile for the Millwork
                var profile = Polygon.Rectangle(Vector3.Origin, new Vector3(Depth, Width, 0));

                // Extrude the profile to height
                var stand = new Extrude(profile, standHeight, Vector3.ZAxis, false);

                rep.SolidOperations.Add(stand);
            }

            List<Extrude> extrudes;
            switch (Style)
            {
                case "squared":
                    extrudes = CreateSquaredSides(baseThickness, sideThickness);
                    break;
                case "curved":
                    extrudes = CreateCurvedSides(baseThickness, sideThickness, standHeight);
                    CreatedCurvedWindow(baseThickness, sideThickness, standHeight);
                    break;
                default:
                    extrudes = CreateSquaredSides(baseThickness, sideThickness);
                    break;
            }

            foreach (Extrude ex in extrudes)
            {
                rep.SolidOperations.Add(ex);
            }

            this.Representation = rep;
        }

        public override void SetMaterial()
        {
            // Implementation for shelf material...

            var wood = new Material("Wood", Colors.White, texture: "./Textures/Wood050_1K_Color.jpg");
            this.Material = wood;
        }

        private List<Extrude> CreateSquaredSides(double baseThickness, double sideThickness)
        {
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

            return new List<Extrude>() { mLeft, mRight };
        }

        private List<Extrude> CreateCurvedSides(double baseThickness, double sideThickness, double standHeight, int divisions = 20)
        {
            var pathOutter = new Arc(Depth, 0, 90).ToPolyline(divisions).Vertices.Select(v => new Vector3(v.X, v.Y, v.Z)).ToList();
            pathOutter.Insert(0, Vector3.Origin);
            pathOutter.Add(Vector3.Origin);
            var sideProfileOutter = new Polygon(
                pathOutter
            );

            var pathInner = new Arc(Depth - sideThickness, 0, 90).ToPolyline(divisions).Vertices.Select(v => new Vector3(v.X, v.Y, v.Z)).ToList();
            pathInner.Insert(0, Vector3.Origin);
            pathInner.Add(Vector3.Origin);
            var sideProfileInner = new Polygon(
                pathInner
            );

            sideProfileOutter.Transform(
                new Transform()
                    .Rotated(Vector3.XAxis, 90)
                    .Moved(new Vector3(0, 0, standHeight))
            );
            sideProfileInner.Transform(
                new Transform()
                    .Rotated(Vector3.XAxis, 90)
                    .Moved(new Vector3(0, 0, standHeight))
            );

            var mLeftOutter = new Extrude(sideProfileOutter, sideThickness / 2, Vector3.YAxis, false);
            var rightTransformOutter = new Transform()
                .Moved(new Vector3(0, Width - sideThickness / 2, 0));

            var mRightOutter = new Extrude(sideProfileOutter.TransformedPolygon(rightTransformOutter), sideThickness / 2, Vector3.YAxis, false);

            var leftTransformInner = new Transform()
                            .Moved(new Vector3(0, sideThickness / 2, 0));
            var mLeftInner = new Extrude(sideProfileInner.TransformedPolygon(leftTransformInner), sideThickness / 2, Vector3.YAxis, false);
            var rightTransformInner = new Transform()
                .Moved(new Vector3(0, Width - sideThickness, 0));

            var mRightInner = new Extrude(sideProfileInner.TransformedPolygon(rightTransformInner), sideThickness / 2, Vector3.YAxis, false);

            return new List<Extrude>() { mLeftOutter, mRightOutter, mLeftInner, mRightInner };
        }

        private void CreatedCurvedWindow(double baseThickness, double sideThickness, double standHeight, int divisions = 20)
        {
            var path = new Arc(Depth, 0, 90).ToPolyline(divisions * 2).Vertices.Select(v => new Vector3(v.X, v.Y, v.Z)).ToList();
            var pathInner = new Arc(Depth - sideThickness, 0, 90).ToPolyline(divisions).Vertices.Select(v => new Vector3(v.X, v.Y, v.Z)).ToList();
            path.Insert(0, pathInner[0]);
            pathInner.Reverse();
            path.AddRange(pathInner);
            var sideProfile = new Polygon(
                path
            );

            sideProfile.Transform(
                new Transform()
                    .Rotated(Vector3.XAxis, 90)
                    .Moved(new Vector3(0, sideThickness / 2, 0))
                    .Moved(new Vector3(0, 0, standHeight))
            );

            SubElements.Add(new SubElement(new Extrude(sideProfile, Width - sideThickness, Vector3.YAxis, false), "glass"));
        }
    }
}