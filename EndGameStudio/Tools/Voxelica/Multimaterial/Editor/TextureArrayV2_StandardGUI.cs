#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using Fraktalia.Core.FraktaliaAttributes;
using System.Runtime.Serialization;


namespace Fraktalia.Core.FraktaliaAttributes
{
	public class TextureArrayGeneratorV2 : EditorWindow
	{

		public static TextureArrayTemplateV2 texturegenerator;
		public Vector2 scrollpos;

		[MenuItem("Tools/Fraktalia/Create Texture Array From Selected Terrain")]
		public static void Init()
		{

			TextureArrayGeneratorV2 window = (TextureArrayGeneratorV2)EditorWindow.GetWindow(typeof(TextureArrayGeneratorV2));
			window.name = "Texture2D Array Generator";
			if (texturegenerator == null) texturegenerator = CreateGenerator();
			texturegenerator.TerrainToExtractFrom = Selection.activeGameObject.GetComponent<Terrain>().terrainData;
			texturegenerator.ExtractFromTerrain = true;
			window.Show();
		}


		public static TextureArrayTemplateV2 CreateGenerator()
		{
			TextureArrayTemplateV2 output = (TextureArrayTemplateV2)Resources.Load("TerrainDirectExtract4Layer");

			if (output == null) output = ScriptableObject.CreateInstance<TextureArrayTemplateV2>();

			output.WhiteDefaultTexture = (Texture2D)Resources.Load("NULL_white");
			output.BlackDefaultTexture = (Texture2D)Resources.Load("NULL_black");
			output.NormalDefaultTexture = (Texture2D)Resources.Load("NULL_normal");
			output.HeightDefaultTexture = (Texture2D)Resources.Load("NULL_height");

			return output;
		}



		void OnGUI()
		{
			GUIStyle title = new GUIStyle();
			title.fontStyle = FontStyle.Bold;
			title.fontSize = 14;
			title.richText = true;

			GUIStyle bold = new GUIStyle();
			bold.fontStyle = FontStyle.Bold;
			bold.fontSize = 12;
			bold.richText = true;

			Texture2D colortex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
			colortex.SetPixel(0, 0, new Color32(240, 240, 240, 255));
			colortex.Apply();

			GUIStyle text = new GUIStyle();

			text.fontSize = 12;
			text.richText = true;
			text.wordWrap = true;
			text.margin = new RectOffset(5, 5, 5, 5);
			text.normal.background = colortex;
			text.padding = new RectOffset(5, 5, 5, 5);

			EditorStyles.textField.wordWrap = true;

			bool hasErrors = false;

			if (texturegenerator == null) texturegenerator = CreateGenerator();

			Editor texturearrayeditor = Editor.CreateEditor(texturegenerator);
			scrollpos = GUILayout.BeginScrollView(scrollpos, false, true, GUILayout.Width(this.position.width), GUILayout.ExpandHeight(true));

			EditorGUILayout.LabelField("This generator combines multiple Texture2D into Texture2DArrays. It is designed to merge materials using the standard shader. " +
				"Therefore you can assign textures manually or extract texture infomation" +
				" from materials. Assign as many materials as you want and hit the extract material button in order to automatically fill the extraction arrays." +
				" The settings like smoothness, metallic and emission parameters are also extracted so the slice of the resulting texture array matches the assigned standard materials." +
				" After extraction, select output path and optionally assign a target material which uses a MultiTextureArray shader. " +
				"The resulting texture arrays will automatically assigned to the target material. Works best with substance materials.", text);

			texturegenerator = (TextureArrayTemplateV2)EditorGUILayout.ObjectField("Texture Array Template", texturegenerator, typeof(TextureArrayTemplateV2), false);





			texturearrayeditor.OnInspectorGUI();

			if (GUI.changed)
			{
				texturearrayeditor.serializedObject.ApplyModifiedProperties();
			}


			if (texturegenerator.TargetMaterial)
			{
				if (texturegenerator.UseMaterialPath)
				{
					texturegenerator.OutputPath = AssetDatabase.GetAssetPath(texturegenerator.TargetMaterial).Replace("/" + texturegenerator.TargetMaterial.name + ".mat", "");
					texturegenerator.FinalName = texturegenerator.TargetMaterial.name;
				}
				EditorGUILayout.LabelField(texturegenerator.checkMaterial(), text);
			}

			if (!hasErrors)
			{
				if (GUILayout.Button("Extract Materials"))
				{
					texturegenerator.ExtractMaterials();
				}

				if (texturegenerator.ExtractFromTerrain)
				{
					if (GUILayout.Button("Extract From Terrain"))
					{
						texturegenerator.ExtractMaterialsFromTerrain();
					}
				}

				if (GUILayout.Button("Select Output Path"))
				{
					string newpath = EditorUtility.OpenFolderPanel("Texture Array Output", texturegenerator.OutputPath, texturegenerator.OutputPath);
					string absolutepath = Application.dataPath;

					if (newpath != null && newpath != "")
					{
						newpath = newpath.Replace(absolutepath, "Assets");
						texturegenerator.OutputPath = newpath;
					}
				}

				if (GUILayout.Button("Create Texture Array"))
				{
					if (!texturegenerator.AreTexturesReadable())
					{
						if (!EditorUtility.DisplayDialog("Some textures are not readible", "Some textures assigned are not set read/writeable in the asset database." +
							" Enable read/write for those affected textures?", "Do It", "Cancel"))
						{
							return;
						}
					}

					texturegenerator.CreateAllTextureArrays();
				}

				if (GUILayout.Button("Create 3D Texture"))
				{
					if (!texturegenerator.AreTexturesReadable())
					{
						if (!EditorUtility.DisplayDialog("Some textures are not readible", "Some textures assigned are not set read/writeable in the asset database." +
							" Enable read/write for those affected textures?", "Do It", "Cancel"))
						{
							return;
						}
					}

					texturegenerator.CreateAll3DTexture();
				}


				if (GUILayout.Button("Create Texture Atlas"))
				{
					if (!texturegenerator.AreTexturesReadable())
					{
						if (!EditorUtility.DisplayDialog("Some textures are not readible", "Some textures assigned are not set read/writeable in the asset database." +
							" Enable read/write for those affected textures?", "Do It", "Cancel"))
						{
							return;
						}
					}

					texturegenerator.CreateAllTextureAtlases();
				}

				if (GUILayout.Button("Save Profile"))
				{
					texturegenerator.SaveProfile();
				}
			}

			EditorGUILayout.EndScrollView();
		}
	}

	internal class TextureArrayGUI : ShaderGUI
	{
		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
		{
			base.OnGUI(materialEditor, props);

			if (GUILayout.Button("Open Texture Array Creator"))
			{
				TextureArrayGeneratorV2.Init();
			}
		}
	}
}
#endif
