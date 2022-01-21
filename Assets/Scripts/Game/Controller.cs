using UnityEngine;

namespace Bones.Game
{
    public sealed class Controller : MonoBehaviour
    {
        #region Unity Fields

        [SerializeField]
        [Tooltip("Rigidbody to apply force on")]
        private Rigidbody _body = null;

        [SerializeField]
        [Tooltip("Base direction for launch")]
        private Vector3 _direction = Vector3.up + Vector3.forward;

        [SerializeField]
        [Tooltip("Force to apply")]
        private float _force;

        [SerializeField]
        [Tooltip("Launch axis rotation speed")]
        private float _rotationSpeed;

        #endregion

        #region Fields

        private bool _launch = false;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Fired on draw gizmos tick
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(this._body.transform.position, this._body.transform.position + this._direction.normalized);
        }

        /// <summary>
        /// Fired on every physics tick
        /// </summary>
        private void FixedUpdate()
        {
            if (this._launch)
            {
                this._launch = false;
                this._body.AddForce(this._direction.normalized * this._force);
            }
        }

        /// <summary>
        /// Fired on every render tick
        /// </summary>
        private void Update()
        {
            if (!this._launch)
                this._launch = Input.GetKeyDown(KeyCode.Space);

            float rotation = Input.GetAxis("Horizontal");

            this._direction = Quaternion.AngleAxis(this._rotationSpeed * rotation, Vector3.up) * this._direction;
        }

        #endregion
    }
}
