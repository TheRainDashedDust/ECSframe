using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class PathUtil 
{
    public static string GetRelativePath(string path)
    {
        string srcPath = path.Replace("\\", "/");
        var retPath = Regex.Replace(srcPath, @"\b.*Assets", "Assets");
        return retPath;
    }
    public static string GetAbsolutePath(string path)
    {
        string srcPath = path.Replace("\\", "/");
        var retPath = Regex.Replace(srcPath, @"\b.*Assets", Application.dataPath);
        return retPath;
    }
}
