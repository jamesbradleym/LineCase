using Elements.Geometry;
using Elements.Geometry.Solids;
using LineCase;
using Newtonsoft.Json;

public static class Helper
{
    public static double GetAngle(Vector3 vectorA, Vector3 vectorB, double tolerance = 0.0001f)
    {
        // Calculate the dot product of the vectors
        double dotProduct = vectorA.Dot(vectorB);

        // Calculate the magnitudes of the vectors
        double magnitudeA = vectorA.Length();
        double magnitudeB = vectorB.Length();

        // Check if the vectors are nearly parallel (dot product close to 1 or -1)
        if (Math.Abs(dotProduct / (magnitudeA * magnitudeB)) > 1.0 - tolerance)
        {
            // Vectors are nearly parallel, return 0 or 180 degrees based on dot product
            return Math.Acos(Math.Sign(dotProduct)) * 180.0 / Math.PI;
        }

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
}