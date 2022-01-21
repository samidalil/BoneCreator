using Bones.Core;
using UnityEditor;
using UnityEngine;

namespace Bones.Edition
{
    [CustomEditor(typeof(Skeleton))]
    public sealed class SkeletonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Assign bones"))
                (target as Skeleton).AssignBones();
        }
    }
}
