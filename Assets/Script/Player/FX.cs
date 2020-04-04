namespace CJStudio.Dash.Player {
    using System.Collections.Generic;
    using System.Threading.Tasks;
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
            refs.FireTF = refs.FireVFX.transform;
            refs.FireRend = refs.FireVFX.GetComponent<ParticleSystemRenderer> ( );
            refs.SmokeTF = refs.SmokeVFX.transform;
            refs.SmokeRend = refs.SmokeVFX.GetComponent<ParticleSystemRenderer> ( );
            foreach (SFXClip o in refs.SFXClips) {
                clips.Add (o.type, o.clip);
                volumes.Add (o.type, o.volume);
            }
        }
        public void PlayVFX (EVFXType type, bool IsFacingRight = true, float degree = 0f) {
            switch (type) {
                case EVFXType.DUST:
                    Vector3 dustS = refs.DustTF.localScale;
                    dustS.x = IsFacingRight?1f: -1f;
                    refs.DustTF.localScale = dustS;
                    refs.DustVFX.Play ( );
                    break;
                case EVFXType.GRAB:
                    if (refs.GrabVFX.isPlaying) return;
                    Vector3 grabP = refs.GrabTF.localPosition;
                    grabP.x = IsFacingRight? Mathf.Abs (grabP.x): -Mathf.Abs (grabP.x);
                    refs.GrabTF.localPosition = grabP;
                    refs.GrabVFX.Play ( );
                    break;
                case EVFXType.TRAIL:
                    // Enable trail
                    refs.TrailVFX.enabled = true;
                    refs.TrailParticle.Play ( );
                    // calc scale due to direction and enable dash particle
                    Vector3 scale = refs.DashTF.localScale;
                    scale.x = IsFacingRight?1f: -1f;
                    refs.DashTF.localScale = scale;
                    refs.DashVFX.Play ( );
                    // calc roation due to degree
                    // also calc particle direction by set startRotaion
                    // calc startRotation value by degree and transfer it into radian
                    Quaternion rotation = Quaternion.Euler (0f, 0f, degree);
                    ParticleSystem.MinMaxCurve startRot = new ParticleSystem.MinMaxCurve (-degree * Mathf.Deg2Rad);
                    refs.FireTF.rotation = rotation;
                    refs.SmokeTF.rotation = rotation;
                    ParticleSystem.MainModule smokeMain = refs.SmokeVFX.main;
                    ParticleSystem.MainModule fireMain = refs.FireVFX.main;
                    smokeMain.startRotation = startRot;
                    fireMain.startRotation = startRot;
                    // and if abs degree is greater than 90f means it will need to flip in vertical so also set the flip
                    if (Mathf.Abs (degree) > 90f) {
                        refs.SmokeRend.flip = new Vector3 (1f, 1f, 0f);
                        refs.FireRend.flip = new Vector3 (1f, 1f, 0f);
                    }
                    else {
                        refs.SmokeRend.flip = new Vector3 (1f, 0f, 0f);
                        refs.FireRend.flip = new Vector3 (1f, 0f, 0f);
                    }
                    refs.SmokeVFX.Play ( );
                    refs.FireVFX.Play ( );
                    break;
                case EVFXType.CHARGE:
                    refs.ChargeParticle.Play ( );
                    refs.RingVFX.Play ( );
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
                    refs.FireVFX.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    refs.SmokeVFX.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.CHARGE:
                    refs.ChargeParticle.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    refs.RingVFX.Stop (true, ParticleSystemStopBehavior.StopEmitting);
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
            refs.audio.PlayOneShot (clips[type], volumes[type]);
        }

        public void StopAllSFX ( ) {
            refs.audio.Stop ( );
        }

        public void PlayLoopSFX (ESFXType type) {
            refs.loopAudio.clip = clips[type];
            refs.loopAudio.volume = volumes[type];
            refs.loopAudio.Play ( );
        }

        public void StopLoopSFX ( ) {
            refs.loopAudio.Stop ( );
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
        public ParticleSystem RingVFX = null;
        public ParticleSystem FireVFX = null;
        public ParticleSystem SmokeVFX = null;
        public Transform DashTF { get; set; }
        public Transform GrabTF { get; set; }
        public Transform DustTF { get; set; }
        public Transform FireTF { get; set; }
        public Transform SmokeTF { get; set; }
        public ParticleSystemRenderer SmokeRend { get; set; }
        public ParticleSystemRenderer FireRend { get; set; }
        public AudioSource audio;
        public AudioSource loopAudio;
        public SFXClip[ ] SFXClips;
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
        JUMP,
        CHARGE
    }
}