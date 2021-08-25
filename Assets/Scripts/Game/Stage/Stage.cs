using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Every stage has at least 1 of the following.
/// - Castle
/// - EnemySpawnPoint
/// - Level
/// 
/// The stage is passed only if all the levels are passed without
/// any of the castles being invaded. The invasion of the Castle
/// occurs by breaking down one of the gates of the castle.
/// </summary>
public class Stage : MonoBehaviour
{

    // Every stage has at least 1 Castle
    public List<Castle> castles;

    // Every stage has at least 1 EnemySpawnPoint
    public List<EnemySpawnPoint> enemySpawnPoints;

    // Every stage has at least 1 Level
    public List<Level> levels;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
