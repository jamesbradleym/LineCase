using System.Reflection;
using Elements.Geometry;
using Elements.Geometry.Solids;
using LineCase;
using Newtonsoft.Json;

namespace Elements
{
    public class SubElement : GeometricElement
    {
        public Extrude extrude { get; set; }
        public string material { get; set; }

        public SubElement(Extrude extrude, string material)
        {
            this.extrude = extrude;
            this.material = material;

            GenerateGeometry();
            SetMaterial();
        }

        public void SetTransform(Transform transform)
        {
            this.Transform = transform;
        }

        public void GenerateGeometry()
        {
            // Implementation for shelf geometry...

            var rep = new Representation();

            rep.SolidOperations.Add(extrude);

            this.Representation = rep;
        }

        public void SetMaterial()
        {
            var glass = new Material("Glass", new Color(0.678, 0.847, 0.902, 0.784), .5, .9);

            switch (this.material)
            {
                case "glass":
                    this.Material = glass;
                    break;
                default:
                    this.Material = glass;
                    break;
            }
        }
    }
}