using MyDocsHubLib;
using MyDocsHubLib.Helper;
using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;

namespace MyDocsHubAPI.Services;

public class DocObjectService
{
    public Dictionary<string, List<DocObject>> DocObjects { get; set; } = new();

    public void Change(string ProfilePath, int Id, DocObject docObject)
    {
        DocObject? oldDocObject = Get(ProfilePath, Id);

        if (oldDocObject == null) { return; }

        docObject.FullPath = DocObjectParser.GenerateFullPath(ProfilePath, docObject);
        docObject.Id = Id;

        System.IO.File.Move(oldDocObject.FullPath, docObject.FullPath, true);

        if (DocObjects.TryGetValue("ProfilePath", out List<DocObject>? docObjects) && docObjects != null)
        {
            docObjects.Remove(oldDocObject);
            docObjects.Add(docObject);
            docObjects.SetIds();
        }
    }

    public void Delete(string ProfilePath, int Id)
    {
        if (Get(ProfilePath, Id) is DocObject docObject)
        {
            if (DocObjects.TryGetValue(ProfilePath, out List<DocObject> list) && list != null)
            {
                list.Remove(docObject);
                System.IO.File.Delete(docObject.FullPath);
            }
        }
    }

    public List<DocObject> Get(string ProfilePath)
    {
        ProfilePath = ProfilePath.ToLower();

        if (DocObjects.TryGetValue(ProfilePath, out List<DocObject>? docObjects))
        {
            return docObjects;
        }
        else
        {
            DocObjects[ProfilePath] = new List<DocObject>();

            string[] files = System.IO.Directory.GetFiles(ProfilePath, "*.pdf", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                DocObject? docObject = DocObjectParser.Parse(file);
                if (docObject != null)
                {
                    DocObjects[ProfilePath].Add(docObject);
                }
            }

            DocObjects[ProfilePath].SetIds();

            return DocObjects[ProfilePath];
        }
    }

    public DocObject? Get(string ProfilePath, int Id)
    {
        return Get(ProfilePath).SingleOrDefault(o => o.Id == Id);
    }

    public int Save(string ProfilePath, string OriginalPath, DocObject DocObject, bool copy = false)
    {
        DocObject.FullPath = DocObjectParser.GenerateFullPath(ProfilePath, DocObject);
        DocObject.Id = -1;
        if (copy)
        {
            System.IO.File.Copy(OriginalPath, DocObject.FullPath, true);
        }
        else
        {
            System.IO.File.Move(OriginalPath, DocObject.FullPath, true);
        }
        if (DocObjects.TryGetValue("ProfilePath", out List<DocObject>? docObjects) && docObjects != null)
        {
            docObjects.Add(DocObject);
            docObjects.SetIds();
        }
        return DocObject.Id;
    }
}