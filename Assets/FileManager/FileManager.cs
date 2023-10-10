using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    public static string getFileContents(string[] _editor_path, string[] _build_path)
    {
        string path = getFilePath(_editor_path, _build_path);

        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }
        return null;
    }
    
    public static string getFileContents(string[] _editor_path)
    {
        string path = getFilePath(_editor_path);

        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }
        return null;
    }
    
    public static string getFilePath(string[] _editor_path, string[] _build_path)
    {
        string path;
        
        #if UNITY_EDITOR
            path = Path.Combine(Application.dataPath, Path.Combine(_editor_path));
        #else
            path = Path.Combine(Application.persistentDataPath, Path.Combine(_build_path));
        #endif
        
        ensureDirectoryExists(Path.GetDirectoryName(path));
        return path;
    }
    
    public static string getFilePath(string[] _editor_path)
    {
        string path;
        path = Path.Combine(Application.dataPath, Path.Combine(_editor_path));
        ensureDirectoryExists(Path.GetDirectoryName(path));
        return path;
    }
    
    private static void ensureDirectoryExists(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
    
    public static string[][] parseCSV(string fileContents)
    {
        string[] lines = fileContents.Split('\n');
        
        string[][] data = new string[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            data[i] = lines[i].Split(',');
        }

        return data;
    }

    public static string[] parseCSVtoRows(TextAsset file)
    {
        return file.text.Split(new string[] { "\n" }, StringSplitOptions.None);
    }

    public static void saveToFile(string[] _editor_path, string[] _build_path, string _content)
    {
        string path = getFilePath(_editor_path, _build_path);

        File.WriteAllText(path, _content);
    }

    public static void saveToCSV(string[] _editor_path, string[] _build_path, string[][] _content)
    {
        string path = getFilePath(_editor_path, _build_path);

        using (StreamWriter sw = new StreamWriter(path))
        {
            for (int i = 0; i < _content.Length; i++)
            {
                sw.WriteLine(string.Join(",", _content[i]));
            }
        }
    }

    public static void saveToCSV(string[] _editor_path, string[] _build_path, List<List<string>> _content)
    {
        string[][] content = new string[_content.Count][];
        
        for (int row_index = 0; row_index < _content.Count; row_index++)
        {
            var row_data = _content[row_index];
            string[] columns = new string[row_data.Count];
            
            for (int column_index = 0; column_index < row_data.Count; column_index++)
            {
                columns[column_index] = row_data[column_index];
            }
            
            content[row_index] = columns;
        }

        saveToCSV(_editor_path, _build_path, content);
    }
}
