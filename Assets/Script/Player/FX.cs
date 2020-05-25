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
        public Dictionary<EVFXType, AVFXBase> VFXS => vfxs;
        public FX (Player player, FXRef refs) : base (player) {
            this.refs = refs;
            foreach (SFXClip o in refs.SFXClips) {
                clips.Add (o.type, o.clip);
                volumes.Add (o.type, o.volume);
            }
            vfxs.Add (EVFXType.DUST, new Dust ( ));
            vfxs.Add (EVFXType.FIRE, new Fire ( ));
            vfxs.Add (EVFXType.FLIP_RECTANGLE, new FlipRectangle ( ));
            vfxs.Add (EVFXType.GATHER_RECTANGLE, new GatherRectangle ( ));
            vfxs.Add (EVFXType.GREEN_LIGHT, new GreenLight ( ));
            vfxs.Add (EVFXType.MAGICAL_CIRCLE, new MagicalCircle ( ));
            vfxs.Add (EVFXType.UP_FLOW_RECTANGLE, new UpFlowRectangle ( ));
        }

        public void PlayVFX (EVFXType type, bool IsFacingRight = true, float degree = 0f, Vector2 direction = default (Vector2)) {
            vfxs[type].PlayVFX (IsFacingRight, degree, direction);
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
}