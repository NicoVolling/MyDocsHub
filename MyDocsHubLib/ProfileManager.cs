using MyDocsHubLib.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyDocsHubLib
{
    public class ProfileManager
    {
        public const string ProfilesFile = "Profiles.json";

        public List<Profile> ProfileList { get; set; } = new();

        public void LoadProfiles()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GlobalConst.AppName, ProfilesFile);

            if (!File.Exists(path))
            {
                this.ProfileList = new();
                return;
            }

            string json = File.ReadAllText(path);

            List<Profile>? ProfileList = JsonSerializer.Deserialize<List<Profile>>(json);

            this.ProfileList = ProfileList?.SetIds() ?? new();
        }

        public void SaveProfiles()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GlobalConst.AppName, ProfilesFile);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string json = JsonSerializer.Serialize(ProfileList, options);

            File.WriteAllText(path, json);
        }
    }
}