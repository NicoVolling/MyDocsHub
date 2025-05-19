using MyDocsHubLib;
using MyDocsHubLib.Helper;

namespace MyDocsHubAPI.Services;

public class ProfileService
{
    private ProfileManager profileManager;

    public ProfileService()
    {
        profileManager = new ProfileManager();
        profileManager.LoadProfiles();
    }

    public void DeleteProfile(Profile Profile)
    {
        profileManager.ProfileList.Remove(Profile);
        profileManager.SaveProfiles();
    }

    public Profile? Get(int Id)
    {
        return profileManager.ProfileList.SingleOrDefault(o => o.Id == Id);
    }

    public List<Profile> GetProfiles()
    {
        return profileManager.ProfileList;
    }

    public void SaveExistingProfile(Profile Profile)
    {
        if (profileManager.ProfileList.SingleOrDefault(o => o.Id == Profile.Id) is Profile existingProfile)
        {
            profileManager.ProfileList.Remove(existingProfile);
            profileManager.ProfileList.Add(Profile);
        }
        profileManager.SaveProfiles();
    }

    public int SaveNewProfile(Profile Profile)
    {
        Profile.Id = -1;
        profileManager.ProfileList.Add(Profile);
        profileManager.ProfileList.SetIds();
        profileManager.SaveProfiles();
        return Profile.Id;
    }
}