namespace Dash.Player {
    [System.Serializable]
    public class PlayerComponent {
        protected Player player = null;
        protected PlayerControl control = null;
        public PlayerComponent (Player player) {
            this.player = player;
            this.control = player.PlayerControl;
        }
        public virtual void Tick ( ) { }
        public virtual void FixedTick ( ) { }
        public virtual void OnEnable ( ) { }
        public virtual void OnDisable ( ) { }

    }
    public abstract class PlayerStats { }

}
