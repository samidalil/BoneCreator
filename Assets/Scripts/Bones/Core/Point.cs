using System;
using UnityEngine;

namespace Bones.Core
{
    /// <summary>
    /// Represents a point in space
    /// </summary>
    public sealed class Point : MonoBehaviour
    {
        #region Static Fields

        private static float _updateDelay = 0.5f;

        #endregion

        #region Events

        /// <summary>
        /// Invokes when the position of the point changes
        /// </summary>
        public event Action<Point> OnPositionChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the position of the point
        /// </summary>
        public Vector3 Position => this.transform.position;

        #endregion

        #region Fields

        private float _nextUpdate;
        private Vector3 _oldPosition;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Fired on script awake
        /// </summary>
        private void Awake()
        {
            this._nextUpdate = Time.time + Point._updateDelay;
            this._oldPosition = transform.position;
        }

        /// <summary>
        /// Fired every update tick
        /// </summary>
        private void Update()
        {
            if (this._oldPosition != this.transform.position)
            {
                if (Time.time > this._nextUpdate)
                {
                    this.OnPositionChanged?.Invoke(this);
                    this._oldPosition = this.transform.position;
                }

                this._nextUpdate = Time.time + Point._updateDelay;
            }
        }

        #endregion
    }
}
