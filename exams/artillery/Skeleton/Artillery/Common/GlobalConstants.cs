using System;
using System.Collections.Generic;
using System.Text;

namespace Artillery.Common
{
    public static class GlobalConstants
    {
        //Country
        public const int COUNTRY_NAME_MIN_LENGTH = 4;
        public const int COUNTRY_NAME_MAX_LENGTH = 60;
        public const int COUNTRY_ARMYSZ_MIN_LENGTH = 50000;
        public const int COUNTRY_ARMYSZ_MAX_LENGTH = 10_000_000;

        //Manufacturer
        public const int MANUFACTURER_NAME_MIN_LENGTH = 4;
        public const int MANUFACTURER_NAME_MAX_LENGTH = 40;
        public const int MANUFACTURER_FOUNDED_MIN_LENGTH = 10;
        public const int MANUFACTURER_FOUNDED_MAX_LENGTH = 100;

        //Shell
        public const double SHELL_WEIGHT_MAX_LENGTH = 1680;
        public const double SHELL_WEIGHT_MIN_LENGTH = 2;
        public const int SHELL_CALIBER_MIN_LENGTH = 4;
        public const int SHELL_CALIBER_MAX_LENGTH = 30;

        //Gun
        public const int GUN_WEIGHT_MIN_LENGTH = 100;
        public const int GUN_WEIGHT_MAX_LENGTH = 1_350_000;
        public const double GUN_BARREL_MIN_LENGTH = 2.00;
        public const double GUN_BARREL_MAX_LENGTH = 35.00;
        public const int GUN_RANGE_MIN_LENGTH = 1;
        public const int GUN_RANGE_MAX_LENGTH = 100_000;

    }
}
