namespace Eccentric.Input {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine.InputSystem.DualShock;
    using UnityEngine.InputSystem;
    using UnityEngine;
    static class GamepadController {
        static Dictionary<EVibrateDuration, int> duration =
            new Dictionary<EVibrateDuration, int> ( ) { { EVibrateDuration.SHORT, 100 }, { EVibrateDuration.NORMAL, 200 }, { EVibrateDuration.LONG, 500 }
            };
        static Dictionary<EVibrateStrength, Strength> strength =
            new Dictionary<EVibrateStrength, Strength> ( ) { { EVibrateStrength.SLIGHT, new Strength (.25f, .25f) }, { EVibrateStrength.NORMAL, new Strength (.5f, .5f) }, { EVibrateStrength.STRONG, new Strength (.95f, .95f) }
            };
        static async public void VibrateController (int time = 100, float lowFrequency = 0.25f, float highFrequency = 0.75f) {
            if (Gamepad.current == null) return;
            Gamepad.current.SetMotorSpeeds (lowFrequency, highFrequency);
            await Task.Delay (time);
            Gamepad.current.SetMotorSpeeds (0f, 0f);
        }
        static async public void VibrateController (EVibrateDuration duration = EVibrateDuration.NORMAL, EVibrateStrength strength = EVibrateStrength.NORMAL) {
            if (Gamepad.current == null) return;
            Strength s = GamepadController.strength[strength];
            Gamepad.current.SetMotorSpeeds (s.LowFrequency, s.HighFrequency);
            await Task.Delay (GamepadController.duration[duration]);
            Gamepad.current.SetMotorSpeeds (0f, 0f);
        }

        static public void SetDS4LightColor (Color color) {
            DualShockGamepad ds4 = Gamepad.current as DualShockGamepad;
            ds4.SetLightBarColor (color);
        }
        private class Strength {
            public Strength (float low, float high) {
                this.LowFrequency = low;
                this.HighFrequency = high;
            }
            public float LowFrequency { get; private set; }
            public float HighFrequency { get; private set; }
        }
    }

    enum EVibrateStrength {
        STRONG,
        NORMAL,
        SLIGHT,
    }

    enum EVibrateDuration {
        SHORT,
        NORMAL,
        LONG,
    }

}