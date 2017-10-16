using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SpritePostProcessor : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        Debug.Log("Importer: " + textureImporter.assetPath);

        var folderName = Path.GetDirectoryName(textureImporter.assetPath);
        if (folderName.Contains("UI"))
        {
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spritePackingTag = folderName.Split('_')[1];
        }

        AssetDatabase.Refresh();
    }

    void OnPostprocessSprites(Texture2D texture, Sprite sprite)
    {
        Debug.Log("Texture2D: " + texture.name);
        Debug.Log("Sprite: " + sprite.name);
    }
}
