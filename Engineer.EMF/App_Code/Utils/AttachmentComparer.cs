using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.EMF.Utils
{
    public class AttachmentComparer : IEqualityComparer<Attachment>
    {
        public bool Equals(Attachment x, Attachment y)
        {

            return x.Id == y.Id;
        }

        public int GetHashCode(Attachment obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
