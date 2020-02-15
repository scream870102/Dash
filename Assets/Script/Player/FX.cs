namespace CJStudio.Dash.Player {
    using UnityEngine;
    [System.Serializable]
    class FX : PlayerComponent {
        FXStats stats = null;
        public FX (Player player, FXStats stats) : base (player) {
            this.stats = stats;
            stats.GrabTF = stats.GrabVFX.transform;
            stats.DustTF = stats.DustVFX.transform;
        }
        public void PlayVFX (EVFXType type, bool IsFacingRight = true) {
            switch (type) {
                case EVFXType.DUST:
                    Vector3 dustS = stats.DustTF.localScale;
                    dustS.x = IsFacingRight?1f: -1f;
                    stats.DustTF.localScale = dustS;
                    stats.DustVFX.Play ( );
                    break;
                case EVFXType.GRAB:
                    if (stats.GrabVFX.isPlaying)return;
                    Vector3 grabP = stats.GrabTF.localPosition;
                    grabP.x = IsFacingRight? Mathf.Abs (grabP.x): -Mathf.Abs (grabP.x);
                    stats.GrabTF.localPosition = grabP;
                    stats.GrabVFX.Play ( );
                    break;
                case EVFXType.TRAIL:
                    stats.TrailVFX.enabled = true;
                    break;
            }
        }

        public void StopVFX (EVFXType type, bool IsFacingRight = true) {
            switch (type) {
                case EVFXType.DUST:
                    stats.DustVFX.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.GRAB:
                    stats.GrabVFX.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.TRAIL:
                    stats.TrailVFX.enabled = false;
                    break;
            }
        }
        override public void Tick ( ) { }
        override public void FixedTick ( ) { }
        override public void OnEnable ( ) { }
        override public void OnDisable ( ) { }
        override public void SetSaveData (SaveData data) { }
    }

    [System.Serializable]
    class FXStats : PlayerStats {
        public ParticleSystem GrabVFX = null;
        public ParticleSystem DustVFX = null;
        public TrailRenderer TrailVFX = null;
        public Transform GrabTF { get; set; }
        public Transform DustTF { get; set; }
    }
    enum EVFXType {
        GRAB,
        DUST,
        TRAIL,
    }
}
