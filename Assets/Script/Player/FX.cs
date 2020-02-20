namespace CJStudio.Dash.Player {
    using System.Collections.Generic;

    using UnityEngine;
    [System.Serializable]
    class FX : PlayerComponent {
        FXStats stats = null;
        Dictionary<ESFXType, AudioClip> clips = new Dictionary<ESFXType, AudioClip> ( );
        Dictionary<ESFXType, float> volumes = new Dictionary<ESFXType, float> ( );
        public FX (Player player, FXStats stats) : base (player) {
            this.stats = stats;
            stats.GrabTF = stats.GrabVFX.transform;
            stats.DustTF = stats.DustVFX.transform;
            stats.DashTF = stats.DashVFX.transform;
            foreach (SFXClip o in stats.SFXClips) {
                clips.Add (o.type, o.clip);
                volumes.Add (o.type, o.volume);
            }
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
                    stats.TrailParticle.Play ( );
                    Vector3 dashS = stats.DashTF.localScale;
                    dashS.x = IsFacingRight?1f: -1f;
                    stats.DashTF.localScale = dashS;
                    stats.DashVFX.Play ( );
                    break;
                case EVFXType.CHARGE:
                    stats.ChargeParticle.Play ( );
                    break;
                case EVFXType.AURA:
                    stats.AuraParticle.Play ( );
                    break;
                case EVFXType.HEAL:
                    stats.HealVFX.Play ( );
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
                    stats.TrailParticle.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    stats.DashVFX.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.CHARGE:
                    stats.ChargeParticle.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.AURA:
                    stats.AuraParticle.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.HEAL:
                    stats.HealVFX.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
            }
        }

        public void PlaySFX (ESFXType type) {
            stats.audio.PlayOneShot (clips [type], volumes [type]);
        }

        public void StopAllSFX ( ) {
            stats.audio.Stop ( );
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
        public ParticleSystem TrailParticle = null;
        public ParticleSystem ChargeParticle = null;
        public ParticleSystem AuraParticle = null;
        public ParticleSystem DashVFX = null;
        public ParticleSystem HealVFX = null;
        public Transform DashTF { get; set; }
        public Transform GrabTF { get; set; }
        public Transform DustTF { get; set; }
        public AudioSource audio;
        public SFXClip [] SFXClips;
    }

    [System.Serializable]
    class SFXClip {
        public ESFXType type;
        public AudioClip clip;
        public float volume = 1f;
    }

    enum EVFXType {
        GRAB,
        DUST,
        TRAIL,
        CHARGE,
        AURA,
        HEAL,
    }
    enum ESFXType {
        RESET_DASH,
        HEAL,
        DASH,
        JUMP
    }
}
