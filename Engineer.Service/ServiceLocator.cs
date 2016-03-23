using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Service
{
    public class ServiceLocator<T>
    {
        public object locate()
        {
            if (typeof(T).FullName == "Engineer.EMF.UserStory")
                return new UserStoryService();

            else if (typeof(T).FullName == "Engineer.EMF.Project")
                return new UserStoryService();

            else if (typeof(T).FullName == "Engineer.EMF.Sprint")
                return new SprintService();

            else if (typeof(T).FullName == "Engineer.EMF.Attachment")
                return new DiagramService();

            else if (typeof(T).FullName == "Engineer.EMF.AspNetUser")
                return new UserService();

            return null;
        }
    }
}
