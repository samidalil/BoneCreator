using UnityEditor;
using UnityEngine;

namespace Bones.Core
{
    [CustomEditor(typeof(Skeleton))]
    public sealed class SkeletonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Skeleton skeleton = (Skeleton)target;

            if (skeleton.HasSkinnedMeshRenderer && GUILayout.Button("Assign to Skinned Mesh Renderer"))
                skeleton.AssignBones();
        }
    }
}
