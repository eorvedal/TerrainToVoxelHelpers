using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fraktalia.VoxelGen.Modify.Procedural;
using System.Threading.Tasks;
using Fraktalia.VoxelGen;
using System;
using UnityEditor;

public class MultiTerrainToVoxel : MonoBehaviour
{
    public GameObject VoxelTemplate;
    public int size = 512;
    public Terrain[] trains;
    public Vector3 offset = Vector3.zero;

    [ContextMenu("Get all terrains")]
    public void GetAllTerrains()
    {
        trains = FindObjectsByType<Terrain>(FindObjectsSortMode.None);
    }

    [ContextMenu("Create all voxels")]
    public void CreateAllVoxelsFromTerrains()
    {
        foreach (Terrain T in trains)
        {
            StartCoroutine(HeavyLifting(T));
        }
    }

    [ContextMenu("Create Multiblock from All Voxels")]
    public void CreateMultiblockFromAllVoxels()
    {
        GameObject Multiblock = new GameObject("MultiBlock");
        VoxelGenerator[] voxelGenerators = FindObjectsByType<VoxelGenerator>(FindObjectsSortMode.None);
        Multiblock.transform.position = voxelGenerators[0].gameObject.transform.position;
        VoxelGeneratorMultiblock mb = Multiblock.AddComponent<VoxelGeneratorMultiblock>();
        voxelGenerators[0].gameObject.transform.SetParent(Multiblock.transform);
        mb.Reference = voxelGenerators[0];
        for (int i = 1; i < voxelGenerators.Length; i++)
        {
            voxelGenerators[i].gameObject.transform.SetParent(Multiblock.transform);
        }
        mb.ConnectGenerators();
    }

    [ContextMenu("Parent Generators to Terrains")]
    public void ParentGeneratorsToTerrains()
    {
        foreach(Terrain t in trains)
        {
            TerrainToVoxel surfaceModifier = t.GetComponentInChildren<TerrainToVoxel>();
            GameObject newParent = surfaceModifier.TargetGenerator.gameObject;
            t.transform.SetParent(newParent.transform);
        }
    }

    [ContextMenu("Run All Modifiers")]
    public void RunAllModifiers()
    {
        StartCoroutine(RunAllModifiersCoroutine());
    }
    private IEnumerator RunAllModifiersCoroutine()
    {

        int totalTasks = trains.Length;
        int currentTask = 0;

        foreach (Terrain t in trains)
        {
            currentTask++;
            EditorUtility.DisplayProgressBar("Running All Modifiers", $"Processing {t.name} ({currentTask}/{totalTasks})...", (float)currentTask / totalTasks);

            yield return StartCoroutine(ProcessModifiersForVoxelGenerator(t));
        }

        EditorUtility.ClearProgressBar();
        Debug.Log("Run All Modifiers Complete.");
    }

    private IEnumerator ProcessModifiersForVoxelGenerator(Terrain ter)
    {
        TerrainToVoxel[] surfaceModifiers = ter.gameObject.GetComponentsInChildren<TerrainToVoxel>();

        foreach (TerrainToVoxel surfaceModifier in surfaceModifiers)
        {
            Debug.Log($"Applying modifier for {ter.name}...");
            surfaceModifier.ApplyProceduralModifier();

            yield return null;
        }

        Debug.Log($"Finished processing {ter.name}.");
    }

    [ContextMenu("Save All Voxel Generators")]
    public void SaveAllVoxelGenerators()
    {
        VoxelGenerator[] voxelGenerators = FindObjectsByType<VoxelGenerator>(FindObjectsSortMode.None);
        foreach(VoxelGenerator vg in voxelGenerators)
        {
            vg.savesystem.Save();
        }
    }

    public IEnumerator HeavyLifting(Terrain T)
    {
        yield return new WaitForSeconds(1);
        try
        {
            GameObject newTer = Instantiate(VoxelTemplate, T.transform.position + offset, T.transform.rotation);
            VoxelGenerator vg = newTer.GetComponent<VoxelGenerator>();
            vg.RootSize = size;
            TerrainToVoxel[] SurfaceModifiers = newTer.GetComponentsInChildren<TerrainToVoxel>();
            foreach (TerrainToVoxel SurfaceModifier in SurfaceModifiers)
            {
                SurfaceModifier.transform.SetParent(T.transform);
                SurfaceModifier.transform.localPosition = Vector3.zero;
                SurfaceModifier.transform.localRotation = Quaternion.identity;
                SurfaceModifier.TargetGenerator = vg;
                SurfaceModifier.TerrainToConvert = T;
            }
        }
        catch (Exception e)
        {
            Debug.Log($"error : {e.Message}");
        }
    }

}

