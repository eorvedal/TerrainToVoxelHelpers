using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MultiTerrainToVoxel))]
public class MultiTerrainToVoxelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw default inspector UI
        DrawDefaultInspector();

        // Get reference to the script
        MultiTerrainToVoxel script = (MultiTerrainToVoxel)target;

        // Add a button for "Get all terrains"
        if (GUILayout.Button("Get All Terrains"))
        {
            script.GetAllTerrains();
        }

        // Add a button for "Create all voxels"
        if (GUILayout.Button("Create All Voxels From Terrains"))
        {
            script.CreateAllVoxelsFromTerrains();
        }

        if (GUILayout.Button("Create MultiBlock From All Voxels"))
        {
            script.CreateMultiblockFromAllVoxels();
        }

        if (GUILayout.Button("Parent Terrains to Generators"))
        {
            script.ParentGeneratorsToTerrains();
        }

        if (GUILayout.Button("Run All Modifiers"))
        {
            script.RunAllModifiers();
        }

        if (GUILayout.Button("Save All Voxel Generators"))
        {
            script.SaveAllVoxelGenerators();
        }

    }
}
