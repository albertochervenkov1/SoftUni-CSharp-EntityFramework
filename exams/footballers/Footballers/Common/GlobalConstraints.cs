using System;
using System.Collections.Generic;
using System.Text;

namespace Footballers.Common
{
    public static class GlobalConstraints
    {
        //Footballer
        public const int FOTBBALLER_NAME_MIN_LENGTH = 2;
        public const int FOTBBALLER_NAME_MAX_LENGTH = 40;
        
        //Team

        public const int TEAM_NAME_MIN_LENGTH = 3;
        public const int TEAM_NAME_MAX_LENGTH = 40;

        public const int TEAM_NAT_MIN_LENGTH = 2;
        public const int TEAM_NAT_MAX_LENGTH = 40;

        public const string TEAM_NAME_REGEX = @"^[A-Za-z0-9\s.-]+$";

        //Coach
        public const int COACH_NAME_MIN_LENGTH = 2;
        public const int COACH_NAME_MAX_LENGTH = 40;

    }
}
