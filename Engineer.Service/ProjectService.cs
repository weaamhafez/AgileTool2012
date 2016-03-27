using Engineer.EMF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Service
{
    public class ProjectService
    {
        ProjectRepository rep = new ProjectRepository();
        public void Delete(Project project,string userId)
        {
            project.state = AppConstants.PROJECT_STATUS_DELETED;
            rep.UpdateStatus(project,userId);

            var existProject = rep.GetById(project.Id);
            UserStoryService service = (UserStoryService)new ServiceLocator<Engineer.EMF.UserStory>().locate();
            if (existProject.UserStories.ToList().Any())
            {
                existProject.UserStories.ToList().ForEach(us =>
                {
                    us.state = AppConstants.USERSTORY_STATUS_DELETED;
                    service.Delete(us);
                });
            }
        }
    }
}
