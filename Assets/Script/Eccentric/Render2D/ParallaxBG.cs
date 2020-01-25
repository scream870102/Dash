using System.Collections;
using System.Collections.Generic;

using UnityEngine;
namespace Eccentric.Render2D {
    /// <summary>This is a parallax background</summary>
    /// <remarks>set bg object postion.x due to its z position</remarks>
    /// <remarks>you can have lots of bg object at the same time</remarks>
    /// <remarks>ref:https://www.youtube.com/watch?v=5E5_Fquw7BM</remarks>
    public class ParallaxBG : MonoBehaviour {
        [SerializeField] Transform [] bgS = null;
        //How smooth the parallax is going to be, Make sure the value is grater than zero
        [Range (0f, 1000f)]
        [SerializeField] float factor = 1f;
        [SerializeField] new Transform camera = null;
        Vector3 prevCamPos = Vector3.zero;
        float [] parallaxScales = null;

        // Start is called before the first frame update
        void Start ( ) {
            prevCamPos = camera.position;
            parallaxScales = new float [bgS.Length];
            for (int i = 0; i < bgS.Length; i++)
                parallaxScales [i] = bgS [i].position.z * -1f;
        }

        // Update is called once per frame
        void Update ( ) {
            for (int i = 0; i < bgS.Length; i++) {
                float parallax = (prevCamPos.x - camera.position.x) * parallaxScales [i];
                Vector3 bgTargetPos = new Vector3 (bgS [i].position.x + parallax, bgS [i].position.y, bgS [i].position.z);
                bgS [i].position = Vector3.Lerp (bgS [i].position, bgTargetPos, factor * Time.deltaTime);
            }
            prevCamPos = camera.position;
        }
    }
}
