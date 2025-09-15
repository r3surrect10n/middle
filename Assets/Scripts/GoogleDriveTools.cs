using System;
using System.Collections.Generic;
using System.Text;
using UnityGoogleDrive;
using UnityGoogleDrive.Data;

public static class GoogleDriveTools
{
    public static List<File> FileList()
    {
        List<File> output= new List<File>();
        GoogleDriveFiles.List().Send().OnDone += fileList => { output = fileList.Files; };
        return output;
    }

    public static File Upload(String obj)
    {
        var file = new UnityGoogleDrive.Data.File { Name = "GameData.json", Content = Encoding.ASCII.GetBytes(obj) };
        GoogleDriveFiles.Create(file).Send();
        return file;
    }

    public static File Download(String fileId)
    {
        File output = new File();
        GoogleDriveFiles.Download(fileId).Send().OnDone += file => { output = file; };
        return output;
    }
}
