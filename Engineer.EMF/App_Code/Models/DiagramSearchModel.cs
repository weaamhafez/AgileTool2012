using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.EMF.Models
{
    public class DiagramSearchModel
    {
        public string DiagramName { set; get; }
        public string SprintNumber { set; get; }
        public string Users { set; get; }
        public string UserStoryName { set; get; }
        public int SprintId { set; get; }
        public int UserStoryId { set; get; }
        public int AttachmentId { set; get; }
        public int HistoryId { set; get; }
        public DateTime Date {set;get;}
        public int Version { set; get; }
    }
}
