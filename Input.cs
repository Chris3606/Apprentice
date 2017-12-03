using RLNET;
using System;

namespace Apprentice
{
    // NONE MUST be last.
    enum InputAction { UP, UP_RIGHT, RIGHT, DOWN_RIGHT, DOWN, DOWN_LEFT, LEFT, UP_LEFT, ACTIVATE, DOWN_STAIR, SPELLS_SCREEN,
                          MESSAGE_RECALL_SCREEN, BACK, QUIT, FULLSCREEN, NONE };

    // Game's interpretation of keys.  There is an extension method defined for RLKeyPress that converts it to this, so we can control
    // how key-presses are interpreted exactly.
    class KeyPress
    {
        public RLKey Key { get; private set; }
        public bool Shift { get; private set; }
        public bool Control { get; private set; }
        public bool Alt { get; private set; }

        public KeyPress(RLKey key, bool shift = false, bool control = false, bool alt = false)
        {
            Key = key;
            Shift = shift;
            Control = control;
            Alt = alt;
        }

        public static bool operator==(KeyPress obj1, KeyPress obj2)
            => obj1.Key == obj2.Key && obj1.Shift == obj2.Shift && obj1.Control == obj2.Control && obj1.Alt == obj2.Alt;
        
        public static bool operator!=(KeyPress obj1, KeyPress obj2) => !(obj1 == obj2);

        public override bool Equals(object obj)
        {
            if (obj is KeyPress)
            {
                var keyPress = (KeyPress)obj;
                return keyPress == this;
            }

            return false;
        }

        public override int GetHashCode() => base.GetHashCode();
    }

    static class Input
    {
        // One per keybind, minus NONE action
        private static KeyPress[] keyBindings = new KeyPress[Enum.GetNames(typeof(InputAction)).Length - 1];

        // TODO: For now this just sets the default key-bindings.  Really this should be read-in from a file of sorts (settings/Key-bindings)
        static Input()
        {
            keyBindings[(int)InputAction.UP] = new KeyPress(RLKey.Keypad8);
            keyBindings[(int)InputAction.UP_RIGHT] = new KeyPress(RLKey.Keypad9);
            keyBindings[(int)InputAction.RIGHT] = new KeyPress(RLKey.Keypad6);
            keyBindings[(int)InputAction.DOWN_RIGHT] = new KeyPress(RLKey.Keypad3);
            keyBindings[(int)InputAction.DOWN] = new KeyPress(RLKey.Keypad2);
            keyBindings[(int)InputAction.DOWN_LEFT] = new KeyPress(RLKey.Keypad1);
            keyBindings[(int)InputAction.LEFT] = new KeyPress(RLKey.Keypad4);
            keyBindings[(int)InputAction.UP_LEFT] = new KeyPress(RLKey.Keypad7);

            keyBindings[(int)InputAction.ACTIVATE] = new KeyPress(RLKey.Comma);
            keyBindings[(int)InputAction.DOWN_STAIR] = new KeyPress(RLKey.Period, true); // > key

            keyBindings[(int)InputAction.SPELLS_SCREEN] = new KeyPress(RLKey.P, true); // Shift-P
            keyBindings[(int)InputAction.MESSAGE_RECALL_SCREEN] = new KeyPress(RLKey.P, false, true); // Ctrl-P

            keyBindings[(int)InputAction.FULLSCREEN] = new KeyPress(RLKey.F11);

            keyBindings[(int)InputAction.BACK] = new KeyPress(RLKey.Escape);
            keyBindings[(int)InputAction.QUIT] = new KeyPress(RLKey.Q);
        }

        public static InputAction ActionFor(RLKeyPress rlKeyPress)
        {
            var keyPress = rlKeyPress.AsKeyPress();

            int i = 0; // Start guaranteed to be 0 so array accesses line up
            foreach (var keyBind in keyBindings)
            {
                // If a keybind is unused, just say null
                if (keyPress == keyBind)
                    return (InputAction)i;
                i++;
            }

            return InputAction.NONE;
        }

        public static KeyPress KeybindFor(InputAction action)
        {
            if (action == InputAction.NONE)
                return null;

            return keyBindings[(int)action];
        }

        // Extension method for explicit conversion.
        // TODO: For now this assumes caps lock on is not equal to shift.  Can easily be changed later.
        public static KeyPress AsKeyPress(this RLKeyPress key) => new KeyPress(key.Key, key.Shift, key.Control, key.Alt);
    }
}
