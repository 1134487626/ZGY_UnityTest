using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpritePacker : Editor
{
    //[MenuItem("Tools/选中刚导入的图片打包图集")]
    //static void OnSpritePacker()
    //{
    //    Object[] objectArray = Selection.objects;

    //    if (objectArray?.Length > 0)
    //    {
    //        TextureImporterSettings settings = new TextureImporterSettings
    //        {
    //            spriteMeshType = SpriteMeshType.FullRect,
    //            spriteBorder = Vector4.zero,
    //        };

    //        for (int i = 0; i < objectArray.Length; i++)
    //        {
    //            if (objectArray[i] == null) continue;

    //            string path = AssetDatabase.GetAssetPath(objectArray[i]);
    //            TextureImporter texture = AssetImporter.GetAtPath(path) as TextureImporter;
    //            if (texture != null)
    //            {
    //                texture.textureType = TextureImporterType.Sprite;
    //texture.spriteImportMode = SpriteImportMode.s;
    //                texture.spritePixelsPerUnit = 1;
    //                texture.ReadTextureSettings(settings);
    //                texture.mipmapEnabled = false;
    //                AssetDatabase.ImportAsset(path);
    //            }
    //        }
    //    }
    //}

    //[MenuItem("Toos/打包图集", true)]
    //static bool FindSpriteObject()
    //{
    //    return Selection.objects != null;
    //}
}
