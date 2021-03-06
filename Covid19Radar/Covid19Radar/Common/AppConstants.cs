﻿namespace Covid19Radar.Common
{
    public static class AppConstants
    {
        public static readonly int NumberOfGroup = 86400;
        /// <summary>
        /// Number of days covered from the date of diagnosis or onset
        /// </summary>
        public const int DaysToSendTek = -3;
        /// <summary>
        /// Cache Timeout
        /// </summary>
        public const int CacheTimeout = 60;
        /// <summary>
        /// Active Rolling Period
        /// </summary>
        public const uint ActiveRollingPeriod = 144;
        /// <summary>
        /// Max Error Count
        /// </summary>
        public const int MaxErrorCount = 3;
        /// <summary>
        /// Max diagnosis UID Count
        /// </summary>
        public const int MaxDiagnosisUidCount = 8;

        public const string positiveRegex = @"\b[0-9]{8}\b";

    }
}
