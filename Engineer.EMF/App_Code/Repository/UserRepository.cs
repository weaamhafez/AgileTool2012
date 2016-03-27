using Engineer.EMF.Utils;
using Engineer.EMF.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.EMF
{
    public class UserRepository : BaseRepository
    {
        public List<AspNetUser> FindAll()
        {
            return db.AspNetUsers.ToList();
        }

        public AspNetUser FindById(string userId)
        {
            return db.AspNetUsers.SingleOrDefault(s => s.Id == userId);
        }

        public List<AspNetUser> FindByDiagram(string diagramId)
        {
            int id = int.Parse(diagramId);
            // get by creator
            var users = db.AspNetUsers.Where(w => w.UserStories
            .Where(story => story.UserStoryAttachments
            .Where(attach => attach.attachId == id).Count() > 0).Count() > 0).ToList();

            // get by shared
            users.AddRange(db.AspNetUsers.Where(w => w.UserStories1
             .Where(story => story.UserStoryAttachments
            .Where(attach => attach.attachId == id).Count() > 0).Count() > 0).AsEnumerable().Except(users,new UserComparer()));

            return users;
        }

        public List<AspNetUser> GetByProject(string projectId)
        {
            int projectIdInt = int.Parse(projectId);
            return db.AspNetUsers.Where(w => w.Projects.Where(proj => proj.Id == projectIdInt).Count() > 0).ToList();
        }
    }
}
