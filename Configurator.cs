using System.IO;
using SimpleJSON;
using UnityEngine;

public class Configurator
{
    private static string path;

    public static void Initialize(string _path, string config)
    {
        path = _path;
        JSONNode json = LoadConfig(config);
        Debug.Log(json);
    }

    private static JSONNode LoadConfig(string config)
    {
        StreamReader reader = new StreamReader(path + string.Format(@"\{0}.json", config));
        string jsonstring = reader.ReadToEnd();
        reader.Close();

        JSONNode json = JSON.Parse(jsonstring);

        if (json["_extends"] != null)
        {
            JSONNode parent = LoadConfig(json["_extends"]);
            json = Assign(parent, json);
        }

        return json;
    }

    private static JSONNode Assign(JSONNode destination, JSONNode source)
    {
        Debug.Log("DESTINATION: " + destination.ToString());
        Debug.Log("SOURCE: " + source.ToString());

        JSONNode json = new JSONObject();
        foreach (string key in destination.Keys)
        {
            Debug.Log("Key: " + key);

            // if (destination[key].IsObject)
            // {
            //     if (source[key].IsObject)
            //     {
            //         json[key] = Assign(destination[key].AsObject, source[key].AsObject);
            //     }
            //     else
            //     {
            //         json[key] = Pick(source[key], destination[key]);
            //     }
            // }

            Debug.Log("source: " + source[key].ToString());
            Debug.Log("destination: " + destination[key].ToString());

            json[key] = Pick(source[key], destination[key]);
            Debug.Log("json[key]: " + json[key].ToString());
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
}