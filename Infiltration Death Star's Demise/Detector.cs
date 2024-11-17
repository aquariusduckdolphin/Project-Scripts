using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    //NOTES:
    //now that there is a function to reset frame by frame target data,
    //it would be better to reuse old targets in vision and audio lists instead of clearing the Lists each cycle

    //if target is remembered but no longer audible or no longer visible store its last known location to search for it

    //program in ability for other senses like smell/clairvoyance/etc
    //make toggles for turning off different senses for bots that can only hear, only see, etc

    //prioritize - damage amount & visible > damage direction but not visible > visible > audible > remembered but no longer seen/heard
    //  -make target focus a priority so it tries to maintain the same target if possible
    //  -sort the Remembered Targets with an IComparer based on visibility and damage dealt so you can just pick element 0 from List
    //  -regardless of damage, bot should forget a target that has not been visible for too long
    //for damage direction but not visible vs visible not damaging
    //1) calculate whether you can kill visible target faster than you would die
    //2) calculate whether you can turn fast enough to shoot at them before you die
    //3) calculate if it would be better to run and hide

    //separate script class for targeting selection
    //  -should be able to request highest damage, vision, audio, etc from this class
    //  -allows you to set up different targeting priorities for different types of enemies
    //assign target priorities, choose target based on highest priority, remember all targets above a certain priority threshold
    //make blue version of detector bot

    public SwarmBot.Team team = SwarmBot.Team.red;
    public bool uiOn = true;

    public enum DetectorState { clear, warning, alert }

    public DetectorState visionDetected = DetectorState.clear;
    public DetectorState audioDetected = DetectorState.clear;
    public DetectorState damageDetected = DetectorState.clear;

    [System.Serializable]
    public class TargetData
    {
        public Transform transform;

        public float targetPriority = 0f;

        public bool damagedMe = false;
        public float damageAmount = 0f;
        public float secondsSinceLastDamage = 0f;

        public bool isVisible = false;
        public bool canSeeHead = false;
        public bool canSeeChest = false;
        [HideInInspector]
        public float centerOfViewFactor = 0f;
        [HideInInspector]
        public float distanceFactor = 0f;
        [HideInInspector]
        public float coverFactor = 0f;
        public float visionAlertLevel = 0f;

        public bool isAudible = false;
        public float audioAlertLevel = 0f;

        [HideInInspector]
        public Health health;
        [HideInInspector]
        public Speedometer sm;
        [HideInInspector]
        public Transform chest;
        [HideInInspector]
        public Transform head;
        [HideInInspector]
        public Vector3 targetDirectionVector;
        [HideInInspector]
        public float centerOfVisionProximity = 0f;
        [HideInInspector]
        public float distance = 0f;
        [HideInInspector]
        public float movementSpeed = 0f;
        [HideInInspector]
        public float currentAudioVolumeLevel = 0f;

        public TargetData(Transform target, Transform detectorTransform)
        {
            transform = target;
            health = transform.GetComponent<Health>();
            sm = transform.GetComponent<Speedometer>();
            chest = health.chest;
            head = health.head;
            targetDirectionVector = Vector3.Normalize((transform.position - detectorTransform.position));
            centerOfVisionProximity = Vector3.Dot(targetDirectionVector, detectorTransform.forward);
            distance = Vector3.Distance(detectorTransform.position, transform.position);
        }
        public TargetData(Vector3 targetDirection, Transform detectorTransform)
        {
            transform = null;
            targetDirectionVector = Vector3.Normalize(targetDirection);
            centerOfVisionProximity = Vector3.Dot(targetDirectionVector, detectorTransform.forward);
        }
    }

    [System.Serializable]
    public class DamageDirection
    {
        public GameObject indicator;
        public Renderer rend;

        public Vector3 direction;
        public float damageRecieved = 0f;
        public float secondsSinceLastDamage = 0f;

        public bool inViewFrustrum = false;
    }

    [Header("GENERAL DETECTION SETTINGS")]
    [Tooltip("Determines the number of seconds between cycles of the detector system (0.25 seconds recommended).")]
    //Smaller number = more responsive detection, but more computationally intensive
    public float detectorResponseTime = 0.25f;
    [Tooltip("Targets outside of this range will be ignored.")]
    public float maxDetectRange = 80f;
    //Layers that could contain a target.
    private LayerMask generalDetectMask;
    [Tooltip("All possible targets that are in range.")]
    private Collider[] possibleTargetColliders;
    [Tooltip("All possible targets that are in range.")]
    [HideInInspector]
    public List<TargetData> possibleTargetsData;

    [Header("TARGET MEMORY")]
    [Tooltip("Targets that will be remembered across multiple frames.")]
    public List<TargetData> rememberedTargets;

    [Header("VISION TARGETS")]
    [Tooltip("All valid targets that can be seen.")]
    public List<TargetData> visionTargets;
    public TargetData closestVisibleTarget;
    [Header("VISION DETECTION SETTINGS")]
    public float minFrustrumDotProduct = 0.5f;
    public float visionWarningMaxDistance = 80f;
    public float visionAlertMaxDistance = 20f;
    [Header("VISION TARGET PRIORITY WEIGHTING")]
    public float visionFrustrumWeight = 0.15f;
    public float visionDistanceWeight = 0.55f;
    public float visionCoverWeight = 0.3f;


    [Header("AUDIO TARGETS")]
    [Tooltip("All valid targets that can be heard.")]
    public List<TargetData> audioTargets;
    public TargetData loudestAudibleTarget;
    [Header("AUDIO DETECTION SETTINGS")]
    public float maxAudioDetectRange = 30f;
    [HideInInspector]
    public float maxAudioDetectVolume = 5f;
    public float audioWarningMinLoudness = 0.25f;
    public float audioAlertMinLoudness = 0.65f;

    [Header("DAMAGED ME TARGETS")]
    [Tooltip("All valid targets that have damaged me.")]
    public List<TargetData> damagedMeTargets;
    public TargetData mostDamageTarget;
    [Header("DAMAGE DIRECTIONS")]
    [Tooltip("How much damage you have recieved from 12 primary directions.")]
    public DamageDirection[] damageDirections;
    public DamageDirection mostDamageFromDirection;
    [Header("DAMAGE DETECTION SETTINGS")]
    public float damageForAlert = 25f;
    public float damageForWarning = 1f;
    public float damageFadePerSecond = 5f;
    public float damageExpiresAfterSeconds = 3.5f;

    public Vector3 testDamageRaycast;

    public Gradient warningColorGradient;
    [HideInInspector]
    public Renderer visionIndicator;
    private Material visionMat;
    [HideInInspector]
    public Renderer audioIndicator;
    private Material audioMat;
    [HideInInspector]
    public Renderer damageIndicator;
    private Material damageMat;

    void Start()
    {
        if (team == SwarmBot.Team.red) { generalDetectMask = LayerMask.GetMask("BlueBot"); }
        if (team == SwarmBot.Team.blue) { generalDetectMask = LayerMask.GetMask("RedBot"); }

        InitDamageDirections();

        visionMat = visionIndicator.material;
        audioMat = audioIndicator.material;
        damageMat = damageIndicator.material;

        if (uiOn) { ShowUI(); }
        else { HideUI(); }

        StartCoroutine(RefreshDetectors());
    }

    void InitDamageDirections()
    {
        foreach(DamageDirection damageDirection in damageDirections)
        {
            damageDirection.rend = damageDirection.indicator.GetComponent<Renderer>();
            damageDirection.direction = Vector3.Normalize(damageDirection.indicator.transform.forward);
        }
    }

    private IEnumerator RefreshDetectors()
    {
        ResetAllTargetData();

        GatherPossibleTargetData();

        CheckVisionAll();
        CheckAudioAll();
        CheckDamage();

        RememberTargets();

        DisplayUIIndicators();

        yield return new WaitForSeconds(detectorResponseTime);
        StartCoroutine(RefreshDetectors());
    }

    private void ResetAllTargetData()
    {
        visionDetected = DetectorState.clear;
        audioDetected = DetectorState.clear;
        damageDetected = DetectorState.clear;

        ResetAllRememberedTargetFrameData();

        possibleTargetsData.Clear();
        visionTargets.Clear();
        closestVisibleTarget = null;
        audioTargets.Clear();
        loudestAudibleTarget = null;
    }

    private void ResetAllRememberedTargetFrameData()
    {
        foreach(TargetData targetData in rememberedTargets)
        {
            ResetFrameByFrameTargetData(targetData);
        }
    }

    private void ResetFrameByFrameTargetData(TargetData targetData)
    {
        targetData.targetPriority = 0f;

        targetData.isVisible = false;
        targetData.canSeeChest = false;
        targetData.canSeeHead = false;
        targetData.centerOfViewFactor = 0f;
        targetData.distanceFactor = 0f;
        targetData.coverFactor = 0f;
        targetData.visionAlertLevel = 0f;

        targetData.isAudible = false;
        targetData.audioAlertLevel = 0f;

        targetData.movementSpeed = 0f;
        targetData.currentAudioVolumeLevel = 0f;

        targetData.targetDirectionVector = Vector3.Normalize((targetData.transform.position - transform.position));
        targetData.centerOfVisionProximity = Vector3.Dot(targetData.targetDirectionVector, transform.forward);
        targetData.distance = Vector3.Distance(transform.position, targetData.transform.position);
    }

    private void GatherPossibleTargetData()
    {
        //Only search through targets within the maximum range
        possibleTargetColliders = Physics.OverlapSphere(transform.position, maxDetectRange, generalDetectMask);

        //Assemble more informative target data for each possible target
        foreach (Collider col in possibleTargetColliders)
        {
            //check to make sure it is not already in the remembered targets List
            if (!IsAlreadyRemembered(col.transform))
            {
                TargetData targetData = new TargetData(col.transform, transform);
                possibleTargetsData.Add(targetData);
            }
        }
    }

    private void CheckVisionAll()
    {
        foreach (TargetData target in rememberedTargets)
        {
            CheckVision(target);
        }
        foreach (TargetData target in possibleTargetsData)
        {
            CheckVision(target);
        }

        //Set the vision detected state to clear, warning, or alert
        if (closestVisibleTarget != null)
        {
            if (closestVisibleTarget.distance <= visionAlertMaxDistance) { visionDetected = DetectorState.alert; }
            else if (closestVisibleTarget.distance <= visionWarningMaxDistance) { visionDetected = DetectorState.warning; }
            else { visionDetected = DetectorState.clear; }
        }
        else { visionDetected = DetectorState.clear; }
    }

    private void CheckVision(TargetData target)
    {
        //Is the target in the detector's view frustrum?
        if (target.centerOfVisionProximity >= minFrustrumDotProduct)
        {
            RaycastHit hit;

            //Are there any visual obstructions in the way?
            //Check chest first...
            if (Physics.Linecast(transform.position, target.chest.position, out hit))
            {
                if (hit.transform == target.transform){ target.canSeeChest = true; }
            }
            //...then check head
            if (Physics.Linecast(transform.position, target.head.position, out hit))
            {
                if (hit.transform == target.transform){ target.canSeeHead = true; }
            }
            //...then mark it as visible if either can be seen
            if (target.canSeeChest || target.canSeeHead)
            {
                target.isVisible = true;
                visionTargets.Add(target);
            }
        }

        //If there are any visible targets, set its vision alert level and select the closest visible target
        if (target.isVisible)
        {
            //calculate vision alert level from its three components of center of vision, distance, and cover
            target.centerOfViewFactor = 1f - (1f-target.centerOfVisionProximity)/(1f-minFrustrumDotProduct);
            target.distanceFactor = 0.5f * Mathf.Clamp01( 1f - ( (target.distance-visionAlertMaxDistance) / (visionWarningMaxDistance-visionAlertMaxDistance) ) )
                + 0.5f * Mathf.Clamp01(1f - (target.distance/visionAlertMaxDistance) );
            target.coverFactor = 0.5f * (target.canSeeChest ? 1f : 0f) + 0.5f * (target.canSeeHead ? 1f : 0f);

            target.visionAlertLevel = (visionFrustrumWeight * target.centerOfViewFactor) + (visionDistanceWeight * target.distanceFactor) + (visionCoverWeight * target.coverFactor);

            //select closest visible target
            if (closestVisibleTarget == null || closestVisibleTarget.distance > target.distance)
            {
                closestVisibleTarget = target;
            }
        }
    }

    private void CheckAudioAll()
    {
        foreach (TargetData target in rememberedTargets)
        {
            CheckAudio(target);
        }
        foreach (TargetData target in possibleTargetsData)
        {
            CheckAudio(target);
        }

        //Set the audio detected state to clear, warning, or alert
        if (loudestAudibleTarget != null)
        {
            if (loudestAudibleTarget.audioAlertLevel >= audioAlertMinLoudness) { audioDetected = DetectorState.alert; }
            else if (loudestAudibleTarget.audioAlertLevel >= audioWarningMinLoudness) { audioDetected = DetectorState.warning; }
            else { audioDetected = DetectorState.clear; }
        }
        else { audioDetected = DetectorState.clear; }
    }

    private void CheckAudio(TargetData target)
    {
        //Is the target close enough to be heard?
        if (target.distance <= maxAudioDetectRange)
        {
            if (target.sm != null)
            {
                target.movementSpeed = target.sm.speed;

                //Is the target moving enough to be heard?
                if (target.movementSpeed >= 0.01f)
                {
                    target.isAudible = true;
                    audioTargets.Add(target);
                }
            }
        }

        //If there are any audible targets, set its audio alert level and select the loudest audible target
        if (target.isAudible)
        {
            //calculate audio alert level from movement speed attenuated by distance
            target.currentAudioVolumeLevel = ( 1 - (target.distance / maxAudioDetectRange) ) * target.movementSpeed;
            target.audioAlertLevel = Mathf.Clamp01(target.currentAudioVolumeLevel/maxAudioDetectVolume);

            //select the loudest audible target
            if (loudestAudibleTarget == null || loudestAudibleTarget.audioAlertLevel < target.audioAlertLevel)
            {
                loudestAudibleTarget = target;
            }
        }
    }

    private void CheckDamage()
    {
        PruneDeadAndNullTargets(damagedMeTargets);

        mostDamageFromDirection = null;

        foreach (DamageDirection damageDirection in damageDirections)
        {
            //fade away damage recieved over time
            damageDirection.damageRecieved -= detectorResponseTime * damageFadePerSecond;
            if (damageDirection.damageRecieved < 0f) { damageDirection.damageRecieved = 0f; }

            //check which direction has recieved the most damage this tick
            if (mostDamageFromDirection == null) {
                if (damageDirection.damageRecieved > 0f) { mostDamageFromDirection = damageDirection; }
            }
            else
            {
                if (damageDirection.damageRecieved > mostDamageFromDirection.damageRecieved) { mostDamageFromDirection = damageDirection; }
            }

            //update seconds since last damage
            damageDirection.secondsSinceLastDamage += detectorResponseTime;

            //scale damage direction indicators based on bot look direction
            if (Vector3.Dot(transform.forward, damageDirection.indicator.transform.forward) >= minFrustrumDotProduct)
            {
                damageDirection.inViewFrustrum = true;
            }
            else
            {
                damageDirection.inViewFrustrum = false;
            }
        }

        mostDamageTarget = null;
        
        foreach (TargetData target in damagedMeTargets)
        {
            //update secondsSinceLastDamage for all targets in damagedMeTargets List
            target.secondsSinceLastDamage += detectorResponseTime;

            //check which target has done the most damage
            if (mostDamageTarget == null) { mostDamageTarget = target; }
            else if (target.damageAmount > mostDamageTarget.damageAmount) { mostDamageTarget = target; }
        }

        //set damage detector state based on how much damage bot has been recieving
        if (mostDamageFromDirection != null)
        {
            if (mostDamageFromDirection.damageRecieved > damageForAlert) { damageDetected = DetectorState.alert; }
            else if (mostDamageFromDirection.damageRecieved > damageForWarning) { damageDetected = DetectorState.warning; }
            else { damageDetected = DetectorState.clear; }
        }
        else { damageDetected = DetectorState.clear; }
    }

    public void RecievedDamage(float firedAmount, Vector3 firingDirection)
    {
        //which of the 12 directions is the fire most aligned to?
        float currentDotProduct = 0f;
        float largestDotProduct = 0f;
        DamageDirection alignedTo = damageDirections[0];

        foreach (DamageDirection damageDirection in damageDirections)
        {
            currentDotProduct = Vector3.Dot(damageDirection.direction, firingDirection);
            if (currentDotProduct > largestDotProduct)
            {
                largestDotProduct = currentDotProduct;
                alignedTo = damageDirection;
            }
        }

        alignedTo.damageRecieved += firedAmount;
        alignedTo.secondsSinceLastDamage = 0f;

        //after a cooldown time have the damage from this attack expire
        StartCoroutine(DamageExpired(alignedTo, firedAmount));

        //if in view frustrum, raycast to see if who fired on you is visible
        if (alignedTo.inViewFrustrum)
        {
            RaycastHit hit;
            int layer = 1 << LayerMask.NameToLayer("Default");
            if (team == SwarmBot.Team.red) { layer = layer << LayerMask.NameToLayer("BlueBot"); }
            if (team == SwarmBot.Team.blue) { layer = layer << LayerMask.NameToLayer("RedBot"); }

            if (Physics.SphereCast(transform.parent.position, 0.5f, firingDirection, out hit, maxDetectRange, layer, QueryTriggerInteraction.Ignore))
            {
                bool alreadyInTargetsList = false;

                //see if bot is already in the damagedMeTargets List
                foreach (TargetData target in damagedMeTargets)
                {
                    if (target.transform == hit.transform)
                    {
                        alreadyInTargetsList = true;
                        target.damageAmount += firedAmount;
                        target.secondsSinceLastDamage = 0f;
                    }
                }

                if (!alreadyInTargetsList)
                {
                    //if not, find bot in visionTargets and add them to the damagedMeTargets List
                    foreach (TargetData target in visionTargets)
                    {
                        //if found...
                        if (target.transform == hit.transform)
                        {
                            //flag them as having damaged you, to make them a priority target
                            target.damagedMe = true;
                            target.damageAmount += firedAmount;
                            target.secondsSinceLastDamage = 0f;
                            //add the found vision target to the damagedMeTargets List
                            damagedMeTargets.Add(target);
                        }
                    }
                }
            }
        }
    }

    private IEnumerator DamageExpired(DamageDirection damageDirection, float damageAmount)
    {
        yield return new WaitForSeconds(damageExpiresAfterSeconds);
        damageDirection.damageRecieved -= damageAmount;
        if (damageDirection.damageRecieved < 0f) { damageDirection.damageRecieved = 0f; }
    }

    private void PruneDeadAndNullTargets(List<TargetData> targetDataList)
    {
        int numberRemoved = 0;

        for (int i=0; i<(targetDataList.Count-numberRemoved); i++)
        {
            if (targetDataList[(i-numberRemoved)].transform == null)
            {
                targetDataList.RemoveAt((i - numberRemoved));
                numberRemoved++;
            }
            else if (targetDataList[(i - numberRemoved)].health == null)
            {
                targetDataList.RemoveAt((i - numberRemoved));
                numberRemoved++;
            }
            else if (targetDataList[(i - numberRemoved)].health.dead)
            {
                targetDataList.RemoveAt((i - numberRemoved));
                numberRemoved++;
            }
        }
    }

    private void RememberTargets()
    {
        PruneDeadAndNullTargets(rememberedTargets);

        //remember all targets that have damaged me for future frames of the game
        foreach (TargetData target in damagedMeTargets)
        {
            if (!IsAlreadyRemembered(target.transform)){ rememberedTargets.Add(target); }
        }

        
        //remember closest visible target for next frames of the game
        if (closestVisibleTarget != null)
        {
            if (!IsAlreadyRemembered(closestVisibleTarget.transform)) { rememberedTargets.Add(closestVisibleTarget); }
        }

        //remember loudest target for future frames of the game
        if (loudestAudibleTarget != null)
        {
            if (!IsAlreadyRemembered(loudestAudibleTarget.transform)) { rememberedTargets.Add(loudestAudibleTarget); }
        }
    }

    private bool IsAlreadyRemembered(Transform checkMe)
    {
        foreach (TargetData checkAgainst in rememberedTargets)
        {
            if (checkMe == checkAgainst.transform)
            {
                return true;
            }
        }

        return false;
    }

    private void ShowUI()
    {
        foreach(DamageDirection damageUI in damageDirections)
        {
            damageUI.rend.enabled = true;
        }

        visionIndicator.enabled = true;
        audioIndicator.enabled = true;
        damageIndicator.enabled = true;
    }

    private void HideUI()
    {
        foreach (DamageDirection damageUI in damageDirections)
        {
            damageUI.rend.enabled = false;
        }

        visionIndicator.enabled = false;
        audioIndicator.enabled = false;
        damageIndicator.enabled = false;
    }

    private void DisplayUIIndicators()
    {
        switch (visionDetected)
        {
            case DetectorState.clear:
                visionMat.color = Color.green;
                break;
            case DetectorState.warning:
                visionMat.color = Color.yellow;
                break;
            case DetectorState.alert:
                visionMat.color = Color.red;
                break;
        }
        switch (audioDetected)
        {
            case DetectorState.clear:
                audioMat.color = Color.green;
                break;
            case DetectorState.warning:
                audioMat.color = Color.yellow;
                break;
            case DetectorState.alert:
                audioMat.color = Color.red;
                break;
        }
        switch (damageDetected)
        {
            case DetectorState.clear:
                damageMat.color = Color.green;
                break;
            case DetectorState.warning:
                damageMat.color = Color.yellow;
                break;
            case DetectorState.alert:
                damageMat.color = Color.red;
                break;
        }

        foreach(DamageDirection damageDirection in damageDirections)
        {
            //scale damage direction indicators based on bot look direction
            if ( damageDirection.inViewFrustrum )
            {
                damageDirection.indicator.transform.localScale = 1.2f * Vector3.one;
            }
            else
            {
                damageDirection.indicator.transform.localScale = Vector3.one;
            }

            //light up with color from gradient based on damage dealt
            damageDirection.rend.material.color = warningColorGradient.Evaluate(damageDirection.damageRecieved / damageForAlert);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, maxDetectRange);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, maxAudioDetectRange);
    }
}
