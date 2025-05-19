using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyDocsHubLib
{
    public class DocumentTypesManager
    {
        public DocumentTypesManager()
        {
            Types = new List<string>
            {
                "Dokument",
                "Brief",
                "Rechnung",
                "Quittung",
                "Mahnung",
                "Vertrag",
                "Kontoauszug",
                "Entgeltabrechnung",
                "Amtsdokument",
                "Förmliche Zustellung",
                "Arztbrief",
                "Arbeitsunfähigkeitsbescheinigung",
                "Bescheid",
                "Urkunde",
                "Zeugnis",
                "Vollmacht",
                "Antrag",
                "Gutachten",
                "Zertifikat",
                "Lizenz",
                "Abmahnung",
                "Gutschrift",
                "Angebot",
                "Einladung",
                "Notiz",
            };
        }

        public List<string> Types { get; set; }
    }
}