namespace CJStudio.Dash.MapObject {
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UnityEngine;
    class OneTouchObj : AMapObject {
        [SerializeField] float remainTime = 1f;
        void OnCollisionEnter2D (Collision2D other) {
            if (other.gameObject.tag == "Player") {
                List<ContactPoint2D> contacts = new List<ContactPoint2D> ( );
                col.GetContacts (contacts);
                foreach (ContactPoint2D o in contacts) {
                    Debug.Log (o.collider.name + " " + o.normal);
                    if (o.normal.y == -1f && o.collider.gameObject.tag == "Player") {
                        DisableObj ( );
                        return;
                    }
                }
            }
        }

        async void DisableObj ( ) {
            await Task.Delay ((int)(remainTime * 1000));
            SetActive (false);
        }
    }

}
