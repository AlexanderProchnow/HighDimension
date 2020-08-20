using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
 
public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };
 
    // UNUSED
    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load<TextAsset>(file);
 
        var lines = Regex.Split (data.text, LINE_SPLIT_RE);
 
        if(lines.Length <= 1) return list;
 
        var header = Regex.Split(lines[0], SPLIT_RE);
        for(var i=1; i < lines.Length; i++) {
 
            var values = Regex.Split(lines[i], SPLIT_RE);
            if(values.Length == 0 ||values[0] == "") continue;
 
            var entry = new Dictionary<string, object>();
            for(var j=0; j < header.Length && j < values.Length; j++ ) {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if(int.TryParse(value, out n)) {
                    finalvalue = n;
                } else if (float.TryParse(value, out f)) {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add (entry);
        }
        return list;
    }

    public static string[,] IntoMultidimensionalArray(string path) {

        // import data from resources folder
        TextAsset raw_data = Resources.Load<TextAsset>(path);

        // line split
        var lines = Regex.Split(raw_data.text, LINE_SPLIT_RE);

        // get column names
        var header = Regex.Split(lines[0], @",");

        // create empty array of shape (#lines, #variables)
        string[,] data = new string[lines.Length, header.Length];

        // set row 0 in data to be header
        for(var variable = 0; variable < header.Length; variable++) {
             data[0, variable] = header[variable];
        }

        // iterate over each line
        for (int line = 1; line < lines.Length; line++)
        {
            // split values
            var values = Regex.Split(lines[line], @",");
            
            // assign value to index in data array
            for(var variable = 0; variable < values.Length; variable++) {
                data[line, variable] = values[variable];
            }
        }

        return data;
    }

    public static string[][] IntoJaggedArray(string path) {

        // import data from resources folder
        TextAsset raw_data = Resources.Load<TextAsset>(path);

        // line split
        var lines = Regex.Split(raw_data.text, LINE_SPLIT_RE);

        // get column names
        var header = Regex.Split(lines[0], @",");

        // create empty array of shape (#lines
        string[][] data = new string[lines.Length][];

        // set row 0 in data to be header
        data[0] = header;

        // iterate over each line
        for (int line = 1; line < lines.Length; line++)
        {
            // split values
            var values = Regex.Split(lines[line], @",");
            
            // assign value to index in data array
            data[line] = values;
            
        }
        return data;
    }
}