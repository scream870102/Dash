namespace CJStudio.Dash.Player {
    [System.Serializable]
    class PlayerComponent {
        Player player = null;
        PlayerControl control = null;
        public Player Player => player;
        public PlayerControl Control => control;
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
