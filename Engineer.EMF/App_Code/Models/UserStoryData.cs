using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.EMF.Models
{
    [Serializable]
    public class UserStoryData
    {
        public string ProjectId { set; get; }
        public List<string>  Users { set; get; }
    }
}
