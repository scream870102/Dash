using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
public class TestScript : MonoBehaviour {
    PlayerControl pc = null;
    private void Awake ( ) {
        pc = new PlayerControl ( );
        // pc.GamePlay.Shoot.started += ctx => Debug.Log ("Shoot Started");
        // pc.GamePlay.Shoot.canceled += ctx => Debug.Log ("Shoot Canceld");
        // pc.GamePlay.Shoot.performed += OnPressed;
    }
    // Start is called before the first frame update
    void Start ( ) {

    }

    // Update is called once per frame
    void Update ( ) {
     }
    void OnPressed (InputAction.CallbackContext obj) {

        Debug.Log (obj.duration);
    }
    private void OnEnable ( ) {
        pc.GamePlay.Enable ( );
    }
    private void OnDisable ( ) {
        pc.GamePlay.Disable ( );
    }
}
