namespace WepApi
{
    public static class AppConstants
    {
        public const string AppName = "UniqloApi";

        public const string FABRIC_TYPE_VALUE = "F";
        public const string FABRIC_TYPE_LABEL = "Fabric";
        public const string GREIGE_TYPE_VALUE = "G";
        public const string GREIGE_TYPE_LABEL = "Greige";

        public static readonly List<(string Value, string Label)> TypesList = new List<(string, string)>
        {
            (FABRIC_TYPE_VALUE, FABRIC_TYPE_LABEL),
            (GREIGE_TYPE_VALUE, GREIGE_TYPE_LABEL)
        };

        // RecordTypes constants
        public const string RECORD_TYPE_PRODUCTION_PLAN_VALUE = "1";
        public const string RECORD_TYPE_PRODUCTION_PLAN_LABEL = "Production Plan";
        public const string RECORD_TYPE_CONSUMPTION_ACTUAL_VALUE = "2";
        public const string RECORD_TYPE_CONSUMPTION_ACTUAL_LABEL = "Consumption Actual";
        public const string RECORD_TYPE_PRODUCTION_ACTUAL_VALUE = "3";
        public const string RECORD_TYPE_PRODUCTION_ACTUAL_LABEL = "Production Actual";

        public static readonly List<(string Value, string Label)> RecordTypes = new List<(string, string)>
        {
            (RECORD_TYPE_PRODUCTION_PLAN_VALUE, RECORD_TYPE_PRODUCTION_PLAN_LABEL),
            (RECORD_TYPE_CONSUMPTION_ACTUAL_VALUE, RECORD_TYPE_CONSUMPTION_ACTUAL_LABEL),
            (RECORD_TYPE_PRODUCTION_ACTUAL_VALUE, RECORD_TYPE_PRODUCTION_ACTUAL_LABEL)
        };

        // Status constants
        public const string STATUS_DART_VALUE = "D";
        public const string STATUS_DART_LABEL = "Dart";
        public const string STATUS_CONFIRMED_VALUE = "C";
        public const string STATUS_CONFIRMED_LABEL = "Confirmed";
        public const string STATUS_SENT_VALUE = "S";
        public const string STATUS_SENT_LABEL = "Sent";

        public static readonly List<(string Value, string Label)> StatusList = new List<(string, string)>
        {
            (STATUS_DART_VALUE, STATUS_DART_LABEL),
            (STATUS_CONFIRMED_VALUE, STATUS_CONFIRMED_LABEL),
            (STATUS_SENT_VALUE, STATUS_SENT_LABEL)
        };
    }
}