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
            //tries to find and import the values from an appropriately formatted text file
            try
            {
                string contents = File.ReadAllText(file);

                //splits the vile by an environment-based new-line (Windows - "\r\n", Unix - '\n')
                string[] values = contents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                //uses the first line of the file to determine the type of the message, thus understanding what data to expect and what to return
                header = values[0][0];
                switch (header)
                {
                    //indexes:
                    //1 - sender, 2 - subject, 3 - message body, 4 - is SIR, 5 - SIR date, 6 - SIR sort code, 7 - SIR nature
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
                //returns a null value if we failed to retrieve the file or the values required for the specific type
                return null;
            }
        }

        //serializes the list of serialized dictionaries
        public static void exportMessages(String file, string[] output)
        {
            JsonSerializer serializer = new JsonSerializer();

            //serialization parameters; null values are bypassed and the text format of the file is indented, rather than one line
            serializer.Formatting = Formatting.Indented;
            serializer.NullValueHandling = NullValueHandling.Ignore;

            //opens the file to be written to, then writes to it using the JSON writer
            using (StreamWriter stream = new StreamWriter(Directory.GetCurrentDirectory() + "/" + file))
            using (JsonWriter writer = new JsonTextWriter(stream))
            {
                //serializes the string[] of serialized dictionaries
                //yes, we double serialize; we cannot serialize the objects here as the DataLayer doesn't have a reference to BusinessLayer, as we need a vice-versa reference in order to get the IOSystem import
                serializer.Serialize(writer, output);
            }
        }

        public static String[] importMessages(String file)
        {
            String[] import = null;

            JsonSerializer deserializer = new JsonSerializer();
            deserializer.Formatting = Formatting.Indented;
            deserializer.NullValueHandling = NullValueHandling.Ignore;

            try
            {
                using (StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + "/" + file))
                {
                    //deserializes the file contents as type string[]
                    import = (String[])deserializer.Deserialize(reader, typeof(String[]));
                }
                
            }
            catch(FileNotFoundException e) { }

            return import;
        }
    }
}
