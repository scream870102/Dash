namespace CJStudio.Dash.Player {
    using UnityEngine;

    class AVFXBase {
        protected ParticleSystem mainPtc = null;
        protected GameObject root = null;
        virtual public void Init (GameObject root) {
            this.root = root;
            this.mainPtc = root.GetComponent<ParticleSystem> ( );
        }
        virtual public void StopVFX (ParticleSystemStopBehavior stopBehavior = ParticleSystemStopBehavior.StopEmitting) {
            mainPtc.Stop (true, stopBehavior);
        }
        virtual public void PlayVFX (bool IsFacingRight = true, float degree = 0f, Vector2 direction = default (Vector2)) {
            mainPtc.Play ( );
        }
    }
    class GreenLight : AVFXBase { }
    class FlipRectangle : AVFXBase { }
    class GatherRectangle : AVFXBase {
        ParticleSystem gatherParticle = null;
        ParticleSystem ring = null;
        override public void Init (GameObject root) {
            base.Init (root);
            gatherParticle = root.GetComponent<ParticleSystem> ( );
            ring = root.GetComponentInChildren<ParticleSystem> ( );
        }
        override public void PlayVFX (bool IsFacingRight = true, float degree = 0, Vector2 direction = default (Vector2)) {
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
        override public void Init (GameObject root) {
            base.Init (root);
            tf = root.transform;
        }
        override public void PlayVFX (bool IsFacingRight = true, float degree = 0, Vector2 direction = default (Vector2)) {
            Vector3 dustS = tf.localScale;
            dustS.x = IsFacingRight?1f: -1f;
            tf.localScale = dustS;
            mainPtc.Play ( );
        }
    }
    class UpFlowRectangle : AVFXBase {
        Transform tf = null;
        override public void Init (GameObject root) {
            base.Init (root);
            tf = root.transform;
        }
        override public void PlayVFX (bool IsFacingRight = true, float degree = 0, Vector2 direction = default (Vector2)) {
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
        override public void Init (GameObject root) {
            base.Init (root);
            fire = root.GetComponent<ParticleSystem> ( );
            trail = root.GetComponentInChildren<TrailRenderer> ( );
            trailParticle = trail.gameObject.GetComponentInChildren<ParticleSystem> ( );
            smoke = root.transform.Find ("Smoke").GetComponent<ParticleSystem> ( );
            fireTf = fire.transform;
            smokeTf = smoke.transform;
            fireRend = fire.GetComponent<ParticleSystemRenderer> ( );
            smokeRend = smoke.GetComponent<ParticleSystemRenderer> ( );
        }
        override public void PlayVFX (bool IsFacingRight = true, float degree = 0, Vector2 direction = default (Vector2)) {
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
        float maxMagicalCircleAngle = 70f;
        public MagicalCircle ( ) { }
        override public void PlayVFX (bool IsFacingRight = true, float degree = 0, Vector2 direction = default (Vector2)) {
            direction = direction.normalized;
            ParticleSystem.MainModule magicalCircleMain = mainPtc.main;
            magicalCircleMain.startRotationX = new ParticleSystem.MinMaxCurve (maxMagicalCircleAngle * direction.y * Mathf.Deg2Rad);
            magicalCircleMain.startRotationY = new ParticleSystem.MinMaxCurve (-maxMagicalCircleAngle * direction.x * Mathf.Deg2Rad);
            mainPtc.Play ( );
        }
    }
}