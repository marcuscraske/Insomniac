using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Core
{
    public class ConfigFile
    {
        // Purpose: creates and parses configuration that can be written and read from file, which
        // is useful to save e.g. player and game data. It's also quite simple to edit manually
        // and to be accessed/modified by external applications.

        // Config file format: <Group name> <Key=rofl> </Group name> E.g.:
        // <Player> <- Group start tag
        // <Name=New player> <- A group key with a value
        // <Type=Player> <- A group key with a value
        // </Player> <- Group end tag

        /// <summary>
        /// Items in the parent go in the group [DEFAULT].
        /// </summary>
        public Hashtable Groups = new Hashtable();
        /// <summary>
        /// Adds a key and automatically creates the group.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddKey(string group, string name, string value)
        {
            // If the group is blank, the group is [DEFAULT]
            if (group == "")
            {
                group = "[DEFAULT]";
            }
            // Check if it exists, else create
            if (!Groups.Contains(group))
            {
                Groups.Add(group, new Hashtable());
            }
            // Add key - if it exists, replace existing value
            if (((Hashtable)Groups[group]).Contains(name))
            {
                ((Hashtable)Groups[group])[name] = value;
            }
            else
            {
                ((Hashtable)Groups[group]).Add(name, value);
            }
        }
        /// <summary>
        /// Removes a key; safe protected against non-existant keys and groups.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="name"></param>
        public void RemoveKey(string group, string name)
        {
            // If the group is blank, the group is [DEFAULT]
            if (group == "")
            {
                group = "[DEFAULT]";
            }
            // Check the group exists
            if (Groups.Contains(group))
            {
                Hashtable temp = (Hashtable)Groups[group];
                if (temp.Contains(name))
                {
                    temp.Remove(name);
                }
            }
        }
        /// <summary>
        /// Removes a group; you can also remove the default group and theres protection against non-existant keys and groups.
        /// </summary>
        /// <param name="group"></param>
        public void RemoveGroup(string group)
        {
            // If the group is blank, the group is [DEFAULT]
            if (group == "")
            {
                group = "[DEFAULT]";
            }
            // Check the group exists
            if (Groups.Contains(group))
            {
                Groups.Remove(group);
            }
        }
        /// <summary>
        /// Saves the configuration to the file specified.
        /// </summary>
        /// <param name="str"></param>
        public void SaveToFile(string str)
        {
            // Uses "ToString()" which is a function that returns the configuration as a
            // string for this configuration class to re-read in the future
            File.WriteAllText(str, ToString());
        }
        /// <summary>
        /// Returns the configuration as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // The config file will be stored in this variable
            string str = "";
            Hashtable temp;
            foreach (DictionaryEntry de in Groups)
            {
                temp = (Hashtable)de.Value;
                // Write group start-tag
                str += "<" + de.Key + ">" + Environment.NewLine;
                // Add keys
                foreach (DictionaryEntry de2 in temp)
                {
                    str += "<" + de2.Key + "=" + de2.Value + ">" + Environment.NewLine;
                }
                // Write group end-tag
                str += "</" + de.Key + ">" + Environment.NewLine;
            }
            return str;
        }
        /// <summary>
        /// Gets a string and loads valid groups and keys within it.
        /// </summary>
        /// <param name="str"></param>
        public void Parse(string str)
        {
            // Remove any new line chars
            str = str.Replace(Environment.NewLine, "");
            // Loop through each tag
            string current_group = "[DEFAULT]";
            string[] temp;
            foreach (string str2 in str.Split('<'))
            {
                // Ensure its a valid tag
                if (str2.EndsWith(">"))
                {
                    // If it contains an equals sign, its a key - else its a group
                    if (str2.Contains("="))
                    {
                        // Adds the key
                        temp = str2.Split('=');
                        AddKey(current_group, temp[0], temp[1].Remove(temp[1].Length - 1, 1));
                    }
                    else
                    {
                        // Sets the group currently being read in the configuration
                        current_group = str2.Remove(str2.Length - 1, 1);
                    }
                }
            }

        }
        /// <summary>
        /// Loads groups and keys from a valid config file.
        /// </summary>
        /// <param name="str"></param>
        public void LoadFromFile(string str)
        {
            // Check the file exists, if so...
            if (File.Exists(str))
            {
                // The file is read and 
                Parse(File.ReadAllText(str));
            }
        }
        /// <summary>
        /// Gets a key; if the key does not exist, a blank string is returned.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetKey(string group, string key)
        {
            // If the group is blank, set it to [DEFAULT]
            if (group == "")
            {
                group = "[DEFAULT]";
            }
            // Find the key, else return a blank string
            if (Groups.Contains(group))
            {
                if (((Hashtable)Groups[group]).Contains(key))
                {
                    return ((Hashtable)Groups[group])[key].ToString();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Similar to AddKey - however if the key exists, the new value is added to it.
        /// </summary>
        public void AddToKey(string group, string name, int value)
        {
            string temp2 = GetKey(group, name);
            if(temp2 != "")
            {
                value += Convert.ToInt32(temp2);
            }
            AddKey(group, name, value.ToString());
        }
    }
}