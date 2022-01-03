using System.Linq;
using UnityEngine;

namespace Bones.Core
{
    /// <summary>
    /// Represents a skeleton with multiple bones
    /// </summary>
    public sealed class Skeleton : MonoBehaviour
    {
        #region Unity Fields

        [SerializeField]
        [Tooltip("Bones of the skeleton")]
        private Bone[] _bones;

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
        /// Initializes the skeleton and its bones
        /// </summary>
        private void Initialize()
        {
            foreach (Bone bone in this._bones) bone.Initialize();

            // TD : Raccordement à faire entre les bones
            // Deux possibilités :
            // - tout doit être générique dès l'import du mesh, la partie convexe devra être taggée avec la partie du corps à laquelle elle correspond et le raccordement devra évaluer les possibilités,
            // - l'utilisateur indique qu'une certaine partie convexe correspond à une certaine partie du corps et le raccordement sera directement hardcodé

        }

        #endregion
    }
}
