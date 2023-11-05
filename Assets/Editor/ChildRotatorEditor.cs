using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChildRotator))]
public class ChildRotatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ChildRotator childRotator = (ChildRotator)target;

        if (GUILayout.Button("Rotate and Check"))
        {
            childRotator.RotateAllChildren();
        }
    }
}