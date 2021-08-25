using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDatabaseFacade
{
    /// <summary>
    /// Should be called before attempting any operations on the facade.
    /// 
    /// Initializes necessary items for Facade use, such as existence of the
    /// base save folder location.
    /// </summary>
    void Init();

    /// <summary>
    /// Used for retrieving a list of all the profiles in the system.
    /// </summary>
    /// <returns>List of Profile Names in the system</returns>
    List<ProfileName> GetAllProfileNames();

    /// <summary>
    /// Used for retrieving all data for a specific profile
    /// </summary>
    /// <returns>All Data for the specified Profile, or null if not found</returns>
    ProfileData GetProfileData(string profileName);

    /// <summary>
    /// Used to add a new profile to the system
    /// </summary>
    /// <param name="profileName"></param>
    /// <returns>Whether the profile was added</returns>
    bool AddProfileName(string profileName);

    /// <summary>
    /// Allows checking if a profileName already exists, and therefore cannot
    /// be used to create and additional profile
    /// </summary>
    /// <param name="profileName"></param>
    /// <returns>Wether the profile already exists</returns>
    bool HasProfileName(string profileName);

    /// <summary>
    /// Used to save the current data of the profile.
    /// 
    /// Will not be successful if the inner specified ProfileName does not
    /// already exist in the system.
    /// </summary>
    /// <param name="profileData"></param>
    /// <returns>Whether the save was successful</returns>
    bool SaveProfileData(ProfileData profileData);
    
}
