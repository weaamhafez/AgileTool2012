using Engineer.EMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.EMF.Utils
{
    public class AttachUSAndSprintComparer : IEqualityComparer<DiagramSearchModel>
    {
        public bool Equals(DiagramSearchModel x, DiagramSearchModel y)
        {
            return x.AttachmentId == y.AttachmentId && x.UserStoryId == y.UserStoryId && x.SprintId == y.SprintId;
        }

        public int GetHashCode(DiagramSearchModel obj)
        {
            return obj.AttachmentId;
        }
    }
}
