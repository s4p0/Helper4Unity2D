using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SpriteLibraryCreator : MonoBehaviour
{
    [SerializeField]
    private new string name;
    [SerializeField]
    private AnimationCollection[] animationCollections = new AnimationCollection[0];

    [SerializeField]
    private Texture2D[] textureCollections = new Texture2D[0];

    private Sprite[] GetSpriteFromTexture2D(Texture2D texture)
    {
        string spriteSheet = AssetDatabase.GetAssetPath(texture);
        return AssetDatabase.LoadAllAssetsAtPath(spriteSheet)
            .OfType<Sprite>()
            .ToArray();
    }
    
    [ContextMenu("Create Sprite Library")]
    public void Do()
    {
        foreach (var texture in textureCollections)
        {
            var asset = ScriptableObject.CreateInstance<SpriteLibraryAsset>();

            var sprites = GetSpriteFromTexture2D(texture);
            var animationCollectionIndex = 0;
            var animationCollection = animationCollections[animationCollectionIndex];

            var spritePerAnimationIndex = 0;
            for (int i = 0; i < sprites.Length; i++)
            {
                asset.AddCategoryLabel(sprites[i], animationCollection.Name, $"{animationCollection.Name}_{spritePerAnimationIndex}");



                spritePerAnimationIndex++;

                if (spritePerAnimationIndex > animationCollection.SpriteCount - 1)
                {
                    spritePerAnimationIndex = 0;
                    animationCollectionIndex++;

                    if (animationCollectionIndex < animationCollections.Length)
                        animationCollection = animationCollections[animationCollectionIndex];
                }
            }


            AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath($"Assets/{texture.name}.asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(asset);

        }
    }
    
}

[Serializable]
public class AnimationCollection
{
    [SerializeField]
    private string name;
    [SerializeField]
    private int spriteCount;

    public string Name { get => name;  }
    public int SpriteCount { get => spriteCount;  }
    public override string ToString()
    {
        return $"{name}: {spriteCount}";
    }
}
