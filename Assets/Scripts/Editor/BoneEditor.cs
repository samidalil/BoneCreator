using Bones.Core;
using UnityEditor;
using UnityEngine;

namespace Bones.Inspector
{
    /// <summary>
    /// Editor layout for Bone object
    /// </summary>
    [CustomEditor(typeof(Bone))]
    public sealed class BoneEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Fill with childrens")) (this.target as Bone).FillWithChildrens();
            DrawDefaultInspector();
        }
    }
}
