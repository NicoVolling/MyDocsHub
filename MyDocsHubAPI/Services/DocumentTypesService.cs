using MyDocsHubLib;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace MyDocsHubAPI.Services;

public class DocumentTypesService
{
    private const string DocTypeFileName = "DocumentTypes.json";

    private Dictionary<string, DocumentTypesManager> _documentTypeManagers = new();

    public DocumentTypesManager? Get(string ProfilePath)
    {
        if (_documentTypeManagers.TryGetValue(ProfilePath.ToLower(), out DocumentTypesManager? documentTypesManager))
        {
            return documentTypesManager;
        }
        else
        {
            DocumentTypesManager? loadedDocumentTypesManager = Load(ProfilePath);
            if (loadedDocumentTypesManager != null)
            {
                _documentTypeManagers[ProfilePath] = loadedDocumentTypesManager;
            }
            return loadedDocumentTypesManager;
        }
    }

    public void Set(string ProfilePath, DocumentTypesManager documentTypesManager)
    {
        if (_documentTypeManagers.ContainsKey(ProfilePath))
        {
            _documentTypeManagers[ProfilePath] = documentTypesManager;
        }
        else
        {
            _documentTypeManagers.Add(ProfilePath, documentTypesManager);
        }
        Save(documentTypesManager, ProfilePath);
    }

    private DocumentTypesManager? Load(string ProfilePath)
    {
        try
        {
            DocumentTypesManager documentTypesManager = new DocumentTypesManager();

            if (!Directory.Exists(ProfilePath))
            {
                return null;
            }
            string _docTypeFilePath = Path.Combine(ProfilePath, DocTypeFileName);

            if (!File.Exists(_docTypeFilePath))
            {
                Save(documentTypesManager, ProfilePath);
                return null;
            }

            string json = File.ReadAllText(_docTypeFilePath);
            var deserializedTypes = JsonSerializer.Deserialize<List<string>>(json);

            if (deserializedTypes != null && deserializedTypes.Count > 0)
            {
                documentTypesManager.Types.Clear();
                documentTypesManager.Types.AddRange(deserializedTypes.Order());
            }

            return documentTypesManager;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private void Save(DocumentTypesManager documentTypesManager, string ProfilePath)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string json = JsonSerializer.Serialize(documentTypesManager.Types, options);
            string _docTypeFilePath = Path.Combine(ProfilePath, DocTypeFileName);
            File.WriteAllText(_docTypeFilePath, json);
        }
        catch (Exception ex)
        {
            return;
        }
    }
}