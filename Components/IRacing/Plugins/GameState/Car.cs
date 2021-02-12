namespace Slipstream.Components.IRacing.Plugins.GameState
{
    public class Car
    {
        public int CarIdx { get; set; }
        public string CarNumber { get; set; } = string.Empty;
        public long UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public long TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string CarName { get; set; } = string.Empty;
        public string CarNameShort { get; set; } = string.Empty;
        public long IRating { get; set; }
        public string License { get; set; } = string.Empty;
        public bool IsSpectator { get; set; }
        public int LapsCompleted { get; set; }
        public bool OnPitRoad { get; set; }
        public int ClassPosition { get; set; }
        public int Position { get; set; }
    }
}