using UnityEngine;
using UnityEditor;

namespace CombatZone.Bot
{

    [CustomEditor(typeof(BotBehavior))]
    public class FieldOfViewEditor : Editor
    {

        void OnSceneGUI()
        {

            BotBehavior fov = (BotBehavior)target;

            Handles.color = fov.fov.teamColor;

            Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.fov.viewRadius);

            Vector3 viewAngleA = fov.DirFromAngle(-fov.fov.viewAngle / 2, false);

            Vector3 viewAngleB = fov.DirFromAngle(fov.fov.viewAngle / 2, false);

            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.fov.viewRadius);

            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.fov.viewRadius);

            Handles.color = fov.fov.teamColor;

            foreach (Transform visibleTarget in fov.visibleTargets)
            {

                Handles.DrawLine(fov.transform.position, visibleTarget.position);

            }

        }

    }

}

