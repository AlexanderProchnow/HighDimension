using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
 
public class CSVReader
{
    //static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    // static string SPLIT_RE = @"(,|;)"; OLD
    static string SPLIT_RE = UIManagerDesktop.separator;
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    private static int numLines;
 
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
        // Get separator from Desktop UI
        SPLIT_RE = UIManagerDesktop.separator;

        // import data from resources folder
        TextAsset raw_data = Resources.Load<TextAsset>(path);

        // line split
        var lines = Regex.Split(raw_data.text, LINE_SPLIT_RE);

        numLines = lines.Length;

        // get column names
        var header = Regex.Split(lines[0], SPLIT_RE);

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
            var values = Regex.Split(lines[line], SPLIT_RE);
            
            // assign value to index in data array
            for(var variable = 0; variable < values.Length; variable++) {
                data[line, variable] = values[variable];
            }
        }

        return data;
    }

    public static string[][] IntoJaggedArray(string path) {
        // Get separator from Desktop UI
        SPLIT_RE = UIManagerDesktop.separator;

        // import data from resources folder
        string raw_data = File.ReadAllText(path);
        
        // line split
        var lines = Regex.Split(raw_data, LINE_SPLIT_RE);

        numLines = lines.Length;

        // get column names
        var header = Regex.Split(lines[0], SPLIT_RE);

        // create empty array of shape (#lines, ?)
        string[][] data = new string[lines.Length][];

        // set row 0 in data to be header
        data[0] = header;

        // iterate over each line
        for (int line = 1; line < lines.Length; line++)
        {
            // split values
            var values = Regex.Split(lines[line], SPLIT_RE);
            
            // assign value to index in data array
            data[line] = values;
            
        }
        return data;
    }
    
    public static Tuple<float[,], string[]> IntoFloatArray(string path) {
        string[][] raw_data = IntoJaggedArray(path);
        float[,] data = new float[numLines, raw_data[0].Length];

        // For non-numeric data entries create Array, each index being the index of the variable
        // A list contains all values of that variable
        List<string>[] levels = new List<string>[raw_data[0].Length];

        // Initialize all lists
        for(int i=0; i<levels.Length; i++) {
            levels[i] = new List<string>();
        }

        for(int row=1; row<numLines-1; row++) {
            for(int column=0; column<raw_data[0].Length; column++) {
                string currentValue = raw_data[row][column];
                float fvalue;

                // convert entry to float. If it is not directly convertable (e.g. a string), assign an integer to the value
                if(!float.TryParse(currentValue, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out fvalue)) {
                    // returns -1 if not found
                    int indexOfLevel = levels[column].IndexOf(currentValue);

                    // if new level: assign next number for that variable
                    if (indexOfLevel==-1) {

                        levels[column].Add(currentValue);
                        fvalue = levels[column].Count;
                    
                    // if not new level: assign already determined number for that variable                    
                    } else {
                        fvalue = levels[column].IndexOf(currentValue);
                    }
                }
                data[row, column] = fvalue;
            }
        }
        return new Tuple<float[,], string[]>(data, raw_data[0]);
    }
}