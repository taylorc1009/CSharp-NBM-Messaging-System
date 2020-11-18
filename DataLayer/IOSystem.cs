using System;
using System.IO;
using Newtonsoft.Json;

namespace DataLayer
{
    public class IOSystem
    {
        public char header { get; set; }

        public IOSystem() { }

        public String[] importFile(String file)
        {
            try
            {
                string contents = File.ReadAllText(file);
                string[] values = contents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                header = values[0][0];
                switch (header)
                {
                    case 'S':
                        return new String[] { values[1], String.Empty, values[2], "false", String.Empty, String.Empty, String.Empty };
                    case 'T':
                        return new String[] { values[1], String.Empty, values[2], "false", String.Empty, String.Empty, String.Empty };
                    case 'E':
                        if (values[2] == "true") {
                            String[] sirData = values[3].Split(':');
                            return new String[] { values[1], String.Empty, values[4], values[2], sirData[0], sirData[1], sirData[2] };
                        }
                        else
                            return new String[] { values[1], values[3], values[4], values[2], String.Empty, String.Empty, String.Empty };
                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static void exportMessages(string[] output)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter stream = new StreamWriter(Directory.GetCurrentDirectory() + "/messages.json"))
            using (JsonWriter writer = new JsonTextWriter(stream))
            {
                serializer.Serialize(writer, output);
            }
        }

        public static String[] importMessages()
        {
            String[] import = null;
            JsonSerializer deserializer = new JsonSerializer();
            deserializer.Formatting = Formatting.Indented;
            deserializer.NullValueHandling = NullValueHandling.Ignore;

            try
            {
                using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + "/messages.json"))
                {
                    import = (String[])deserializer.Deserialize(reader, typeof(String[]));
                }
                return import;
            }
            catch(FileNotFoundException e)
            {
                return null;
            }
        }
    }
}
