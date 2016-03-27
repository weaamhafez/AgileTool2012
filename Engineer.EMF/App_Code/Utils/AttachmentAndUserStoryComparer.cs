using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.EMF.Utils
{
    public class AttachmentAndUserStoryComparer : IEqualityComparer<UserStoryAttachment>
    {
        public bool Equals(UserStoryAttachment x, UserStoryAttachment y)
        {
            return x.attachId == y.attachId && x.userStoryId == y.userStoryId;
        }

        public int GetHashCode(UserStoryAttachment obj)
        {
            return obj.attachId + obj.userStoryId;
        }
    }
}
