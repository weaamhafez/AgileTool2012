using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.EMF.Utils
{
    public class AttachmentAndUserStoryComparer : IEqualityComparer<Attachment>
    {
        public bool Equals(Attachment x, Attachment y)
        {
            throw new NotImplementedException();
            //return x.Id == y.Id && x.user
        }

        public int GetHashCode(Attachment obj)
        {
            throw new NotImplementedException();
        }
    }
}
