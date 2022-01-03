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

        #region Unity Fields

        [SerializeField]
        [Tooltip("Point cloud used to generate the bone")]
        private Point[] _points = null;

        #endregion

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

        private float _lastUpdate;
        private (Vector3, Vector3) _primaryComponent;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Fired on script awake
        /// </summary>
        private void Awake()
        {
            // Automatic initialization
            if (this._points == null || this._points.Length == 0) this.FillWithChildrens();

            foreach (Point point in this._points) point.OnPositionChanged += this.OnPointPositionChanged;

            this._lastUpdate = Time.time;
        }

        /// <summary>
        /// Fired on script destruction
        /// </summary>
        private void OnDestroy()
        {
            foreach (Point point in this._points) point.OnPositionChanged -= this.OnPointPositionChanged;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the point cloud with the childrens of the game object the script is attached to
        /// </summary>
        public void FillWithChildrens()
        {
            this._points = this.transform.GetComponentsInChildren<Point>();
        }

        /// <summary>
        /// Initializes the bone
        /// </summary>
        public void Initialize()
        {
            // Compute barycenter -> Center -> Compute covariance matrix

            Vector3 barycenter = Geometry.ComputeBarycenter(this._points);
            Vector3[] points = this._points.Select(p => p.Position - barycenter).ToArray();
            Mat3x3 covarianceMatrix = Statistics.ComputeCovarianceMatrix(points, barycenter);

            // Compute approximation of the matrix's proper vector -> Retrieve repositionned extremums of the projected points

            Vector3 properVector = Statistics.ComputeProperVectorApproximation(covarianceMatrix, 50);
            this._primaryComponent = Geometry.RetrieveProjectedExtremums(points, properVector, barycenter);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Fired on a point's position changed
        /// </summary>
        /// <param name="point">Moved point</param>
        private void OnPointPositionChanged(Point point)
        {
            if (Time.time != this._lastUpdate)
            {
                this._lastUpdate = Time.time;
                // this.Initialize();
            }
        }

        #endregion
    }
}
