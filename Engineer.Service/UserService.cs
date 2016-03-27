using Engineer.EMF;
using Engineer.EMF.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Service
{
    public class UserService
    {
        UserRepository uRepository = new UserRepository();

        public List<AspNetUser> ListAll()
        {
            return uRepository.FindAll();
        }

        public List<AspNetUser> FindByDiagram(string diagramId)
        {
            return uRepository.FindByDiagram(diagramId);
        }

        public List<AspNetUser> FindAssignedUsersByProject(string projectId)
        {
            return uRepository.GetByProject(projectId);
        }
    }
}
