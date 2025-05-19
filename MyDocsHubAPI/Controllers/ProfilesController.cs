using Microsoft.AspNetCore.Mvc;
using MyDocsHubAPI.Services;
using MyDocsHubLib;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyDocsHubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly DocumentTypesService documentTypesService;
        private readonly ProfileService profileService;

        public ProfilesController(ProfileService profileService, DocumentTypesService documentTypesService)
        {
            this.profileService = profileService;
            this.documentTypesService = documentTypesService;
        }

        // DELETE api/<ProfilesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            if (profileService.GetProfiles().SingleOrDefault(o => o.Id == id) is Profile profile)
            {
                profileService.DeleteProfile(profile);
            }
        }

        // GET: api/<ProfilesController>
        [HttpGet]
        public IEnumerable<int> Get()
        {
            return profileService.GetProfiles().Select(p => p.Id);
        }

        // GET api/<ProfilesController>/5
        [HttpGet("{id}")]
        public Profile? Get(int id)
        {
            return profileService.GetProfiles().SingleOrDefault(o => id == o.Id);
        }

        // GET api/<ProfilesController>/5
        [HttpGet("{id}/documenttypes")]
        public DocumentTypesManager? GetDocumentTypes(int id)
        {
            if (profileService.GetProfiles().SingleOrDefault(o => o.Id == id) is Profile profile)
            {
                return documentTypesService.Get(profile.Path);
            }
            return null;
        }

        // POST api/<ProfilesController>
        [HttpPost]
        public void Post([FromBody] Profile profile)
        {
            profileService.SaveNewProfile(profile);
        }

        // PUT api/<ProfilesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Profile profile)
        {
            profile.Id = id;
            profileService.SaveExistingProfile(profile);
        }

        // GET api/<ProfilesController>/5
        [HttpPut("{id}/documenttypes")]
        public void PutDocumentTypes(int id, [FromBody] DocumentTypesManager documentTypesManager)
        {
            if (profileService.GetProfiles().SingleOrDefault(o => o.Id == id) is Profile profile)
            {
                documentTypesService.Set(profile.Path, documentTypesManager);
            }
        }
    }
}