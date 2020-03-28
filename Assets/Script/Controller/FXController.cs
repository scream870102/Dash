namespace CJStudio.Dash {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Eccentric;
    using MapObject;
    using Player;
    using UnityEngine;
    class FXController : MonoBehaviour {
        [SerializeField] new AudioSource audio = null;
        [SerializeField] Transform fxParent = null;
        [SerializeField] VFXPool explosionFX = null;
        [SerializeField] VFXPool ringFX = null;
        [SerializeField] List<SFXClip> SFXClips = new List<SFXClip> ( );
        [SerializeField] float pitchAtAim = .5f;
        [SerializeField] float pitchTimeTransFrmoNormalToAim = .2f;
        Dictionary<ESFXType, AudioClip> clips = new Dictionary<ESFXType, AudioClip> ( );
        Dictionary<ESFXType, float> volumes = new Dictionary<ESFXType, float> ( );

        void Start ( ) {
            explosionFX.Init (fxParent);
            ringFX.Init (fxParent);
            foreach (SFXClip o in SFXClips) {
                clips.Add (o.type, o.clip);
                volumes.Add (o.type, o.volume);
            }
        }

        void OnExplosionFX (OnExplosionVFX e) {
            ParticleSystem obj = explosionFX.Ask ( );
            obj.transform.position = e.Pos;
            obj.Play ( );
            audio.PlayOneShot (clips[ESFXType.EXPLOSION], volumes[ESFXType.EXPLOSION]);
        }
        void OnRingFX (OnRingVFX e) {
            ParticleSystem obj = ringFX.Ask ( );
            obj.transform.position = e.Pos;
            obj.Play ( );
            audio.PlayOneShot (clips[ESFXType.SPRING], volumes[ESFXType.SPRING]);
        }

        void OnStageChange (OnStageChange e) {
            if (e.ActiveStage != e.PrevStage)
                audio.PlayOneShot (clips[ESFXType.FIRE_LIT], volumes[ESFXType.FIRE_LIT]);
        }

        //修改這裡轉換聲音的方式 或許是播放其他的音效 或是有淡入淡出的效果
        //純粹轉換會過於突兀
        void OnAiming (OnAiming e) {
            if (e.IsStart)
                audio.pitch = pitchAtAim;
            else
                audio.pitch = 1f;
        }

        void OnEnable ( ) {
            DomainEvents.Register<OnExplosionVFX> (OnExplosionFX);
            DomainEvents.Register<OnRingVFX> (OnRingFX);
            DomainEvents.Register<OnStageChange> (OnStageChange);
            DomainEvents.Register<OnAiming> (OnAiming);
        }
        void OnDisable ( ) {
            DomainEvents.UnRegister<OnExplosionVFX> (OnExplosionFX);
            DomainEvents.UnRegister<OnRingVFX> (OnRingFX);
            DomainEvents.UnRegister<OnStageChange> (OnStageChange);
            DomainEvents.UnRegister<OnAiming> (OnAiming);
        }

        [System.Serializable]
        class SFXClip {
            public ESFXType type;
            public AudioClip clip;
            public float volume = 1f;
        }

        [System.Serializable]
        class VFXPool {
            Queue<ParticleSystem> queue = new Queue<ParticleSystem> ( );
            List<ParticleSystem> playingObj = new List<ParticleSystem> ( );
            Transform parent = null;
            [SerializeField] int amount = 1;
            [SerializeField] bool isGrow = false;
            [SerializeField] GameObject obj = null;
            public void Init (Transform parent) {
                for (int i = 0; i < amount; i++) {
                    SpawnObj (parent);
                }
            }
            public ParticleSystem Ask ( ) {
                CheckFinish ( );
                if (queue.Count == 0 && isGrow)
                    SpawnObj (parent);
                ParticleSystem tmp = queue.Dequeue ( );
                playingObj.Add (tmp);
                return tmp;
            }
            public void Stop ( ) {
                foreach (ParticleSystem o in playingObj) {
                    o.Stop (false, ParticleSystemStopBehavior.StopEmitting);
                    queue.Enqueue (o);
                    playingObj.Remove (o);
                }
            }

            void CheckFinish ( ) {
                if (playingObj.Count == 0) return;
                for (int i = playingObj.Count - 1; i >= 0; i--) {
                    if (!playingObj[i].isPlaying) {
                        queue.Enqueue (playingObj[i]);
                        playingObj.RemoveAt (i);
                    }
                }
            }
            void SpawnObj (Transform parent = null) {
                Transform tmp = Instantiate (obj).transform;
                tmp.parent = parent;
                queue.Enqueue (tmp.GetComponent<ParticleSystem> ( ));
            }
        }
    }
    class OnExplosionVFX : IDomainEvent {
        Vector2 pos;
        public Vector2 Pos => pos;
        public OnExplosionVFX (Vector2 pos) {
            this.pos = pos;
        }
    }
    class OnRingVFX : IDomainEvent {
        Vector2 pos;
        ESpringDirection direction;
        public Vector2 Pos => pos;
        public ESpringDirection Direction => direction;
        public OnRingVFX (Vector2 pos, ESpringDirection direction) {
            this.pos = pos;
            this.direction = direction;
        }
    }
    enum ESFXType {
        EXPLOSION,
        SPRING,
        FIRE_LIT
    }
}