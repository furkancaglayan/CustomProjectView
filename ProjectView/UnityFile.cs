using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnityFile
{


    private string path;
    private string extension;
    private string fileName;


    private UnityFolder parentFolder;
    private Texture2D fileIcon;
    private GUIContent fileContent;


    public UnityFile(string path, UnityFolder parentFolder)
    {
        this.path = path;
        this.parentFolder = parentFolder;
        extension = FindExtension(path);
        fileName = FindFileName(path);



        Object fileobj = AssetDatabase.LoadAssetAtPath(this.path, typeof(Object));
        fileIcon = AssetPreview.GetMiniThumbnail(fileobj);

        fileContent = new GUIContent(fileName,fileIcon,path);


    }

    private string FindExtension(string path)
    {
        string[] splitPath = path.Split('.');
        string ext = splitPath[splitPath.Length - 1];
        return ext;
    }
    private string FindFileName(string path)
    {
        string[] splitPath = path.Split('\\');
        string fullName = splitPath[splitPath.Length - 1];
        string splitExt = fullName.Split('.')[0];

        return splitExt;
    }
    public void VisualizeFile()
    {
        GUILayout.BeginHorizontal(EditorStyles.helpBox,GUILayout.Width(256));
        GUILayout.Label(fileContent,GUILayout.Width(256),GUILayout.Height(64));
        GUILayout.EndHorizontal();
    }
    public string GetPath()
    {
        return path;

    }
    public string GetExtension()
    {
        return extension;

    }


}
