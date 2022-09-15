namespace DESpeedrunUtil.Hotkeys {

    /// <summary>
    /// Extension of EventArgs to send MouseWheel scroll direction
    /// </summary>
    internal class MouseWheelEventArgs: EventArgs {
        public bool Direction { get; init; }

        public MouseWheelEventArgs(bool dir) : base() {
            Direction = dir;
        }
    }
}
