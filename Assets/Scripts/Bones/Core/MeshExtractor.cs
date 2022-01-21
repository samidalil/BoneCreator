using UnityEngine;

namespace Bones.Core
{
    /// <summary>
    /// Extracts automatically the body parts of the given mesh
    /// </summary>
    public sealed class MeshExtractor : MonoBehaviour
    {
        #region Unity Fields

        [SerializeField]
        [Tooltip("Humanoid mesh")]
        private Mesh _mesh;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Fired on script awake
        /// </summary>
        private void Awake()
        {
            this.Initialize();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the skeleton with the mesh parts
        /// </summary>
        private void Initialize()
        {
            Skeleton skeleton = new GameObject("Skeleton").AddComponent<Skeleton>();

            skeleton.Initialize(
                this._mesh,
                this._mesh,
                this._mesh,
                this._mesh,
                this._mesh,
                this._mesh,
                this._mesh,
                this._mesh,
                this._mesh,
                this._mesh,
                this._mesh,
                this._mesh,
                this._mesh,
                this._mesh,
                0.1f
            );
        }

        #endregion
    }
}
