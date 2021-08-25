using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    // -- STATS -- //

    // base stat values
    private int baseHealth = 50;
    private int baseGroundSpeed = 14;

    // Stat Data Models
    UnitDTC enemy;

    // View Models
    public HealthBar healthBar;

    // -- Movement Target -- //
    public Transform destination = null;
    public bool grounded = false;


    // Start is called before the first frame update
    void Start() {
        enemy = new UnitDTC(baseHealth, baseGroundSpeed);
        InitView();
       
    }

    // Update is called once per frame
    void Update() {
        DoGroundMovement();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            //Debug.Log("Collision Enter: " + collision.relativeVelocity.y);
            TakeFallDamage(collision.relativeVelocity.y);
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            grounded = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        Destination currentDestination = other.gameObject.GetComponent<Destination>();

        if (currentDestination) {
            this.destination = currentDestination.GetNextDestination();
        }

    }

    protected void InitView() {
        // Update the views to be current
        UpdateViewHealth();

        // Observe Events to update views correctly
        enemy.GetStat(EDTCStatType.HEALTH).asStatDepletion.SetStatObserverFunction(HealthStatUpdated);
    }

    public int GetGroundSpeed() {
        return enemy.GetStat(EDTCStatType.MOVEMENT_SPEED).asStatReference.GetStatFiltered();
    }

    public int GetCurrentHealth() {
        return enemy.GetStat(EDTCStatType.HEALTH).asStatDepletion.GetRemainingStatValue();
    }

    private void TakeFallDamage(float fallSpeed) {
        enemy.InteractWithStat(StatFactoryDTC.Instance().NewStatInteractionObject(
            EStatInteractionIntent.STAT_DEPLETE,
            EDTCStatType.HEALTH,
            EDTCStatInteractionType.FALL,
            (int)fallSpeed
            )
        );

        // Update View
        UpdateViewHealth();
    }

    private void UpdateViewHealth() {
        if (healthBar) {
            healthBar.UpdateHealth(enemy.GetStat(EDTCStatType.HEALTH).asStatDepletion.GetRemainingStatValue(), enemy.GetStat(EDTCStatType.HEALTH).asStatDepletion.GetStatFiltered());
        }
    }

    private void StartMovement() {
        if (null == destination) {

            if (!GetDestination()) {
                return;
            }

        }

        DoGroundMovement();
    }

    private void DoGroundMovement() {
        if (null == destination) {
            return;
        }

        if (false == grounded) {
            return;
        }

        float step = (GetGroundSpeed() / (gameObject.transform.position - destination.position).magnitude) * Time.fixedDeltaTime;
        float t = 0;

        t += step; // Goes from 0 to 1, incrementing by step each time
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, destination.position, t); // Move objectToMove closer to b
    }

    private bool GetDestination() {
        GameObject targetPad = GameObject.Find("TargetPad");

        if (targetPad) {
            destination = targetPad.transform;
            return true;
        }

        return false;
    }
    
    public void HealthStatUpdated() {
        UpdateViewHealth();
    }

}