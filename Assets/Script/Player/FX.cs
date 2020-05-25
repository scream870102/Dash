namespace CJStudio.Dash.Player {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    [System.Serializable]
    class FX : PlayerComponent {
        FXRef refs = null;
        Dictionary<ESFXType, AudioClip> clips = new Dictionary<ESFXType, AudioClip> ( );
        Dictionary<ESFXType, float> volumes = new Dictionary<ESFXType, float> ( );
        Dictionary<EVFXType, AVFXBase> vfxs = new Dictionary<EVFXType, AVFXBase> ( );
        public FX (Player player, FXRef refs) : base (player) {
            this.refs = refs;
            refs.UpFlowRectangleTF = refs.UpFlowRectangle.transform;
            refs.DustTF = refs.Dust.transform;
            refs.FireTF = refs.Fire.transform;
            refs.FireRend = refs.Fire.GetComponent<ParticleSystemRenderer> ( );
            refs.SmokeTF = refs.Smoke.transform;
            refs.SmokeRend = refs.Smoke.GetComponent<ParticleSystemRenderer> ( );
            foreach (SFXClip o in refs.SFXClips) {
                clips.Add (o.type, o.clip);
                volumes.Add (o.type, o.volume);
            }
        }
        public void PlayVFX (EVFXType type, bool IsFacingRight = true, float degree = 0f, Vector2 direction = default (Vector2), float maxMagicalCircleAngle = 70f) {
            switch (type) {
                case EVFXType.DUST:
                    Vector3 dustS = refs.DustTF.localScale;
                    dustS.x = IsFacingRight?1f: -1f;
                    refs.DustTF.localScale = dustS;
                    refs.Dust.Play ( );
                    break;
                case EVFXType.UP_FLOW_RECTANGLE:
                    if (refs.UpFlowRectangle.isPlaying) return;
                    Vector3 grabP = refs.UpFlowRectangleTF.localPosition;
                    grabP.x = IsFacingRight? Mathf.Abs (grabP.x): -Mathf.Abs (grabP.x);
                    refs.UpFlowRectangleTF.localPosition = grabP;
                    refs.UpFlowRectangle.Play ( );
                    break;
                case EVFXType.FIRE:
                    // Enable trail
                    refs.Trail.enabled = true;
                    refs.TrailParticle.Play ( );
                    // calc roation due to degree
                    // also calc particle direction by set startRotaion
                    // calc startRotation value by degree and transfer it into radian
                    Quaternion rotation = Quaternion.Euler (0f, 0f, degree);
                    ParticleSystem.MinMaxCurve startRot = new ParticleSystem.MinMaxCurve (-degree * Mathf.Deg2Rad);
                    refs.FireTF.rotation = rotation;
                    refs.SmokeTF.rotation = rotation;
                    ParticleSystem.MainModule smokeMain = refs.Smoke.main;
                    ParticleSystem.MainModule fireMain = refs.Fire.main;
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
                    refs.Smoke.Play ( );
                    refs.Fire.Play ( );
                    break;
                case EVFXType.MAGICAL_CIRCLE:
                    direction = direction.normalized;
                    ParticleSystem.MainModule magicalCircleMain = refs.MagicalCircle.main;
                    magicalCircleMain.startRotationX = new ParticleSystem.MinMaxCurve (maxMagicalCircleAngle * direction.y * Mathf.Deg2Rad);
                    magicalCircleMain.startRotationY = new ParticleSystem.MinMaxCurve (-maxMagicalCircleAngle * direction.x * Mathf.Deg2Rad);
                    refs.Fire.Play ( );
                    break;
                case EVFXType.GATHER_RECTANGLE:
                    refs.GatherParticle.Play ( );
                    refs.Ring.Play ( );
                    break;
                case EVFXType.FLIP_RECTANGLE:
                    refs.FlipRectangle.Play ( );
                    break;
                case EVFXType.GREEN_LIGHT:
                    refs.GreenLight.Play ( );
                    break;
            }
        }

        public void StopVFX (EVFXType type, bool IsFacingRight = true) {
            switch (type) {
                case EVFXType.DUST:
                    refs.Dust.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.UP_FLOW_RECTANGLE:
                    refs.UpFlowRectangle.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.FIRE:
                    refs.Trail.enabled = false;
                    refs.TrailParticle.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    refs.Fire.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    refs.Smoke.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.GATHER_RECTANGLE:
                    refs.GatherParticle.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    refs.Ring.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.FLIP_RECTANGLE:
                    refs.FlipRectangle.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.GREEN_LIGHT:
                    refs.GreenLight.Stop (true, ParticleSystemStopBehavior.StopEmitting);
                    break;
                case EVFXType.MAGICAL_CIRCLE:
                    refs.MagicalCircle.Stop (true, ParticleSystemStopBehavior.StopEmitting);
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
        public List<VFXObject> VFXAction = new List<VFXObject> ( );
        [Header ("GRAB")]
        public ParticleSystem UpFlowRectangle = null;
        public Transform UpFlowRectangleTF { get; set; }

        [Header ("JUMP")]
        public ParticleSystem Dust = null;
        public Transform DustTF { get; set; }

        [Header ("DASH_NORMAL")]
        public TrailRenderer Trail = null;
        public ParticleSystem TrailParticle = null;
        public ParticleSystem Fire = null;
        public ParticleSystemRenderer FireRend { get; set; }
        public ParticleSystem Smoke = null;
        public ParticleSystemRenderer SmokeRend { get; set; }
        public Transform FireTF { get; set; }
        public Transform SmokeTF { get; set; }

        [Header ("CHARGE")]
        public ParticleSystem GatherParticle = null;
        public ParticleSystem Ring = null;
        [Header ("DASH_READY")]
        public ParticleSystem FlipRectangle = null;
        [Header ("HEAL")]
        public ParticleSystem GreenLight = null;
        [Header ("DASH_OTAKU")]
        public ParticleSystem MagicalCircle = null;
        [Header ("OTHER")]
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

    [System.Serializable]
    enum EVFXType {
        UP_FLOW_RECTANGLE,
        DUST,
        FIRE,
        GATHER_RECTANGLE,
        FLIP_RECTANGLE,
        GREEN_LIGHT,
        MAGICAL_CIRCLE,
    }
    enum ESFXType {
        RESET_DASH,
        HEAL,
        DASH,
        JUMP,
        CHARGE
    }

    [System.Serializable]
    enum EVFXAction {
        CHARGE,
        DASH,
        DASH_READY,
        GRAB,
        JUMP,
        HEAL
    }

    [CreateAssetMenu (fileName = "New VFX", menuName = "FX/VFX")]
    class VFXObject : ScriptableObject {
        public EVFXAction action;
        public EVFXType type;

    }
    class AVFXBase {
        ParticleSystem mainPtc = null;
        public AVFXBase (ParticleSystem mainParticleSystem = null) {
            this.mainPtc = mainParticleSystem;
        }
        virtual public void StopVFX (ParticleSystemStopBehavior stopBehavior = ParticleSystemStopBehavior.StopEmitting) {
            mainPtc.Stop (true, stopBehavior);
        }
        virtual public void PlayVFX (bool IsFacingRight = true, float degree = 0f, Vector2 direction = default (Vector2), float maxMagicalCircleAngle = 70f) {
            mainPtc.Play ( );
        }
    }
    class GreenLight : AVFXBase {
        public GreenLight (ParticleSystem mainParticleSystem) : base (mainParticleSystem) { }
    }
}