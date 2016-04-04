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
                return db.Projects.Where(w => w.AspNetUsers.Where(user => user.Id == userId).Count() > 0 && w.state != AppConstants.PROJECT_STATUS_DELETED).ToList();
                //var userStories = userStoryRepository.FindByUser(userId);
                //if(userStories != null)
                //{
                //    userStories.ForEach(us => {
                //        if(us.Project != null)
                //            projects.Add(us.Project);
                //    });
                //    if(db.Projects != null)
                //    {
                //        var projectsByMe = db.Projects.Where(w => w.created_by == userId).ToList();
                //        projects.AddRange(db.Projects.Where(w => w.created_by == userId && w.state != AppConstants.PROJECT_STATUS_DELETED).ToList().Where(w => !projects.Contains(w) && w != null).ToList());
                //    }
                //}
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return projects;
        }
        public void SaveOrUpdate(Project project , string userId,string users)
        {
            if (project.Id > 0)
                Update(project, userId,users);
            else
                Add(project, userId,users);
        }
        private void Add(Project project,string userId,string users)
        {
            project.created_by = userId;
            project.created_date = DateTime.Now;
            if(!string.IsNullOrEmpty(users))
            {
                UserRepository uRep = new UserRepository();
                foreach(string assignUser in users.Split(','))
                {
                    project.AspNetUsers.Add(uRep.FindById(assignUser));
                }
            }
            project = db.Projects.Add(project);
            db.SaveChanges();
        }

        private void Update(Project project,string userId,string users)
        {
            var existProject = db.Projects.SingleOrDefault(w => w.Id == project.Id);
            if (existProject == null)
                throw new NotExistItemException("Project Not exist");


            if (!string.IsNullOrEmpty(users))
            {
                UserRepository uRep = new UserRepository();
                foreach (string assignUser in users.Split(','))
                {
                    project.AspNetUsers.Add(uRep.FindById(assignUser));
                }
            }

            existProject.name = project.name;
            existProject.description = project.description;
            existProject.updated_date = DateTime.Now;
            existProject.update_by = userId;
            existProject.AspNetUsers = new List<AspNetUser>();
            existProject.AspNetUsers = project.AspNetUsers;
            db.SaveChanges();
        }
        public void UpdateStatus(Project project, string userId)
        {
            var exist = GetById(project.Id);
            exist.state = project.state;
            exist.updated_date = DateTime.Now;
            exist.update_by = userId;
            db.SaveChanges();
        }
       public Project GetById(int projectId)
        {
            return db.Projects.SingleOrDefault(s => s.Id == projectId);
        }
    }
}
