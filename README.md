# TerrainToVoxelHelpers
 Helpers for Voxelicas TerrainToVoxel modifier

 The first tool is the version 2 of the TextureArrayGenerator, which is actually just changes to the template and the GUI. Now instead of creating a material for each layer of your terrain textures, you can select the terrain in the hierarchy and then choose "Tools>Fraktalia>Create Texture Array From Selected Terrain".
 
![Screenshot 2024-12-13 114844](https://github.com/user-attachments/assets/d37115a1-118e-4c7b-bc61-d057736848c5)

This will load an alternate Texture Array Generator Window that has two new fields: "Extract From Terrain" and "Terrain To Extract From". The former is the bool to extract the materials from the terrain, which will become obsolete but will be used for now. This should be checked true. The "Terrain To Extract From" field should be automatically filled with the TerrainData that belongs to the Terrain you had highlighted. At this step don't forget to make your material. The included prefab is set up for the 8 channel UV Blend shaders. This has been tested on both the Standard and URP/HDRP projects.

![Screenshot 2024-12-13 115116](https://github.com/user-attachments/assets/ab383814-8f32-4db2-a1c7-a24e6f2216b7)

Make sure all of your options, like pathnames, are set and the new material is in the Target Material field.

![Screenshot 2024-12-13 115329](https://github.com/user-attachments/assets/ef1d58ff-d001-4dc9-bea2-990fb06fb291)

Once you are ready press "Extract From Terrain"

![Screenshot 2024-12-13 115421](https://github.com/user-attachments/assets/23fc28ce-191c-43ce-bd27-59866a7ca1e9)

This will fill in all the fields to create the array for the textures on your terrain.

![Screenshot 2024-12-13 115522](https://github.com/user-attachments/assets/8b891f71-fb14-4858-9eff-c6e7a269b99f)

Press "Create Texture Array".

![Screenshot 2024-12-13 115621](https://github.com/user-attachments/assets/3cbd3b49-fb32-4e27-9a41-9bccd81b784d)

Finish setting up your material and navigate to the prefabs folder and open the prefab "VoxelGenerator_Modular_MultiblockTerrainSetup" and add your material to the Hull Generator.

![image](https://github.com/user-attachments/assets/5b8ad529-bd19-4a84-b6ac-ba7a8ba231c5)

[SUPER IMPORTANT ADDENDUM]
IMPORTANT! For some reason the Post Process Modules on the Hull Generator change inside the prefab in this version of the Helpers. My apologies. Please make sure your Post Process Modules are 1) Cube UV and 2) Multi Texture_V2. The UV power of the cube UV is up to you, but make sure the rest of your settings look exactly like this:

![image](https://github.com/user-attachments/assets/07c83ae4-d2b4-4259-ac1e-f94754553286)

Close the prefab, we will now make sure our offsets are correct. Drag the same prefab in the scene and set it to the same world position as your terrain or first terrain. Change the Volume size of the generator until it fits your terrain around the edges. Lower the Y value of position until you have the terrain in a position inside the block that you are satisfied with. Select all of the TerrainToVoxel modifiers and adjust the Top Extension and the Bottom Extension to make sure they completely overlap your terrain height-wise. Make note of: the prefab's adjusted Y position, the generator's volume size, and the top and bottom extension of the modifiers. Then delete the prefab from the scene WITHOUT saving the changes.

![Screenshot 2024-12-13 122113](https://github.com/user-attachments/assets/056fffee-3040-4aa9-a19c-4faf6f131069)

![Screenshot 2024-12-13 122356](https://github.com/user-attachments/assets/884cb518-3ab7-4a52-9b76-9d208de80d39)

![Screenshot 2024-12-13 122503](https://github.com/user-attachments/assets/51ec0efc-e079-4498-835f-34d22c780603)

Reopen the prefab in prefab mode and set the top and bottom extension numbers on the modifiers. This will save you the time of doing it multiple times if you have more than one terrain. Close the prefab again and return to the scene. Find the [CONVERTER] prefab and drag it into your scene.

![Screenshot 2024-12-13 122537](https://github.com/user-attachments/assets/20d0f61f-6c37-464a-86fd-32ab1297636b)

In the offset field of the [CONVERTER] set the the Y value of the prefab when it was in position with your terrain. Also change the size to the volume size we noted at that time. [IMPORTANT] Let the script position the generators using the offset instead of changing the position of the actual prefab in or out of prefab mode!
Press "Get All Terrains" on the [CONVERTER] and confim that all the terrains you want converted are in the list. If they are, press "Create All Voxels From Terrains" If you are using a single terrain, you can skip right to "Run All Modifiers" and "Save All Voxel Generators". 
However, if you have multiple terrains press "Create Multiblock From All Voxels". In some cases you will need your terrain as part of the voxel block, in this case you can press "Parent Terrains to Generators" and disable the Terrain component until you need it.
You should now be ready to press "Run All Modifiers" and "Save All Voxel Generators" when that is finished.

![Screenshot 2024-12-13 133533](https://github.com/user-attachments/assets/54df04cb-5f19-4873-ae89-d5bf5beeed55)


![Screenshot 2024-12-13 133616](https://github.com/user-attachments/assets/dba02202-b79d-4f58-bf2c-f784a7224f90)

MISSION COMPLETE!
