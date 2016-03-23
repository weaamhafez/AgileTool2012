using Engineer.EMF.Utils.Exceptions;
using Engineer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.EMF
{
    public class ProjectRepository : BaseRepository
    {
        UserStoryRepository userStoryRepository = new UserStoryRepository();
        public List<Project> FindByUserID(string userId)
        {
            var projects = new List<Project>();
            try
            {
                var userStories = userStoryRepository.FindByUser(userId);
                if(userStories != null)
                {
                    userStories.ForEach(us => {
                        if(us.Project != null)
                            projects.Add(us.Project);
                    });
                    if(db.Projects != null)
                    {
                        var projectsByMe = db.Projects.Where(w => w.created_by == userId).ToList();
                        projects.AddRange(db.Projects.Where(w => w.created_by == userId).ToList().Where(w => !projects.Contains(w) && w != null).ToList());
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return projects;
        }
        public void SaveOrUpdate(Project project , string userId)
        {
            if (project.Id > 0)
                Update(project, userId);
            else
                Add(project, userId);
        }
        private void Add(Project project,string userId)
        {
            project.created_by = userId;
            project.created_date = DateTime.Now;
            db.Projects.Add(project);
            db.SaveChanges();
        }

        private void Update(Project project,string userId)
        {
            var existProject = db.Projects.SingleOrDefault(w => w.Id == project.Id);
            if (existProject == null)
                throw new NotExistItemException("Project Not exist");

            existProject.name = project.name;
            existProject.description = project.description;
            existProject.updated_date = DateTime.Now;
            existProject.update_by = userId;
            db.SaveChanges();
        }

        public void Delete(Project project)
        {
            var existProject = db.Projects.SingleOrDefault(w => w.Id == project.Id);
            if (existProject == null)
                throw new NotExistItemException("Project Not exist");

            if (existProject.UserStories.ToList().Any())
                throw new BadRequestException("there is related user stories on this project");

            db.Projects.Remove(existProject);
            db.SaveChanges();
        }
    }
}
