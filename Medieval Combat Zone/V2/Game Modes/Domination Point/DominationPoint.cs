using CombatZone.Objective;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CombatZone.Character;

namespace CombatZone.Manager
{

    public class DominationPoint : MonoBehaviour
    {

        /**************** Variables ****************/

        #region Team Capture Data
        [Header("Team Capture Data")]
        [SerializeField] private List<GameObject> redTeamPlayersInZone = new List<GameObject>();
        [SerializeField] private float redTeamCaptureProgress;

        [SerializeField] private List<GameObject> blueTeamPlayersInZone = new List<GameObject>();
        [SerializeField] private float blueTeamCaptureProgress;
        #endregion

        #region Flag 
        [Header("Flag")]
        [SerializeField] private MeshRenderer flagRenderer;
        [SerializeField] private Material[] flagColors;

        private DominationPointColor dominationPointColor;
        #endregion

        #region Point Ownership & Contest Status
        [Header("Point Ownership & Contest Status")]
        public bool contested = false;
        public bool redTeam = false;
        public bool blueTeam = false;

        public bool isPointContested { get => contested; private set => contested = value; }

        public bool isRedTeamPoint { get => redTeam; private set => redTeam = value; }

        public bool isBlueTeamPoint { get => blueTeam; private set => blueTeam = value; }
        #endregion

        #region Audio Clips
        [Header("Audio Clips")]
        [SerializeField] private AudioClip[] audioClip;
        #endregion

        /**************** Start, Update, Etc. ****************/

        #region Gather Info
        private void Awake()
        {
            dominationPointColor = GetComponent<DominationPointColor>();
            dominationPointColor.UpdateProgressBar(0f);
            flagRenderer.material = flagColors[0];
            CaptureTheDominationPoint(Team.none);
        }
        #endregion

        #region Update
        private void Update()
        {
            RemoveNullTeamPlayers(redTeamPlayersInZone);
            RemoveNullTeamPlayers(blueTeamPlayersInZone);
            CaptureSystem();
        }
        #endregion

        #region On Trigger Enter
        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerData = other.transform.root.GetComponent<PlayerStats>();

            if (playerData == null) { return; }

            if (playerData.team == Team.red)
            {
                redTeamPlayersInZone.Add(other.gameObject);
            }
            else if (playerData.team == Team.blue)
            {
                blueTeamPlayersInZone.Add(other.gameObject);
            }

            AudioToPlay();
        }
        #endregion

        #region On Trigger Exit
        private void OnTriggerExit(Collider other)
        {
            PlayerStats playerData = other.gameObject.transform.root.GetComponent<PlayerStats>();

            if (playerData == null) { return; }

            if (playerData.team == Team.red)
            {
                redTeamPlayersInZone.Remove(other.gameObject);
            }
            else if (playerData.team == Team.blue)
            {
                blueTeamPlayersInZone.Remove(other.gameObject);
            }
        }
        #endregion

        /**************** Audio & Clean up ****************/

        #region Remove Null bots
        private void RemoveNullTeamPlayers(List<GameObject> bots)
        {
            foreach (GameObject bot in bots.ToList())
            {
                if (bot == null) { bots.Remove(bot); }
            }
        }
        #endregion

        #region Audio To Play
        private void AudioToPlay()
        {
            if (redTeamPlayersInZone.Count >= 1 && blueTeamPlayersInZone.Count >= 1)
            {
                AudioManager._instance.AddToAudiQueue(audioClip[0]);
                dominationPointColor.UpdateBarColor(Color.yellow);
            }
            else if (blueTeamPlayersInZone.Count == 1)
            {
                AudioManager._instance.AddToAudiQueue(audioClip[1]);
            }
            else if (redTeamPlayersInZone.Count == 1)
            {
                AudioManager._instance.AddToAudiQueue(audioClip[2]);
            }
        }
        #endregion

        /**************** System ****************/

        #region Checking teams
        private bool AreThereRedTeam()
        {
            if (redTeamPlayersInZone.Count > 0) { return true; }
            else { return false; }
        }

        private bool AreThereBlueTeam()
        {
            if (blueTeamPlayersInZone.Count > 0) { return true; }
            else { return false; }
        }
        #endregion

        #region Capture System
        private void CaptureSystem()
        {
            if (AreThereRedTeam() && AreThereBlueTeam())
            {
                isPointContested = true;
                return;
            }

            if (!AreThereRedTeam() && AreThereBlueTeam())
            { CaptureThePoint(Team.blue); 
            }
            else if (AreThereRedTeam() && !AreThereBlueTeam())
            { CaptureThePoint(Team.red); 
            }
        }
        #endregion

        /**************** Capture Point Logic ****************/

        #region Capture Point
        private void CaptureThePoint(Team team)
        {
            if (team == Team.red && blueTeamCaptureProgress <= 0)
            {
                if (redTeamCaptureProgress >= 1)
                {
                    redTeamCaptureProgress = 1;
                    CaptureTheDominationPoint(team);
                    return;
                }
                TeamCapturing(redTeamPlayersInZone, ref redTeamCaptureProgress, Color.red);
            }
            else if (team == Team.red)
            {
                OpposingTeamCapturing(redTeamPlayersInZone, ref blueTeamCaptureProgress, Color.blue);
            }

            if (team == Team.blue && redTeamCaptureProgress <= 0)
            {
                if (blueTeamCaptureProgress >= 1)
                {
                    blueTeamCaptureProgress = 1;
                    CaptureTheDominationPoint(team);
                    return;
                }

                TeamCapturing(blueTeamPlayersInZone, ref blueTeamCaptureProgress, Color.blue);
            }
            else if (team == Team.blue)
            {
                OpposingTeamCapturing(blueTeamPlayersInZone, ref redTeamCaptureProgress, Color.red);
            }

        }
        #endregion

        #region Team Caupturing Point
        private void TeamCapturing(List<GameObject> team, ref float teamCaptureProgress, Color color)
        {
            isPointContested = false;
            teamCaptureProgress += (0.05f * Time.deltaTime * team.Count);
            teamCaptureProgress = Mathf.Clamp(teamCaptureProgress, 0f, 1f);
            dominationPointColor.UpdateProgressBar(teamCaptureProgress);
            dominationPointColor.UpdateBarColor(color);
        }
        #endregion

        #region Opposing Team Capturing Point
        private void OpposingTeamCapturing(List<GameObject> team, ref float teamCaptureProgress, Color color)
        {
            isPointContested = true;
            teamCaptureProgress -= (0.05f * Time.deltaTime * team.Count);
            teamCaptureProgress = Mathf.Clamp(teamCaptureProgress, 0f, 1f);
            dominationPointColor.UpdateProgressBar(teamCaptureProgress);
            dominationPointColor.UpdateBarColor(color);
        }
        #endregion

        #region Setting colors of the domination point ring
        private void CaptureTheDominationPoint(Team team)
        {
            if (team == Team.red)
            {
                isRedTeamPoint = true;
                isBlueTeamPoint = false;
                dominationPointColor.SetControlPointColor(Color.red);
                flagRenderer.material = flagColors[1];
            }
            else if (team == Team.blue)
            {
                isBlueTeamPoint = true;
                isRedTeamPoint = false;
                dominationPointColor.SetControlPointColor(Color.blue);
                flagRenderer.material = flagColors[2];
            }
            else
            { 
                dominationPointColor.SetControlPointColor(Color.white);
            }
        }
        #endregion

    }

}