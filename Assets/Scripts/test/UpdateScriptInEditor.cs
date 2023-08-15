using UnityEngine;
using UnityEditor;

/// <summary>
/// This simple editor script regenerates plane when change is made.
/// </summary>
[CustomEditor(typeof(VertexTest))]
public class UpdateScriptInEditor : Editor
{
    /// <summary>
    /// Unity method called to draw inspector.
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Reference to the plane generator script.
        var generator = target as VertexTest;

        // Regenerating plane only if we are in Play mode.
        if (Application.isPlaying)
        {
            generator.UpdateInEditor();
        }
    }
}
