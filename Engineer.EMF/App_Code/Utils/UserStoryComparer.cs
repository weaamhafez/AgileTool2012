using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.EMF.App_Code.Utils
{
    public class UserStoryComparer : IEqualityComparer<UserStory>
    {
        public bool Equals(UserStory x, UserStory y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(UserStory obj)
        {
            return obj.Id;
        }
    }
}
