using System;
using System.Collections.Generic;
using System.Text;

namespace MoreMountains
{
	using UnityEngine;
	using System.IO;
	using System.Linq;
    using UnityEngine.AddressableAssets;

	//Currently Unused, will revive if i make custom assets
	public static class Assets
	{
		/*public static AssetBundle CrimsonAssets;
		public static AssetBundle AmberAssets;
		public const string CrimsonBundleName = "crimsonmountainassets";
		public const string AmberBundleName = "ambermountainassets";
	    public const string assetBundleFolder = "assetBundles";

		public static string CrimsonBundlePath
		{
			get
			{
				string myPath = Path.Combine(Path.GetDirectoryName(MoreMountainsPlugin.thePluginInfo.Location), assetBundleFolder, CrimsonBundleName);
				Log.LogInfo(myPath);
				return myPath;
			}
		}

        public static string AmberBundlePath
        {
            get
            {
                string myPath = Path.Combine(Path.GetDirectoryName(MoreMountainsPlugin.thePluginInfo.Location), assetBundleFolder, AmberBundleName);
                Log.LogInfo(myPath);
                return myPath;
            }
        }

        public static void Init()
		{
			CrimsonAssets = AssetBundle.LoadFromFile(CrimsonBundlePath);
			AmberAssets = AssetBundle.LoadFromFile(AmberBundlePath);
		}

		static List<Material> materialsWithSwappedShaders;
		private static void SwapShadersFromMaterials(AssetBundle assetBundle)
		{
			var materials = assetBundle.LoadAllAssets<Material>().Where(mat => mat.shader.name.StartsWith("StubbedRoR2"));
			foreach (Material material in materials)
			{
				if (material != null && material.shader != null)
				{
					Log.LogInfo("Logging individual mat shader names");
					Log.LogInfo(material.shader.name);
					try
					{
						SwapShader(material);
					}
					catch (Exception e) { Debug.LogError(e); }
				}
			}
		}
		private static async void SwapShader(Material material)
		{
			var shaderName = material.shader.name.Substring("Stubbed".Length);
			var adressablePath = $"{shaderName}.shader";
			Log.LogInfo(adressablePath);
			var asyncOp = Addressables.LoadAssetAsync<Shader>(adressablePath);
			var shaderTask = asyncOp.Task;
			var shader = await shaderTask;
			material.shader = shader;
            materialsWithSwappedShaders.Add(material);
        }
	}*/
	}
}
