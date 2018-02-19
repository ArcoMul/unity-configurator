using System;
using System.IO;
using SimpleJSON;
using UnityEngine;
using System.Linq;

public class Configurator
{
    private static string path;
    private static JSONNode json;

    public static void Initialize(string _path, string config)
    {
        path = _path;
        json = LoadConfig(config);
    }

    public static JSONNode GetJson()
    {
        return json;
    }

    private static JSONNode LoadConfig(string config)
    {
        StreamReader reader = new StreamReader(path + string.Format(@"\{0}.json", config));
        string jsonstring = reader.ReadToEnd();
        reader.Close();

        JSONNode json = JSON.Parse(jsonstring);
        JSONNode result;

        if (json["_extends"] != null)
        {
            JSONNode parent = LoadConfig(json["_extends"]);
            result = Assign(parent, json);
        }
        else
        {
            result = json.AsObject;
        }

        return result;
    }

    private static JSONNode Assign(JSONNode destination, JSONNode source)
    {
        JSONNode json = source;
        foreach (string key in destination.Keys)
        {
            if (destination[key].IsObject)
            {
                if (source[key].IsObject)
                {
                    json[key] = Assign(destination[key].AsObject, source[key].AsObject);
                }
                else
                {
                    json[key] = Pick(source[key], destination[key]);
                }
            }
            else
            {
                json[key] = Pick(source[key], destination[key]);
            }
        }
        return json;
    }

    private static JSONNode Pick(JSONNode first, JSONNode second)
    {
        if (first != null)
        {
            return first;
        }
        return second;
    }

    private static JSONNode GetJSONNode(string path)
    {
        string[] paths = path.Split('.');
        JSONNode depth = json;
        foreach (string p in paths)
        {
            if (depth[p] == null)
            {
                return null;
            }
            depth = depth[p];
        }
        return depth;
    }

    public static string Get(string path)
    {
        return GetJSONNode(path).ToString();
    }
    public static int GetInt(string path)
    {
        return GetJSONNode(path);
    }
    public static bool GetBool(string path)
    {
        return GetJSONNode(path).AsBool;
    }
    public static float GetFloat(string path)
    {
        return GetJSONNode(path).AsFloat;
    }
    public static double GetDouble(string path)
    {
        return GetJSONNode(path).AsDouble;
    }
    public static JSONObject GetObject(string path)
    {
        return GetJSONNode(path).AsObject;
    }

    public static JSONArray GetArray(string path)
    {
        JSONNode n = GetJSONNode(path);
        if (n == null)
        {
            return null;
        }
        return n.AsArray;
    }
    public static string[] GetStringArray(string path)
    {
        return GetArray(path).Children.Select(node => node.ToString()).ToArray();
    }
    public static int[] GetIntArray(string path)
    {
        return GetArray(path).Children.Select(node => node.AsInt).ToArray();
    }
    public static bool[] GetBoolArray(string path)
    {
        return GetArray(path).Children.Select(node => node.AsBool).ToArray();
    }
    public static float[] GetFloatArray(string path)
    {
        return GetArray(path).Children.Select(node => node.AsFloat).ToArray();
    }
    public static double[] GetDoubleArray(string path)
    {
        return GetArray(path).Children.Select(node => node.AsDouble).ToArray();
    }

}