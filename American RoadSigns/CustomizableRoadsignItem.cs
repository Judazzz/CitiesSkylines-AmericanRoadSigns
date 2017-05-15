namespace AmericanRoadSigns
{
    public class CustomizableRoadsignItem
    {
        public PropInfo _originalSign;
        public PropInfo _customSign;

        private PropInfo originalSign
        {
            get { return _originalSign; }
            set { _originalSign = value; }
        }

        private PropInfo customSign
        {
            get { return _customSign; }
            set { _customSign = value; }
        }
    }
}
