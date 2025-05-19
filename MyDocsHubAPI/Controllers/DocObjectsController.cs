using Microsoft.AspNetCore.Mvc;
using MyDocsHubAPI.Services;
using MyDocsHubLib;
using System.Runtime.InteropServices.Marshalling;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyDocsHubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocObjectsController : ControllerBase
    {
        private readonly DocObjectService docObjectService;
        private readonly ProfileService profileService;

        public DocObjectsController(DocObjectService docObjectService, ProfileService profileService)
        {
            this.docObjectService = docObjectService;
            this.profileService = profileService;
        }

        [HttpDelete("{profileId}/{id}")]
        public void Delete(int profileId, int id)
        {
            if (profileService.Get(profileId) is Profile Profile)
            {
                docObjectService.Delete(Profile.Path, id);
            }
        }

        [HttpGet("{profileId}")]
        public IEnumerable<int>? Get(int profileId, [FromQuery] string? receiver, [FromQuery] string? sender, [FromQuery] int? year, [FromQuery] int? month, [FromQuery] bool? outgoing)
        {
            if (profileService.Get(profileId) is Profile Profile)
            {
                IEnumerable<DocObject> docobjects = docObjectService.Get(Profile.Path);

                if (receiver != null)
                {
                    docobjects = docobjects.Where(o => o.Receiver == receiver);
                }

                if (sender != null)
                {
                    docobjects = docobjects.Where(o => o.Sender == sender);
                }

                if (year != null)
                {
                    docobjects = docobjects.Where(o => o.ReceivedDate.Year == year.Value);
                }

                if (month != null)
                {
                    docobjects = docobjects.Where(o => o.ReceivedDate.Month == month.Value);
                }

                if (outgoing != null)
                {
                    docobjects = docobjects.Where(o => o.Outgoing == outgoing.Value);
                }

                return docobjects.Select(o => o.Id);
            }
            return null;
        }

        [HttpGet("{profileId}/details")]
        public IEnumerable<DocObject>? GetDetails(int profileId, [FromQuery] string? receiver, [FromQuery] string? sender, [FromQuery] int? year, [FromQuery] int? month, [FromQuery] bool? outgoing)
        {
            List<DocObject> list = new();
            foreach (int id in Get(profileId, receiver, sender, year, month, outgoing) ?? new List<int>())
            {
                if (GetDetails(profileId, id) is DocObject docObject)
                {
                    list.Add(docObject);
                }
            }
            return list;
        }

        [HttpGet("{profileId}/{id}")]
        public DocObject? GetDetails(int profileId, int id)
        {
            if (profileService.Get(profileId) is Profile Profile)
            {
                return docObjectService.Get(Profile.Path, id);
            }
            return null;
        }

        [HttpGet("{profileId}/years")]
        public IEnumerable<int>? GetYears(int profileId)
        {
            if (profileService.Get(profileId) is Profile Profile)
            {
                return docObjectService.Get(Profile.Path).Select(o => o.ReceivedDate.Year).Distinct();
            }
            return null;
        }

        [HttpGet("{profileId}/{year}/months")]
        public IEnumerable<int>? GetYears(int profileId, int year)
        {
            if (profileService.Get(profileId) is Profile Profile)
            {
                return docObjectService.Get(Profile.Path).Where(o => o.ReceivedDate.Year == year).Select(o => o.ReceivedDate.Month).Distinct();
            }
            return null;
        }

        [HttpPost("{profilePath}")]
        public int? Post(string profilePath, [FromQuery] string originalPath, [FromBody] DocObject docObject, [FromQuery] bool copy = false)
        {
            return docObjectService.Save(profilePath, originalPath, docObject, copy);
        }

        [HttpPut("{profilePath}/{id}")]
        public void Put(string profilePath, int id, [FromBody] DocObject docObject)
        {
            docObjectService.Change(profilePath, id, docObject);
        }
    }
}