using UnityEngine;

namespace Bones.Core
{
    /// <summary>
    /// Represents a skeleton with multiple bones
    /// </summary>
    public sealed class Skeleton : MonoBehaviour
    {
        #region Fields

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the skeleton and its bones
        /// </summary>
        public void Initialize(Vector3[] leftArmVertices)
        {
            Bone leftArm = this.CreateBone(leftArmVertices);

            // TD : Raccordement à faire entre les bones
            // Deux possibilités :
            // - tout doit être générique dès l'import du mesh, la partie convexe devra être taggée avec la partie du corps à laquelle elle correspond et le raccordement devra évaluer les possibilités, <-
            // - l'utilisateur indique qu'une certaine partie convexe correspond à une certaine partie du corps et le raccordement sera directement hardcodé
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a bone as a child and initializes it
        /// </summary>
        /// <param name="vertices">A point cloud</param>
        /// <returns>The newly created and initialized bone</returns>
        private Bone CreateBone(Vector3[] vertices, string name = "Bone")
        {
            Bone bone = new GameObject(name).AddComponent<Bone>();

            bone.transform.SetParent(this.transform);
            bone.Initialize(vertices);

            return bone;
        }

        #endregion
    }
}
