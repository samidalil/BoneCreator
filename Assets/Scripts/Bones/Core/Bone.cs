using Bones.Data;
using Bones.Math;
using System.Linq;
using UnityEngine;

namespace Bones.Core
{
    /// <summary>
    /// Represents a bone
    /// </summary>
    public sealed class Bone : MonoBehaviour
    {
        // TD : Le tableau de points sera retiré par la suite, voir comment diviser un mesh en plusieurs parties convexes

        #region Properties

        /// <summary>
        /// Gets the first point of the computed primary component
        /// </summary>
        public Vector3 NegativeExtremum => this._primaryComponent.Item1;

        /// <summary>
        /// Gets the last point of the computed primary component
        /// </summary>
        public Vector3 PositiveExtremum => this._primaryComponent.Item2;

        /// <summary>
        /// Gets the computed primary component alias a segment from the negative extremum to the positive extremum of an approximated proper vector
        /// </summary>
        public (Vector3, Vector3) PrimaryComponent => this._primaryComponent;

        #endregion

        #region Fields

        private Vector3 _debugBarycenter;
        private Vector3[] _debugCenteredPoints;
        private Vector3[] _debugPoints;
        private Vector3 _debugProperVector;

        private (Vector3, Vector3) _primaryComponent;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Fired on draw gizmos tick
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawSphere(this._debugBarycenter, 0.01f);

            if (this._debugPoints != null)
            {
                Gizmos.color = Color.blue;

                foreach (Vector3 point in this._debugPoints)
                    Gizmos.DrawSphere(point, 0.001f);
            }

            Gizmos.color = Color.green;

            Gizmos.DrawLine(this._debugBarycenter, this._debugBarycenter + this._debugProperVector);

            Gizmos.color = Color.red;

            Gizmos.DrawLine(this._primaryComponent.Item1, this._primaryComponent.Item2);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the bone
        /// </summary>
        public void Initialize(Vector3[] points)
        {
            this._debugPoints = points;

            // Compute barycenter -> Center -> Compute covariance matrix

            Vector3 barycenter = this._debugBarycenter = Geometry.ComputeBarycenter(points);
            Vector3[] centeredPoints = this._debugCenteredPoints = points.Select(p => p - barycenter).ToArray();
            Mat3x3 covarianceMatrix = Statistics.ComputeCovarianceMatrix(centeredPoints, barycenter);

            // Compute approximation of the matrix's proper vector -> Retrieve repositionned extremums of the projected points

            Vector3 properVector = this._debugProperVector = Statistics.ComputeProperVectorApproximation(covarianceMatrix, 50);
            this._primaryComponent = Geometry.RetrieveProjectedExtremums(centeredPoints, properVector, barycenter);
        }

        #endregion
    }
}
