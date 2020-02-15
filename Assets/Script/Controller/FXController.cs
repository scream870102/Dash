namespace CJStudio.Dash {
    using System.Collections.Generic;
    using System.Collections;

    using Eccentric.Collections;
    using Eccentric;

    using MapObject;

    using UnityEngine;
    class FXController : MonoBehaviour {
        [SerializeField] Transform fxParent = null;
        [SerializeField] VFXPool explosionFX = null;
        [SerializeField] VFXPool ringFX = null;

        void Start ( ) {
            explosionFX.Init (fxParent);
            ringFX.Init (fxParent);
        }

        void OnExplosionVFX (OnExplosionVFX e) {
            ParticleSystem obj = explosionFX.Ask ( );
            obj.transform.position = e.Pos;
            obj.Play ( );
        }
        void OnRingVFX (OnRingVFX e) {
            ParticleSystem obj = ringFX.Ask ( );
            obj.transform.position = e.Pos;
            obj.Play ( );
        }

        void OnEnable ( ) {
            DomainEvents.Register<OnExplosionVFX> (OnExplosionVFX);
            DomainEvents.Register<OnRingVFX> (OnRingVFX);
        }
        void OnDisable ( ) {
            DomainEvents.UnRegister<OnExplosionVFX> (OnExplosionVFX);
            DomainEvents.UnRegister<OnRingVFX> (OnRingVFX);
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
                if (playingObj.Count == 0)return;
                for (int i = playingObj.Count - 1; i >= 0; i--) {
                    if (!playingObj [i].isPlaying) {
                        queue.Enqueue (playingObj [i]);
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
}
