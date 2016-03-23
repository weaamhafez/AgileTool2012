using System;
using System.Collections.Generic;

namespace Engineer.EMF.Utils
{
    public class UserComparer : IEqualityComparer<AspNetUser>
    {
        public bool Equals(AspNetUser x, AspNetUser y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(AspNetUser obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}