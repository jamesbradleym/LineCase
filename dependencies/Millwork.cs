using System.Reflection;
using Elements.Geometry;
using Elements.Geometry.Solids;
using LineCase;
using Newtonsoft.Json;

namespace Elements.Millwork
{
    public partial class Millwork : GeometricElement
    {
        public Line Guide { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public string Type { get; set; }

        [JsonProperty("ID")]
        public string ID { get; set; }

        public Millwork(Line line, double width, double height, double depth, string id = null, string type = "Shelf")
        {
            this.Guide = line;
            this.Width = width;
            this.Height = height;
            this.Depth = depth;
            this.Type = type;

            GenerateGeometry();
            SetMaterial();
            SetTransform();

            this.ID = id ?? GetHashCode().ToString();
        }

        public Millwork(Line line, MillworkOverride millworkOverride)
        {
            this.ID = millworkOverride.Identity.ID;
            this.Guide = line;
            this.Width = millworkOverride.Value.Width ?? 1;
            this.Height = millworkOverride.Value.Height ?? 1;
            this.Depth = millworkOverride.Value.Depth ?? 1;
            this.Type = millworkOverride.Value.MillworkType ?? "Shelving";

            GenerateGeometry();
            SetMaterial();
            SetTransform();
        }

        public void Update(MillworkOverride millworkOverride)
        {

            this.Width = millworkOverride.Value.Width ?? this.Width;
            this.Depth = millworkOverride.Value.Depth ?? this.Depth;
            this.Height = millworkOverride.Value.Height ?? this.Height;
            this.Type = millworkOverride.Value.MillworkType ?? this.Type;

        }

        public virtual void GenerateGeometry()
        {
            var rep = new Representation();
            var solidRep = new Solid();

            // Create a rectangular profile for the Millwork
            var profile = Polygon.Rectangle(Vector3.Origin, new Vector3(Width, Depth, 0));

            // Extrude the profile to height
            var extrusion = new Extrude(profile, Height, Vector3.ZAxis, false);

            rep.SolidOperations.Add(extrusion);

            var consol = new ConstructedSolid(solidRep);
            rep.SolidOperations.Add(consol);

            this.Representation = rep;
        }

        public virtual void SetMaterial()
        {
            var materialName = this.Name + "_MAT";
            var materialColor = new Color(0.952941176, 0.360784314, 0.419607843, 1.0); // F15C6B with alpha 1
            var material = new Material(materialName)
            {
                Color = materialColor,
                Unlit = false
            };
            this.Material = material;
        }

        public void SetTransform()
        {
            this.Transform = new Transform().Rotated(Vector3.ZAxis, GetAngle(Vector3.XAxis, Guide.Direction()) - 90).Moved(Guide.Start);
        }

        private double GetAngle(Vector3 vectorA, Vector3 vectorB, double tolerance = 0.0001f)
        {
            // Calculate the dot product of the vectors
            double dotProduct = vectorA.Dot(vectorB);

            // Calculate the magnitudes of the vectors
            double magnitudeA = vectorA.Length();
            double magnitudeB = vectorB.Length();

            // Calculate the angle in radians using the arccosine function
            double angleRadians = (double)Math.Acos(dotProduct / (magnitudeA * magnitudeB));

            // Calculate the signed angle using the cross product to determine the direction
            Vector3 crossProduct = vectorA.Cross(vectorB);
            double signedAngle = Math.Sign(crossProduct.Z) * angleRadians;

            // Convert the angle from radians to degrees
            double angleDegrees = signedAngle * (180.0f / MathF.PI);

            double roundedAngle = Math.Abs(angleDegrees - Math.Round(angleDegrees)) <= tolerance
            ? (double)Math.Round(angleDegrees)
            : angleDegrees;

            return roundedAngle;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return Equals(obj as Millwork);
        }

        public bool Equals(Millwork other)
        {
            if (other == null)
                return false;

            return ArePropertiesEqual(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CalculatePropertiesHash());
        }

        private bool ArePropertiesEqual(Millwork other)
        {
            PropertyInfo[] properties = GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name != "ID" && !property.GetValue(this).Equals(property.GetValue(other)))
                    return false;
            }
            return true;
        }

        private int CalculatePropertiesHash()
        {
            List<string> HashProps = new List<string>() {
                "Guide"
            };
            PropertyInfo[] properties = GetType().GetProperties();
            int hash = 17;
            foreach (PropertyInfo property in properties)
            {
                if (HashProps.Contains(property.Name))
                {
                    hash = hash * 31 + property.GetValue(this).GetHashCode();
                }
            }
            return hash;
        }
    }
}
