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
            foreach (SFXClip o in refs.SFXClips) {
                clips.Add (o.type, o.clip);
                volumes.Add (o.type, o.volume);
            }
            vfxs.Add (EVFXType.DUST, new Dust (refs.Dust));
            vfxs.Add (EVFXType.FIRE, new Fire (refs.TrailParticle, refs.Fire, refs.Smoke, refs.Trail));
            vfxs.Add (EVFXType.FLIP_RECTANGLE, new FlipRectangle (refs.FlipRectangle));
            vfxs.Add (EVFXType.GATHER_RECTANGLE, new GatherRectangle (refs.GatherParticle, refs.Ring));
            vfxs.Add (EVFXType.GREEN_LIGHT, new GreenLight (refs.GreenLight));
            vfxs.Add (EVFXType.MAGICAL_CIRCLE, new MagicalCircle (refs.MagicalCircle));
            vfxs.Add (EVFXType.UP_FLOW_RECTANGLE, new UpFlowRectangle (refs.UpFlowRectangle));

        }
        public void PlayVFX (EVFXType type, bool IsFacingRight = true, float degree = 0f, Vector2 direction = default (Vector2), float maxMagicalCircleAngle = 70f) {
            vfxs[type].PlayVFX (IsFacingRight, degree, direction, maxMagicalCircleAngle);
        }

        public void StopVFX (EVFXType type) {
            vfxs[type].StopVFX ( );
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

        [Header ("JUMP")]
        public ParticleSystem Dust = null;

        [Header ("DASH")]
        public TrailRenderer Trail = null;
        public ParticleSystem TrailParticle = null;
        public ParticleSystem Fire = null;
        public ParticleSystem Smoke = null;

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
        protected ParticleSystem mainPtc = null;
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
    class FlipRectangle : AVFXBase {
        public FlipRectangle (ParticleSystem mainParticleSystem) : base (mainParticleSystem) { }
    }
    class GatherRectangle : AVFXBase {
        ParticleSystem gatherParticle = null;
        ParticleSystem ring = null;
        public GatherRectangle (ParticleSystem gatherParticle, ParticleSystem ring) {
            this.gatherParticle = gatherParticle;
            this.ring = ring;
        }
        override public void PlayVFX (bool IsFacingRight = true, float degree = 0, Vector2 direction = default (Vector2), float maxMagicalCircleAngle = 70) {
            gatherParticle.Play ( );
            ring.Play ( );
        }
        override public void StopVFX (ParticleSystemStopBehavior stopBehavior = ParticleSystemStopBehavior.StopEmitting) {
            gatherParticle.Stop (true, stopBehavior);
            ring.Stop (true, stopBehavior);
        }
    }
    class Dust : AVFXBase {
        Transform tf = null;
        public Dust (ParticleSystem mainPtc) : base (mainPtc) {
            this.tf = mainPtc.transform;
        }
        override public void PlayVFX (bool IsFacingRight = true, float degree = 0, Vector2 direction = default (Vector2), float maxMagicalCircleAngle = 70) {
            Vector3 dustS = tf.localScale;
            dustS.x = IsFacingRight?1f: -1f;
            tf.localScale = dustS;
            mainPtc.Play ( );
        }
    }
    class UpFlowRectangle : AVFXBase {
        Transform tf = null;
        public UpFlowRectangle (ParticleSystem mainPtc) : base (mainPtc) {
            this.tf = mainPtc.transform;
        }
        override public void PlayVFX (bool IsFacingRight = true, float degree = 0, Vector2 direction = default (Vector2), float maxMagicalCircleAngle = 70) {
            if (mainPtc.isPlaying) return;
            Vector3 grabP = tf.localPosition;
            grabP.x = IsFacingRight? Mathf.Abs (grabP.x): -Mathf.Abs (grabP.x);
            tf.localPosition = grabP;
            mainPtc.Play ( );
        }
    }
    class Fire : AVFXBase {
        TrailRenderer trail = null;
        ParticleSystem trailParticle = null;
        ParticleSystem fire = null;
        ParticleSystem smoke = null;
        Transform fireTf = null;
        Transform smokeTf = null;
        ParticleSystemRenderer fireRend = null;
        ParticleSystemRenderer smokeRend = null;

        public Fire (ParticleSystem trailParticle, ParticleSystem fire, ParticleSystem smoke, TrailRenderer trail) {
            this.trailParticle = trailParticle;
            this.fire = fire;
            this.smoke = smoke;
            this.trail = trail;
            this.fireTf = fire.transform;
            this.smokeTf = smoke.transform;
            this.fireRend = fire.GetComponent<ParticleSystemRenderer> ( );
            this.smokeRend = smoke.GetComponent<ParticleSystemRenderer> ( );
        }
        override public void PlayVFX (bool IsFacingRight = true, float degree = 0, Vector2 direction = default (Vector2), float maxMagicalCircleAngle = 70) {
            // Enable trail
            trail.enabled = true;
            trailParticle.Play ( );
            // calc roation due to degree
            // also calc particle direction by set startRotaion
            // calc startRotation value by degree and transfer it into radian
            Quaternion rotation = Quaternion.Euler (0f, 0f, degree);
            ParticleSystem.MinMaxCurve startRot = new ParticleSystem.MinMaxCurve (-degree * Mathf.Deg2Rad);
            fireTf.rotation = rotation;
            smokeTf.rotation = rotation;
            ParticleSystem.MainModule smokeMain = smoke.main;
            ParticleSystem.MainModule fireMain = fire.main;
            smokeMain.startRotation = startRot;
            fireMain.startRotation = startRot;
            // and if abs degree is greater than 90f means it will need to flip in vertical so also set the flip
            if (Mathf.Abs (degree) > 90f) {
                smokeRend.flip = new Vector3 (1f, 1f, 0f);
                fireRend.flip = new Vector3 (1f, 1f, 0f);
            }
            else {
                smokeRend.flip = new Vector3 (1f, 0f, 0f);
                fireRend.flip = new Vector3 (1f, 0f, 0f);
            }
            smoke.Play ( );
            fire.Play ( );
        }
        override public void StopVFX (ParticleSystemStopBehavior stopBehavior = ParticleSystemStopBehavior.StopEmitting) {
            trail.enabled = false;
            trailParticle.Stop (true, stopBehavior);
            fire.Stop (true, stopBehavior);
            smoke.Stop (true, stopBehavior);
        }
    }
    class MagicalCircle : AVFXBase {
        public MagicalCircle (ParticleSystem mainPtc) : base (mainPtc) { }
        override public void PlayVFX (bool IsFacingRight = true, float degree = 0, Vector2 direction = default (Vector2), float maxMagicalCircleAngle = 70) {
            direction = direction.normalized;
            ParticleSystem.MainModule magicalCircleMain = mainPtc.main;
            magicalCircleMain.startRotationX = new ParticleSystem.MinMaxCurve (maxMagicalCircleAngle * direction.y * Mathf.Deg2Rad);
            magicalCircleMain.startRotationY = new ParticleSystem.MinMaxCurve (-maxMagicalCircleAngle * direction.x * Mathf.Deg2Rad);
            mainPtc.Play ( );
        }
    }
}