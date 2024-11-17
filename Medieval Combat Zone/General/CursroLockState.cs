using UnityEngine;

namespace CombatZone.CursorState
{

    public class CursroLockState : MonoBehaviour
    {

        #region Cursor Lock State
        public void CursorLockState()
        {

            Cursor.lockState = CursorLockMode.Locked;

            Cursor.visible = false;

        }
        #endregion

        #region Cursor Unlock State
        public void CursorUnlockState()
        {

            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;

        }
        #endregion

    }

}