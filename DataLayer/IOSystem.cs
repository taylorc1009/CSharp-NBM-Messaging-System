using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;

namespace DataLayer
{
    public class IOSystem
    {
        SMS sms;
        Tweet tweet;
        StandardEmailMessage sem;
        SignificantIncidentReport sir;

        public IOSystem()
        {
        }

        /// <summary>
        /// @param file 
        /// @return
        /// </summary>
        public bool importFile(String file)
        {
            try
            {
                string contents = File.ReadAllText(file);
                string[] values = contents.Split('\n');
                char header = values[0][0];
                if (header == 'S')
                    sms = new SMS(values[1], values[2]);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void exportFile(String file)
        {
            // TODO implement here
        }
    }
}
