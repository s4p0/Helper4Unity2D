using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Linq;
using System;
using System.Security.Cryptography;
using System.Text;

[RequireComponent(typeof(SpriteLibrary))]
[RequireComponent(typeof(SpriteResolver))]
public class SpriteLibToAnimation : MonoBehaviour
{
    [SerializeField]
    private SpriteLibraryAsset[] libraries = new SpriteLibraryAsset[0];
    
    private SpriteResolver myResolver;
    private SpriteLibrary myLib;

    [ContextMenu("Create Animations")]
    public void CreateAnimations()
    {
        myResolver = GetComponent<SpriteResolver>();
        myLib = GetComponent<SpriteLibrary>();

        foreach (var lib in libraries)
        {
            myLib.spriteLibraryAsset = lib;
            myLib.RefreshSpriteResolvers();

            var listValues = new List<float>();

            foreach (var category in myLib.spriteLibraryAsset.GetCategoryNames())
            {
                listValues.Clear();
                foreach (var label in myLib.spriteLibraryAsset.GetCategoryLabelNames(category))
                {
                    myResolver.SetCategoryAndLabel(category, label);
                    var spriteKeyInfo = myResolver.GetType().GetField("m_SpriteKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var value = (float)spriteKeyInfo.GetValue(myResolver);

                    listValues.Add(value);

                }

                CreateAnimationClip(listValues, category, lib.name);

            }
        }
    }

    void CreateAnimationClip(List<float> values, string categoryName, string folder)
    {
        var path = $"Assets/Animations/{folder}";
        if(!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets/Animations", folder);
        }

        AnimationClip animClip = new AnimationClip();
        animClip.wrapMode = WrapMode.Loop;

        var framerate = 10;
        float timePerFrame = 1.0f / framerate;
        float currentTime = 0.0f;
        
        Keyframe[] keys = new Keyframe[values.Count];
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i] = new Keyframe(currentTime, values[i]);
            
            currentTime += timePerFrame;
        }

        var animCurve = new AnimationCurve(keys);
        

        for (int aux = 0; aux < animCurve.keys.Length; aux++)
        {
            AnimationUtility.SetKeyBroken(animCurve, aux, true);
            AnimationUtility.SetKeyLeftTangentMode(animCurve, aux, AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(animCurve, aux, AnimationUtility.TangentMode.Constant);
        }

        animClip.SetCurve("", typeof(SpriteResolver), "m_SpriteKey", animCurve);
        animClip.frameRate = 10;
        

        AssetDatabase.CreateAsset(animClip, $"{path}/{categoryName}.anim");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}
