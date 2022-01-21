using System;
using UnityEngine;

namespace Bones.Data
{
    /// <summary>
    /// Represents a segment between two points
    /// </summary>
    [Serializable]
    public sealed class Segment
    {
        #region Properties

        /// <summary>
        /// Gets the center of the segment
        /// </summary>
        public Vector3 Center => this.Start + (this.End - this.Start) / 2;

        /// <summary>
        /// Gets the length of the segment
        /// </summary>
        public float Length => Vector3.Distance(this.Start, this.End);

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets the start point of the segment
        /// </summary>
        public Vector3 End;

        /// <summary>
        /// Gets or sets the end point of the segment
        /// </summary>
        public Vector3 Start;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="start">Start point</param>
        /// <param name="end">End point</param>
        public Segment(Vector3 start, Vector3 end)
        {
            this.End = end;
            this.Start = start;
        }

        #endregion

        #region Public Operators

        /// <summary>
        /// Gets a point corresponding to the index
        /// </summary>
        /// <param name="i">Index of the point</param>
        /// <returns>A point of the segment</returns>
        public Vector3 this[int i]
        {
            get => i == 0 ? this.Start : this.End;
        }

        #endregion
    }
}
