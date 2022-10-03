using System;
using System.Collections.Generic;
using System.Text;

namespace TeisterMask.Common
{
    public static class GlobalConstants
    {
        //EMPLOYEE
        public const int EMPLOYEE_USERNAME_MAX_LENHTH = 40;
        public const int EMPLOYEE_USERNAME_MIN_LENGTH = 3;
        public const string EMPLOYEE_USERNAME_REGEX = @"^[A-Za-z0-9]+$";
        public const string EMPLOYEE_PHONE_REGEX = @"^\d{3}\-\d{3}\-\d{4}$";

        //Project
        public const int PROJECT_NAME_MAX_LENHTH = 40;
        public const int PROJECT_NAME_MIN_LENHTH = 2;

        //TASKS
        public const int TASK_NAME_MAX_LENGTH = 40;
        public const int TASK_NAME_MIN_LENHTH = 2;

    }
}
