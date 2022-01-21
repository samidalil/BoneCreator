using Bones.Data;
using System.Linq;
using UnityEngine;

namespace Bones.Math
{
    public static class Geometry
    {
        /// <summary>
        /// Computes the barycenter of the point cloud
        /// </summary>
        /// <param name="points">The point cloud</param>
        /// <returns>The weighted sum of the points' positions</returns>
        public static Vector3 ComputeBarycenter(Vector3[] points) => points.Aggregate((a, b) => a + b) / points.Length;

        /// <summary>
        /// Generates an approximation of the primary component of the point cloud
        /// </summary>
        /// <param name="points">A point cloud</param>
        /// <param name="iterations">The more iterations, the more accurate the approximation is</param>
        /// <returns>An approximation of the point cloud's primary component</returns>
        public static Segment GenerateApproximatedPrimaryComponent(Vector3[] points, int iterations = 50)
        {
            // Compute barycenter -> Center points -> Compute covariance matrix

            Vector3 barycenter = Geometry.ComputeBarycenter(points);
            Vector3[] centeredPoints = points.Select(p => p - barycenter).ToArray();
            Mat3x3 covarianceMatrix = Statistics.ComputeCovarianceMatrix(points, barycenter);

            // Compute approximation of the matrix's eigen vector -> Retrieve repositionned extremums of the projected points

            Vector3 eigenVector = Statistics.ComputeEigenVectorApproximation(covarianceMatrix, iterations);
            return Geometry.RetrieveProjectedExtremums(centeredPoints, eigenVector, barycenter);
        }

        /// <summary>
        /// Searches for the extremums of the projection of a point cloud on a given vector
        /// </summary>
        /// <param name="points">Points to project, at least size 2</param>
        /// <param name="projectionVector">The projection vector should be normalized</param>
        /// <param name="barycenter">The barycenter of the point cloud</param>
        /// <returns>The negative extremum in first position and the positive extremum repositionned according to the barycenter in a tuple</returns>
        public static Segment RetrieveProjectedExtremums(Vector3[] points, Vector3 projectionVector, Vector3 barycenter)
        {
            float maxPositiveSqrMagnitude = .0f;
            float maxNegativeSqrMagnitude = .0f;

            Vector3 negativeExtremum = Vector3.zero;
            Vector3 positiveExtremum = Vector3.zero;

            foreach (Vector3 point in points)
            {
                Vector3 projectedPoint = Vector3.Dot(point, projectionVector) * projectionVector;
                float alpha = Vector3.Dot(projectedPoint, projectionVector);

                // COMMENT : les vecteurs vont dans la même direction
                if (alpha > 0)
                {
                    if (projectedPoint.sqrMagnitude > maxPositiveSqrMagnitude)
                    {
                        maxPositiveSqrMagnitude = projectedPoint.sqrMagnitude;
                        positiveExtremum = projectedPoint;
                    }
                }
                // COMMENT : les vecteurs vont dans des directions opposées
                else if (alpha < 0)
                {
                    if (projectedPoint.sqrMagnitude > maxNegativeSqrMagnitude)
                    {
                        maxNegativeSqrMagnitude = projectedPoint.sqrMagnitude;
                        negativeExtremum = projectedPoint;
                    }
                }
            }

            return new Segment(negativeExtremum + barycenter, positiveExtremum + barycenter);
        }
    }
}
