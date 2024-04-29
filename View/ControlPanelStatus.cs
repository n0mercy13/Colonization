namespace Codebase.View
{
    public struct ControlPanelStatus
    {
        public bool IsScanOn { get; private set; }
        public bool IsBuildOn { get; private set; }
        public bool IsCollectOn { get; private set; }


        public void SetScan(bool value) =>
            IsScanOn = value;

        public void SetBuild(bool value) => 
            IsBuildOn = value;

        public void SetCollect(bool value) =>
            IsCollectOn = value;
    }
}
