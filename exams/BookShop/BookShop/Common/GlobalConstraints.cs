using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Common
{
    public static class GlobalConstraints
    {
        //Author

        public const int AUTHOR_FIRSTNAME_MIN_LENGTH = 3;
        public const int AUTHOR_FIRSTNAME_MAX_LENGTH = 30;

        public const int AUTHOR_LASTNAME_MIN_LENGTH = 3;
        public const int AUTHOR_LASTTNAME_MAX_LENGTH = 30;

        public const string AUTHOR_PHONE_REGEX= @"^\d{3}\-\d{3}\-\d{4}$";

        //Book

        public const int BOOK_NAME_MIN_LENGTH = 3;
        public const int BOOK_NAME_MAX_LENGTH = 30;

        public const int BOOK_PAGES_MIN_LENGTH = 50;
        public const int BOOK_PAGES_MAX_LENGTH = 5000;
    }
}
