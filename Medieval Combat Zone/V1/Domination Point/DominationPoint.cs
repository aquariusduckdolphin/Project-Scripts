using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CombatZone.Objective
{

    public enum CapturingTeam { Red, Blue, none }

    public class DominationPoint : MonoBehaviour
    {

        #region Variables

        #region Team Capture Data
        [Header("Team Capture Data")]
        [SerializeField] private List<GameObject> redTeamMembersInPoint = new List<GameObject>();

        [SerializeField] private List<GameObject> blueTeamMembersInPoint = new List<GameObject>();

        [SerializeField] private float redTeamCaptureProgress;

        [SerializeField] private float blueTeamCaptureProgress;
        #endregion

        #region Flag 
        [Header("Flag")]
        [SerializeField] private MeshRenderer flagRenderer;

        [SerializeField] private FillBar captureProgressBar;

        [SerializeField] private Material[] flagColors;

        [SerializeField] private ChangeDominationPointColor dominationPointColorChanger;
        #endregion

        #region Point Ownership & Contest Status
        [Header("Point Ownership & Contest Status")]
        public bool isPointContested = false;

        public bool isRedTeamPoint = false;

        public bool isBlueTeamPoint = false;
        #endregion

        #region Audio Clips
        [Header("Audio Clips")]
        [SerializeField] private AudioClip[] audioClip;
        #endregion

        #endregion

        /***************************************************************/

        #region Gather Info
        private void Awake()
        {

            dominationPointColorChanger = GetComponent<ChangeDominationPointColor>();
            captureProgressBar = GetComponentInChildren<FillBar>();
            captureProgressBar.FillTheBar(0f);
            flagRenderer.material = flagColors[0];
            CaptureTheDominationPoint(CapturingTeam.none);

        }
        #endregion

        #region Update
        private void Update()
        {

            RemoveNullBots(redTeamMembersInPoint);
            RemoveNullBots(blueTeamMembersInPoint);
            CaptureSystem();

        }
        #endregion

        #region Remove Null bots
        private void RemoveNullBots(List<GameObject> bots)
        {

            foreach (GameObject bot in bots.ToList())
            {

                if (bot == null) { bots.Remove(bot); }

            }

        }
        #endregion

        /***************************************************************/

        #region On Trigger Enter
        private void OnTriggerEnter(Collider other)
        {

            PlayerStats playerData = other.transform.root.GetComponent<PlayerStats>();

            if (playerData != null)
            {

                print(playerData.name);

                if (playerData.team == PlayerStats.Team.red)
                {

                    redTeamMembersInPoint.Add(other.gameObject);

                    if (redTeamMembersInPoint.Count == 1 && blueTeamMembersInPoint.Count == 1)
                    {
                        DominationPointAudioManager._instance.AddToAudiQueue(audioClip[0]);

                        dominationPointColorChanger.ChangeBarColorWhileCapturing(Color.yellow);
                    }
                    else if (redTeamMembersInPoint.Count == 1)
                    {
                        DominationPointAudioManager._instance.AddToAudiQueue(audioClip[2]);
                    }

                }
                else if (playerData.team == PlayerStats.Team.blue)
                {

                    blueTeamMembersInPoint.Add(other.gameObject);

                    if (redTeamMembersInPoint.Count == 1 && blueTeamMembersInPoint.Count == 1)
                    {
                        DominationPointAudioManager._instance.AddToAudiQueue(audioClip[0]);

                        dominationPointColorChanger.ChangeBarColorWhileCapturing(Color.yellow);
                    }
                    else if (blueTeamMembersInPoint.Count == 1)
                    {
                        DominationPointAudioManager._instance.AddToAudiQueue(audioClip[1]);
                    }

                }

            }

        }
        #endregion

        #region On Trigger Exit
        private void OnTriggerExit(Collider other)
        {

            PlayerStats playerData = other.gameObject.transform.root.GetComponent<PlayerStats>();

            if (playerData != null)
            {

                if (playerData.team == PlayerStats.Team.red)
                {

                    redTeamMembersInPoint.Remove(other.gameObject);

                }
                else if (playerData.team == PlayerStats.Team.blue)
                {

                    blueTeamMembersInPoint.Remove(other.gameObject);

                }

            }

        }
        #endregion

        /***************************************************************/

        #region Checking teams
        private bool AreThereRedTeam()
        {

            if (redTeamMembersInPoint.Count > 0) { return true; }
            else { return false; }

        }

        private bool AreThereBlueTeam()
        {

            if (blueTeamMembersInPoint.Count > 0) { return true; }
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
            { CaptureThePoint(CapturingTeam.Blue); }
            else if (AreThereRedTeam() && !AreThereBlueTeam())
            { CaptureThePoint(CapturingTeam.Red); }

        }
        #endregion

        #region Capture Point
        private void CaptureThePoint(CapturingTeam team)
        {

            if (redTeamCaptureProgress <= 0 && blueTeamCaptureProgress <= 0)
            {

                CaptureTheDominationPoint(CapturingTeam.none);

                flagRenderer.material = flagColors[0];

            }

            if (team == CapturingTeam.Red && blueTeamCaptureProgress <= 0)
            {

                if (redTeamCaptureProgress >= 1)
                {

                    redTeamCaptureProgress = 1;

                    CaptureTheDominationPoint(team);

                    flagRenderer.material = flagColors[1];

                    isRedTeamPoint = true;

                    return;

                }

                isPointContested = false;

                redTeamCaptureProgress += (0.05f * Time.deltaTime * redTeamMembersInPoint.Count);

                dominationPointColorChanger.ChangeBarColorWhileCapturing(Color.red);

                captureProgressBar.FillTheBar(redTeamCaptureProgress);

            }
            else if (team == CapturingTeam.Red)
            {

                isPointContested = true;

                isRedTeamPoint = false;

                blueTeamCaptureProgress -= (0.05f * Time.deltaTime * redTeamMembersInPoint.Count);

                captureProgressBar.FillTheBar(blueTeamCaptureProgress);

                dominationPointColorChanger.ChangeBarColorWhileCapturing(Color.red);

            }

            if (team == CapturingTeam.Blue && redTeamCaptureProgress <= 0)
            {

                if (blueTeamCaptureProgress >= 1)
                {

                    blueTeamCaptureProgress = 1;

                    CaptureTheDominationPoint(team);

                    flagRenderer.material = flagColors[2];

                    isBlueTeamPoint = true;

                    return;

                }

                isPointContested = false;

                blueTeamCaptureProgress += (0.05f * Time.deltaTime * blueTeamMembersInPoint.Count);

                dominationPointColorChanger.ChangeBarColorWhileCapturing(Color.blue);

                captureProgressBar.FillTheBar(blueTeamCaptureProgress);

            }
            else if (team == CapturingTeam.Blue)
            {

                isPointContested = true;

                isBlueTeamPoint = false;

                redTeamCaptureProgress -= (0.05f * Time.deltaTime * blueTeamMembersInPoint.Count);

                dominationPointColorChanger.ChangeBarColorWhileCapturing(Color.blue);

                captureProgressBar.FillTheBar(redTeamCaptureProgress);

            }

        }
        #endregion

        #region Setting colors of the domination point ring
        private void CaptureTheDominationPoint(CapturingTeam team)
        {

            if (team == CapturingTeam.Red)
            { dominationPointColorChanger.ChangeDominationColor(Color.red); }

            else if (team == CapturingTeam.Blue)
            { dominationPointColorChanger.ChangeDominationColor(Color.blue); }

            else
            { dominationPointColorChanger.ChangeDominationColor(Color.white); }

        }
        #endregion

    }

}