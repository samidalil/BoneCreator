using UnityEngine;

namespace Bones.Math
{
    public static class Helpers
    {
        /// <summary>
        /// Returns the component with the highest absolute value
        /// </summary>
        /// <param name="vector">The vector to evaluate</param>
        /// <returns>A float value</returns>
        public static float GetGreatestAbsoluteComponent(Vector3 vector)
        {
            float xAbs = Mathf.Abs(vector.x);
            float yAbs = Mathf.Abs(vector.y);
            float zAbs = Mathf.Abs(vector.z);

            if (xAbs < yAbs)
            {
                if (yAbs < zAbs) return vector.z;

                return vector.y;
            }
            if (xAbs < zAbs) return vector.z;

            return vector.x;
        }
    }
}
