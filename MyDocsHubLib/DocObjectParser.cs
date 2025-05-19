using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyDocsHubLib
{
    public static class DocObjectParser
    {
        private static Dictionary<string, int> PropertyMapping { get; } = new() { { "Outgoing", 0 }, { "ReceivedDate", 1 }, { "Type", 2 }, { "Sender", 3 }, { "Subject", 4 }, { "Receiver", 5 } };

        public static string GenerateFullPath(string ProfilePath, DocObject DocObject)
        {
            return System.IO.Path.Combine(ProfilePath, DocObject.Receiver, DocObject.ReceivedDate.ToString("yyyy"), DocObject.ReceivedDate.ToString("yyyy-MM"), $"{(DocObject.Outgoing ? "A" : "E")}_{DocObject.ReceivedDate.ToString("yyyy-MM-dd")}_{DocObject.Type}_{DocObject.Sender}_{DocObject.Subject}.pdf");
        }

        public static DocObject? Parse(string FullPath)
        {
            DocObject? docObject = null;
            TryParse(FullPath, out docObject);
            return docObject;
        }

        public static DocObject? Parse(object? Object)
        {
            if (Object is DocObject docObj)
            {
                return docObj;
            }

            if (Object is string FullPath)
            {
                DocObject? result;
                if (TryParse(FullPath, out result))
                {
                    return result;
                }
            }

            return null;
        }

        public static bool TryParse(string FullPath, out DocObject? docObject)
        {
            docObject = new();

            if (!FullPath.EndsWith(".pdf")) { return false; }

            docObject.FullPath = FullPath;
            string filename = System.IO.Path.GetFileNameWithoutExtension(FullPath);
            string[] parts = filename.Split('_');

            if (parts.Length != 5)
            {
                docObject = null;
                return false;
            }

            DateOnly date;
            if (!DateOnly.TryParseExact(parts[PropertyMapping["ReceivedDate"]], "yyyy-MM-dd", out date))
            {
                docObject = null;
                return false;
            }

            docObject.ReceivedDate = date;
            docObject.Type = parts[PropertyMapping["Type"]];
            docObject.Sender = parts[PropertyMapping["Sender"]];
            docObject.Subject = parts[PropertyMapping["Subject"]];
            docObject.Receiver = System.IO.Path.GetFileName(System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(FullPath)!, "..", "..")));
            docObject.FileSize = (int)new System.IO.FileInfo(FullPath).Length / 1000;

            return true;
        }
    }
}