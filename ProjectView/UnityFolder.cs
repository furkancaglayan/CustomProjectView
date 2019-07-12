using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class UnityFolder
{

    private string folderPath;
    private string folderName;
    private UnityFolder parentFolder;

    private List<UnityFile> child_files;
    private UnityFile[,] groupedFiles;
    private List<UnityFolder> child_folders;

    private GUIContent folderContent;

    private Texture2D folderIcon;

    private bool fold = false;
    private int depth;

    private Rect position;

    public UnityFolder(string folderPath, UnityFolder parentFolder,int depth, Rect position)
    {
        this.folderPath = folderPath;
        this.depth = depth;
        this.position = position;//Position is a variable of the EditorWindow.


        child_folders = FindChildFolders();
        child_files = FindChildFiles();
       
        //Create the object by giving its path. Then get the assetpreview.
        Object folderobj = AssetDatabase.LoadAssetAtPath(this.folderPath,typeof(Object));
        folderIcon = AssetPreview.GetMiniThumbnail(folderobj);


        //Assets/New Folder-> folderName:New Folder
        string[] splitPath = this.folderPath.Split('\\');
        folderName = splitPath[splitPath.Length - 1];

        folderContent = new GUIContent(folderName,folderIcon,folderPath);

        //This is a 2D array to group files by rows of 3.
        groupedFiles = GroupChildFiles(child_files);

    }

    public void VisualizeFolder()
    {
        GUILayout.BeginVertical();

        //Do this to give horizontal space
        GUILayout.BeginHorizontal();
        GUILayout.Space(15*depth);
        fold = EditorGUILayout.Foldout(fold,folderContent,true);
        GUILayout.EndHorizontal();


        if (fold)
        {
            VisualizeChildFiles();
            foreach (var VARIABLE in child_folders)
                VARIABLE.VisualizeFolder();

        }
        
        GUILayout.EndVertical();
    }

    private List<UnityFolder> FindChildFolders()
    {
        //GetDirectories will return all the subfolders in the given path.
        string[] dirs = Directory.GetDirectories(folderPath);
        List<UnityFolder> folders = new List<UnityFolder>();
        foreach (var directory in dirs)
        {
            //Turn all directories into our 'UnityFolder' Object.
            UnityFolder newfolder = new UnityFolder(directory,this,depth+1,position);
            folders.Add(newfolder);
        }
        return folders;
    }

    private List<UnityFile> FindChildFiles()
    {
        //GetFiles is similar but returns all the files under the path(obviously)
        string[] fileNames = Directory.GetFiles(folderPath);
        List<UnityFile> files = new List<UnityFile>();
        foreach (var file in fileNames)
        {
            UnityFile newfile = new UnityFile(file,this);
            //Pass meta files.
            if (newfile.GetExtension().Equals("meta"))
                continue;
            files.Add(newfile);
        }

        return files;

    }

    private UnityFile[,] GroupChildFiles(List<UnityFile> files)
    {
        //This method groups files by rows of 3. You can edit this
        //to change visuals.
        int size = files.Count;
        int rows = (size / 3)+1;
        UnityFile[,] groupedFiles = new UnityFile[rows,3];
        int index = 0;
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < 3; j++)
                if(i*3+j<=size-1)
                    groupedFiles[i, j] = files[index++];
       
        return groupedFiles;
    }

    private void VisualizeChildFiles()
    {
        int size = child_files.Count;
        int rows = (size / 3)+1;

        int i = 0, j = 0;
        for (i = 0; i < rows; i++)
        {
            
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            for (j = 0; j < 3; j++)
            {
                if (i * 3 + j <= size - 1)
                    groupedFiles[i, j].VisualizeFile();
                
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();


        }

    }



    public string GetName()
    {
        return folderPath;
    }

    public int GetDepth()
    {
        return depth;
    }
}
