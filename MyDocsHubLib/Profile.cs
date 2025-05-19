using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyDocsHubLib
{
    public class Profile : BaseObject
    {
        private DocumentTypesManager? documentTypeManager;

        public Profile()
        {
        }

        public bool DefaultProfile { get; set; }

        public string DefaultReceiver { get; set; } = "";

        public string Name { get; set; } = "";

        public string Path { get; set; } = "";

        public override string ToString()
        {
            return $"{Name} ({DefaultReceiver})";
        }
    }
}