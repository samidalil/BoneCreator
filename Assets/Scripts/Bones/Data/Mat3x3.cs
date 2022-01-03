using UnityEngine;

namespace Bones.Data
{
    /// <summary>
    /// Represents a 3x3 row-major matrix
    /// </summary>
    public sealed class Mat3x3
    {
        // COMMENT : Nous avons implémenté seulement les opérations dont nous avions besoin
        // à l'exception de l'opérateur crochet pour déboguer plus rapidement

        #region Fields

        private float[] _data;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a 3x3 matrix filled the given values
        /// </summary>
        public Mat3x3(float x1, float x2, float x3, float y1, float y2, float y3, float z1, float z2, float z3)
        {
            this._data = new float[] { x1, x2, x3, y1, y2, y3, z1, z2, z3 };
        }

        /// <summary>
        /// Initializes a 3x3 matrix filled with zeros
        /// </summary>
        public Mat3x3() : this(0, 0, 0, 0, 0, 0, 0, 0, 0) { }

        #endregion

        #region Operators

        /// <summary>
        /// Gets or sets the element at the given index
        /// </summary>
        /// <param name="i">Matrix index</param>
        /// <returns></returns>
        public float this[int i]
        {
            get => this._data[i];
            set => this._data[i] = value;
        }

        /// <summary>
        /// Gets or sets the element at the given row and column
        /// </summary>
        /// <param name="x">Row index</param>
        /// <param name="y">Column index</param>
        /// <returns></returns>
        public float this[int x, int y]
        {
            get => this._data[x * 3 + y];
            set => this._data[x * 3 + y] = value;
        }

        /// <summary>
        /// Returns the product of a 3x3 matrix and a vector
        /// </summary>
        /// <param name="matrix">A 3x3 matrix</param>
        /// <param name="vector">A Unity Vector</param>
        /// <returns>A Vector3 containing the result</returns>
        public static Vector3 operator *(Mat3x3 matrix, Vector3 vector) =>
            new Vector3(
                // a * x + b * y + c * z
                matrix._data[0] * vector.x + matrix._data[1] * vector.y + matrix._data[2] * vector.z,
                // d * x + e * y + f * z
                matrix._data[3] * vector.x + matrix._data[4] * vector.y + matrix._data[5] * vector.z,
                // g * x + h * y + i * z
                matrix._data[6] * vector.x + matrix._data[7] * vector.y + matrix._data[8] * vector.z
            );

        #endregion
    }
}
