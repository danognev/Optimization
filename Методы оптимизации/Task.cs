using PropertyChanged;


namespace OptimizatonMethods
{
    [AddINotifyPropertyChangedInterface]
    public class Task
    {
        public double? Alpha { get; set; }
        public double? Beta { get; set; }
        public double? Mu { get; set; }
        public double? G { get; set; }
        public double? H { get; set; }
        public double? N { get; set; }
        public double? TCoilMin { get; set; }
        public double? TCoilMax { get; set; }
        public double? TCasingMin { get; set; }
        public double? TCasingMax { get; set; }
        public double? Price { get; set; }
        public double? Step { get; set; }
        public double? TempSum { get; set; }
    }
}
