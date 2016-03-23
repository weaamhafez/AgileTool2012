using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.EMF
{
    public class AppConstants
    {
        public static readonly string SPRINT_STATUS_OPEN = "OPEN";
        public static readonly string SPRINT_STATUS_CLOSED = "CLOSED";
        public static readonly string USERSTORY_STATUS_OPEN = "OPEN";
        public static readonly string USERSTORY_STATUS_FINISIHED = "DONE";
        public static readonly string DIAGRAM_STATUS_FINISIHED = "DELETED";
        public static readonly string DIAGRAM_STATUS_OPEN = "OPEN";
        public static string USERSTORY_STATUS_DELETED = "DELETED";
        public static readonly string SPRINT_STATUS_DELETED = "DELETED";

        public static readonly string EXCEPTION_GLOBAL = "Error in application, please contact administrator";
        public static readonly string EXCEPTION_RETREIVE_STORY_DIAGRAMS = "Cannot Retreive story's diagrams";

        public static string EXCEPTION_RETREIVE_STORY_OF_DIAGRAMS = "Cannot get stories";
        public static string EXCEPTION_RETREIVE_DIAGRAMS = "Cannot get diagrams";
        public static string EXCEPTION_DIAGRAM_CANNOT_EMPTY = "Please select diagram";
        public static string EXCEPTION_DIAGRAM_SAVING_ERROR = "Error in saving diagram";
        public static string EXCEPTION_USER_STORY_CANNOT_FIND = "Cannot find user story with this id";
        public static string EXCEPTION_SPRINT_CLOSE_ERROR = "Cannot Close sprint";
    }
}
