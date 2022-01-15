using UnityEngine;

namespace Bones.Core
{
    /// <summary>
    /// Extracts the body parts of the given mesh
    /// </summary>
    public sealed class MeshExtractor : MonoBehaviour
    {
        #region Unity Fields

        [SerializeField]
        [Tooltip("Humanoid mesh")]
        private Mesh _mesh;

        [SerializeField]
        [Tooltip("Skeleton to initialize")]
        private Skeleton _skeleton;

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

            skeleton.Initialize(this._mesh.vertices);
        }

        #endregion
    }
}
