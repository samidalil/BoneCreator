using UnityEditor;
using UnityEngine;

namespace Bones.Core
{
    [CustomEditor(typeof(BodyHandler))]
    public class BodyHandlerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate skeleton"))
                (target as BodyHandler).GenerateSkeleton();
        }
    }
}
