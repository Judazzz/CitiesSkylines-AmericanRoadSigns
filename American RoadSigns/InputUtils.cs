using UnityEngine;

namespace AmericanRoadSigns
{
    class InputUtils
    {
        public static bool HotkeyPressed()
        {
            bool validInput = false;
            //  Preferred hotkey: [Shift] + [A]:
            if (((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyUp(KeyCode.A)) && AmericanRoadsignsTool.config.keyboardshortcut == 0)
            {
                validInput = true;
            }
            //  Preferred hotkey: [Ctrl] + [A]:
            if (((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyUp(KeyCode.A)) && AmericanRoadsignsTool.config.keyboardshortcut == 1)
            {
                validInput = true;
            }
            //  Preferred hotkey: [Alt] + [A]:
            if (((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyUp(KeyCode.A)) && AmericanRoadsignsTool.config.keyboardshortcut == 2)
            {
                validInput = true;
            }
            return validInput;
        }
    }
}