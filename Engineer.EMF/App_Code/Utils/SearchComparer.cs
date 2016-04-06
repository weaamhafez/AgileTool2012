using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.EMF.App_Code.Utils
{
    public class SearchComparer : IEqualityComparer<UserStoryAttachment>
    {
        public bool Equals(UserStoryAttachment x, UserStoryAttachment y)
        {
            return x.attachId == y.attachId
                    && x.userStoryId == y.userStoryId
                    && string.Join(",", x.UserStory.Sprints.Select(s => s.Id.ToString())) == string.Join(",", y.UserStory.Sprints.Select(s => s.Id.ToString()))
                    && string.Join(",", x.UserStory.AspNetUsers.Select(s => s.Id.ToString())) == string.Join(",", y.UserStory.AspNetUsers.Select(s => s.Id.ToString()));
        }

        public int GetHashCode(UserStoryAttachment obj)
        {
            return obj.attachId + obj.userStoryId;
        }
    }
}
