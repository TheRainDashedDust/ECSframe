using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class FBXToAnimWindow : EditorWindow
{
    public static string assetSrcFolderPath = "Assets";
    public static string assetDstFolderPath = "Assets";
    [MenuItem("Tools/Animation/FBX中提取animationclip")]
    public static void ShowWindow()
    {
        EditorWindow thisWindow = EditorWindow.GetWindow(typeof(FBXToAnimWindow));
        thisWindow.titleContent = new GUIContent("fbx动画资源提取");
        thisWindow.position = new Rect(Screen.width / 2, Screen.height / 2, 600, 800);
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("选择目标源文件夹");
        EditorGUILayout.TextField(assetSrcFolderPath);
        if (GUILayout.Button("选择"))
        {
            assetSrcFolderPath = EditorUtility.OpenFolderPanel("选择文件夹", assetSrcFolderPath, "");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("选择保存源文件夹");
        EditorGUILayout.TextField(assetDstFolderPath);
        if (GUILayout.Button("选择"))
        {
            assetDstFolderPath = EditorUtility.OpenFolderPanel("选择文件夹", assetDstFolderPath, "");
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("开始提取") && assetSrcFolderPath != null && assetDstFolderPath != null)
        {
            Seperate();
        }
    }
    private static void Seperate()
    {
        assetSrcFolderPath = PathUtil.GetAbsolutePath(assetSrcFolderPath);
        var files = Directory.GetFiles(assetSrcFolderPath, "*.fbx");
        string dstPath = PathUtil.GetRelativePath(assetDstFolderPath);
        foreach (var file in files)
        {
            string srcPath = PathUtil.GetRelativePath(file);
            AnimationClip srcclip = AssetDatabase.LoadAssetAtPath(srcPath, typeof(AnimationClip)) as AnimationClip;
            if (srcclip == null)
                continue;

            AnimationClip dstclip = AssetDatabase.LoadAssetAtPath(dstPath, typeof(AnimationClip)) as AnimationClip;
            if (dstclip != null)
                AssetDatabase.DeleteAsset(dstPath);

            string name = Path.GetFileNameWithoutExtension(srcPath);
            //Debug.Log(name);
            AnimationClip tempclip = new AnimationClip();
            EditorUtility.CopySerialized(srcclip, tempclip);
            AssetDatabase.CreateAsset(tempclip, dstPath + "/" + name + ".anim");
            if (File.Exists(dstPath + "/" + name + ".anim"))
            {
                Debug.Log("提取"+ name + "动画片段完成");
            }
            
        }
    }

}
