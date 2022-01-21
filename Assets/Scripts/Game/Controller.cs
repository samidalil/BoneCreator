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
        [Tooltip("Force to apply")]
        private float _force;

        #endregion

        #region Fields

        private bool _launch = false;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Fired on every physics tick
        /// </summary>
        private void FixedUpdate()
        {
            if (this._launch)
            {
                Debug.Log("In");

                this._launch = false;

                this._body.AddForce((Vector3.up + Vector3.forward) * this._force);
            }
        }

        /// <summary>
        /// Fired on every render tick
        /// </summary>
        private void Update()
        {
            if (!this._launch)
                this._launch = Input.GetKeyDown(KeyCode.Space);
        }

        #endregion
    }
}
