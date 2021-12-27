using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class GetPNGTexture
{
    [MenuItem("Assets/GetPNGTexture", false, 2)]
    public static void Main()
    {
        var selectObjs = Selection.objects;
        if (selectObjs == null || selectObjs.Length == 0)
        {
            return;
        }

        foreach (var child in selectObjs)
        {
            ChangeTexture(child);
        }
    }

    private static void ChangeTexture(Object obj)
    {
        if (obj is Texture2D)
        {
            ChangeTexture(obj as Texture2D);
        }
    }

    private static void ChangeTexture(Texture2D texture)
    {
        ChangeTextureReadable(texture);

        var pixels = texture.GetPixels();

        Texture2D result = new Texture2D(texture.width, texture.height);
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].r > 0.15f && pixels[i].g > 0.15f && pixels[i].b > 0.15f)
            {
                pixels[i] = new Color(1, 1, 1, 0);
            }
        }

        result.SetPixels(pixels);

        result.Apply();
        var data = result.EncodeToPNG();
        File.WriteAllBytes($"{Application.dataPath}/Result/{texture.name}_changed.png", data);
        AssetDatabase.Refresh();
    }

    private static void ChangeTextureReadable(Texture2D texture)
    {
        TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(texture));
        if (textureImporter.isReadable == false)
        {
            textureImporter.isReadable = true;
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(texture));
        }
    }
}
