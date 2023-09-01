using Elements.Geometry;
using Elements.Geometry.Solids;
using LineCase;
using Newtonsoft.Json;

namespace Elements
{
    public class Guide : GeometricElement
    {
        public Polyline Polyline { get; set; }
        [JsonProperty("Add Id")]
        public string AddId { get; set; }
        public bool ShowPoints { get; set; }

        public Guide(GuidesOverrideAddition add, bool showpoints = true)
        {
            this.Polyline = add.Value.Polyline;
            this.AddId = add.Id;

            this.ShowPoints = showpoints;
            SetMaterial();
        }

        public Guide(string id, Polyline polyline, bool showpoints = true)
        {
            Polyline = polyline;
            this.AddId = id;
            this.ShowPoints = showpoints;
            SetMaterial();
        }

        public bool Match(GuidesIdentity identity)
        {
            return identity.AddId == this.AddId;
        }

        public Guide Update(GuidesOverride edit)
        {
            this.Polyline = edit.Value.Polyline;
            return this;
        }

        public void SetMaterial()
        {
            var materialName = this.Name + "_MAT";
            var materialColor = new Color(0.952941176, 0.360784314, 0.419607843, 1.0); // F15C6B with alpha 1
            var material = new Material(materialName);
            material.Color = materialColor;
            material.Unlit = true;
            this.Material = material;
        }

        public override void UpdateRepresentations()
        {
            var rep = new Representation();
            var solidRep = new Solid();

            // Define parameters for the extruded circle and spherical point
            var circleRadius = 0.025;
            var pointRadius = 0.05;

            // Create an extruded circle along each line segment of the polyline
            for (int i = 0; i < Polyline.Vertices.Count - 1; i++)
            {
                var start = Polyline.Vertices[i];
                var end = Polyline.Vertices[i + 1];
                var direction = Polyline.Segments()[i].Direction();
                var length = Polyline.Segments()[i].Length();

                var circle = new Elements.Geometry.Circle(Vector3.Origin, circleRadius).ToPolygon(10);
                circle.Transform(new Transform(new Plane(start, direction)));

                // Create an extruded circle along the line segment
                var extrusion = new Extrude(circle, length, direction, false);

                rep.SolidOperations.Add(extrusion);
            }

            // Add a spherical point at each vertex of the polyline
            if (ShowPoints)
            {
                foreach (var vertex in Polyline.Vertices)
                {
                    var sphere = Mesh.Sphere(pointRadius, 10);

                    HashSet<Geometry.Vertex> modifiedVertices = new HashSet<Geometry.Vertex>();
                    // Translate the vertices of the mesh to center it at the origin
                    foreach (var svertex in sphere.Vertices)
                    {
                        if (!modifiedVertices.Contains(svertex))
                        {
                            svertex.Position += vertex;
                            modifiedVertices.Add(svertex);
                        }
                    }

                    foreach (var triangle in sphere.Triangles)
                    {
                        // Create a Polygon from the triangle's vertices point
                        var polygon = new Polygon(triangle.Vertices.SelectMany(v => new List<Vector3> { v.Position }).ToList());
                        solidRep.AddFace(polygon);
                    }
                }
            }

            var consol = new ConstructedSolid(solidRep);
            rep.SolidOperations.Add(consol);

            this.Representation = rep;
        }
    }
}