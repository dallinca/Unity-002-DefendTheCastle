using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DatabaseFacadeJson : IDatabaseFacade {

    public static readonly string SAVE_FOLDER = Application.dataPath + "/Save/";
    public static readonly string PROFILE_SAVE_FOLDER = SAVE_FOLDER + "Profiles/";

    public static readonly string PROFILE_NAMES_FILE = SAVE_FOLDER + "ProfileNames.json";

    public List<ProfileName> profileNames; // null state indicates load from database has not yet occurred

    public bool AddProfileName(string profileName) {

        // Load Profile Names to memory if not already done
        if (null == profileNames) {
            LoadProfileNamesData();
        }

        // Cannot add a name that already exists
        if (HasProfileName(profileName)) {
            return false;
        }

        // Add and Save the new Profile Name
        profileNames.Add(new ProfileName(profileName));
        foreach (var name in profileNames) {
            Debug.Log(name.name);
        }
        SaveProfileNames();
        
        return true;
    }

    public List<ProfileName> GetAllProfileNames() {

        // Check if already loaded
        if (null != profileNames) {
            return profileNames;
        }

        LoadProfileNamesData();
        
        return profileNames;
    }

    public ProfileData GetProfileData(string profileName) {
        // Check if ProfileName exists
        if (!HasProfileName(profileName)) {
            return null;
        }

        // Check if ProfileData Exists
        if (File.Exists(PROFILE_SAVE_FOLDER + profileName)) {
            string json = File.ReadAllText(PROFILE_SAVE_FOLDER + profileName);

            ProfileData profileData = JsonUtility.FromJson<ProfileData>(json);

            if (null != profileData) {
                return profileData;
            }
        }

        return new ProfileData(profileName);
    }

    /// <summary>
    /// Allows checking if a profileName already exists, and therefore cannot
    /// be used to create and additional profile
    /// </summary>
    /// <param name="profileName"></param>
    /// <returns>Wether the profile already exists</returns>
    public bool HasProfileName(string profileName) {

        // Load Profile Names to memory if not already done
        if (null == profileNames) {
            LoadProfileNamesData();
        }

        // Check to see if the profileName already exists in the list
        foreach (var pName in profileNames) {
            if (pName.name.CompareTo(profileName) == 0) { // == 0 means Strings are the same
                return true;
            }
        }

        // Could not find the profile Name in the database
        return false;
    }

    public void Init() {
        // Ensure the saving directory is created and available
        if (!Directory.Exists(SAVE_FOLDER)) {
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        // Ensure the profile saving directory is created and available
        if (!Directory.Exists(PROFILE_SAVE_FOLDER)) {
            Directory.CreateDirectory(PROFILE_SAVE_FOLDER);
        }

        // Ensure existence of ProfileName file
        if (!File.Exists(PROFILE_NAMES_FILE)) {
            File.Create(PROFILE_NAMES_FILE).Close();
        }
    }

    public bool SaveProfileData(ProfileData profileData) {

        // Check if profileName exists -- can't save to profileName that doesn't yet exist
        if (!HasProfileName(profileData.name)) {
            return false;
        }

        // Check if profileDataFile exists
        if (!File.Exists(PROFILE_SAVE_FOLDER + profileData.name)) {
            FileStream fs = File.Create(PROFILE_SAVE_FOLDER + profileData.name);
            fs.Close();
        }

        // Update the profileDataFile
        string jsonToSave = JsonUtility.ToJson(profileData);

        File.WriteAllText(PROFILE_SAVE_FOLDER + profileData.name, jsonToSave);
        
        return true;
    }

    private void LoadProfileNamesData() {
        // Attempt to load the profileNames from file
        if (File.Exists(PROFILE_NAMES_FILE)) {
            string savedJson = File.ReadAllText(PROFILE_NAMES_FILE);

            ProfileNames pns = (JsonUtility.FromJson<ProfileNames>(savedJson));

            if (null != pns) {
                profileNames = pns.profileNames;
            }
        }

        if (profileNames == null) {
            profileNames = new List<ProfileName>();
        }
    }

    private void SaveProfileNames() {

        // return if folder does not exist
        if (!Directory.Exists(SAVE_FOLDER)) {
            return;
        }
        
        string jsonToSave = JsonUtility.ToJson(new ProfileNames(profileNames));
        
        File.WriteAllText(PROFILE_NAMES_FILE, jsonToSave);
    }
}
