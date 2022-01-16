using Bones.Data;
using System.Linq;
using UnityEngine;

namespace Bones.Math
{
    public static class Statistics
    {
        /// <summary>
        /// Returns an approximation of the eigen vector of a covariance matrix
        /// </summary>
        /// <param name="covarianceMatrix">A 3x3 covariance matrix</param>
        /// <param name="iterations">The more iterations, the more accurate the approximation is</param>
        /// <returns>An approximation of the eigen vector normalized</returns>
        public static Vector3 ComputeEigenVectorApproximation(Mat3x3 covarianceMatrix, int iterations) =>
            Enumerable
                .Range(0, iterations)
                .Aggregate(covarianceMatrix * new Vector3(0, 0, 1), (vector, _) => covarianceMatrix * vector / Helpers.GetGreatestAbsoluteComponent(vector))
                .normalized;

        /// <summary>
        /// Returns the covariance 3x3 matrix of a set of points
        /// </summary>
        /// <param name="points">Set of points</param>
        /// <param name="barycenter">Barycenter of the set of points</param>
        /// <returns>The covariance 3x3 matrix</returns>
        public static Mat3x3 ComputeCovarianceMatrix(Vector3[] points, Vector3 barycenter)
        {
            float[] X = points.Select((a) => a.x).ToArray();
            float[] Y = points.Select((a) => a.y).ToArray();
            float[] Z = points.Select((a) => a.z).ToArray();
            float covXY = Cov(X, Y, barycenter.x, barycenter.y);
            float covXZ = Cov(X, Z, barycenter.x, barycenter.z);
            float covYZ = Cov(Y, Z, barycenter.y, barycenter.z);

            return new Mat3x3(
                Var(X, barycenter.x), covXY, covXZ,
                covXY, Var(Y, barycenter.y), covYZ,
                covXZ, covYZ, Var(Z, barycenter.z)
            );
        }

        /// <summary>
        /// Returns the covariance of two sets of values
        /// </summary>
        /// <param name="setA">First set of values</param>
        /// <param name="setB">Second set of values, at least the same size as the first set</param>
        /// <param name="meanA">First set's mean</param>
        /// <param name="meanB">Second set's mean</param>
        /// <returns>The covariance of the two sets</returns>
        public static float Cov(float[] setA, float[] setB, float meanA, float meanB) =>
            setA.Zip(setB, (a, b) => a * b).Sum() / setA.Length - (meanA * meanB);

        /// <summary>
        /// Returns the variance of a set of values
        /// </summary>
        /// <param name="set">Set of values</param>
        /// <param name="mean">Set's mean</param>
        /// <returns>The variance of the set</returns>
        public static float Var(float[] set, float mean) =>
            set.Select(v => v * v).Sum() / set.Length - (mean * mean);
    }
}
