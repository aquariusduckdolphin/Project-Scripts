using UnityEngine;

namespace CombatZone.Utilities
{

    public static class CursorLockState
    {

        #region Cursor Lock State
        public static void StateLocked()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        #endregion

        #region Cursor Unlock State
        public static void StateUnlocked()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        #endregion

    }

}