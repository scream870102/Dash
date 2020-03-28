namespace CJStudio.Dash.Player {
    using System.Collections.Generic;

    using UnityEngine;
    [System.Serializable]
    class FX : PlayerComponent {
        FXRef refs = null;
        Dictionary<ESFXType, AudioClip> clips = new Dictionary<ESFXType, AudioClip> ( );
        Dictionary<ESFXType, float> volumes = new Dictionary<ESFXType, float> ( );
        public FX (Player player, FXRef refs) : base (player) {
            this.refs = refs;
            refs.GrabTF = refs.GrabVFX.transform;
            refs.DustTF = refs.DustVFX.transform;
            refs.DashTF = refs.DashVFX.transform;
            foreach (SFXClip o in refs.SFXClips) {
                clips.Add (o.type, o.clip);
                volumes.Add (o.type, o.volume);
            }
        }
        public void PlayVFX (EVFXType type, bool IsFacingRight = true) {
            switch (type) {
                case EVFXType.DUST:
                    Vector3 dustS = refs.DustTF.localScale;
                    dustS.x = IsFacingRight?1f: -1f;
                    refs.DustTF.localScale = dustS;
                    refs.DustVFX.Play ( );
                    break;
                case EVFXType.GRAB:
                    if (refs.GrabVFX.isPlaying)return;
                    Vector3 grabP = refs.GrabTF.localPosition;
                    grabP.x = IsFacingRight? Mathf.Abs (grabP.x): -Mathf.Abs (grabP.x);
                    refs.GrabTF.localPosition = grabP;
                    refs.GrabVFX.Play ( );
                    break;
                case EVFXType.TRAIL:
                    refs.TrailVFX.enabled = true;
                    refs.TrailParticle.Play ( );
                    Vector3 dashS = refs.DashTF.localScale;
                    dashS.x = IsFacingRight?1f: -1f;
                    refs.DashTF.localScale = dashS;
                    refs.DashVFX.Play ( );
                    break;
                case EVFXType.CHARGE:
                    refs.ChargeParticle.Play ( );
                    break;
                case EVFXType.AURA:
                    refs.AuraParticle.Play ( );
                    break;
                case EVFXType.HEAL:
                    refs.HealVFX.Play ( );
                    break;
            }
        }

        public void StopVFX (EVFXType type, bool IsFacingRight = true) {
            switch (type) {
                case EVFXType.DUST:
                    refs.DustVFX.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.GRAB:
                    refs.GrabVFX.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.TRAIL:
                    refs.TrailVFX.enabled = false;
                    refs.TrailParticle.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    refs.DashVFX.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.CHARGE:
                    refs.ChargeParticle.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.AURA:
                    refs.AuraParticle.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.HEAL:
                    refs.HealVFX.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
            }
        }

        public void PlaySFX (ESFXType type) {
            refs.audio.PlayOneShot (clips [type], volumes [type]);
        }

        public void StopAllSFX ( ) {
            refs.audio.Stop ( );
        }

        override public void Tick ( ) { }
        override public void FixedTick ( ) { }
        override public void OnEnable ( ) { }
        override public void OnDisable ( ) { }
        override public void SetSaveData (SaveData data) { }
    }

    [System.Serializable]
    class FXRef : PlayerAttr {
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
