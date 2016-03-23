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
    public class SprintRepository 
    {
        Entities db = RepositoryLocator.loadDBContext();
        public List<Sprint> FindByUser(string userId)
        {
            var sprints = new List<Sprint>();
            try
            {
                var filteredSprints = db.Sprints.ToList().Where(w => w.state != AppConstants.SPRINT_STATUS_DELETED).ToList();
                filteredSprints.ForEach(sprint =>
                {
                    if(sprint.UserStories.Where(w=>w.AspNetUsers.SingleOrDefault(s=>s.Id == userId) != null).Count() > 0)
                    {
                        sprints.Add(sprint);
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            sprints = sprints.OrderByDescending(o => o.Id).ToList();
            return sprints;
        }

        public void SaveOrUpdate(Sprint sprint, string userId)
        {
            if (sprint.Id > 0)
                Update(sprint);
            else
                Add(sprint, userId);
        }
        private void Add(Sprint sprint, string userId)
        {
            sprint.sDate = DateTime.Now;
            sprint.state = AppConstants.SPRINT_STATUS_OPEN;
            db.Sprints.Add(sprint);
            db.SaveChanges();
        }

        public void Update(Sprint sprint)
        {
            var exist = Get(sprint);

            exist.topic = sprint.topic;
            db.SaveChanges();
        }

        public void UpdateState(Sprint sprint)
        {
            var exist = Get(sprint);
            exist.state = sprint.state;
            db.SaveChanges();
        }

        public void Delete(Sprint sprint)
        {
            var exist = Get(sprint);
            if (exist.UserStories.ToList().Any())
                throw new BadRequestException("This story is shared between user stories");

            db.Sprints.Remove(exist);
            db.SaveChanges();


        }

        public Sprint Get(Sprint sprint)
        {
            var exist = db.Sprints.SingleOrDefault(w => w.Id == sprint.Id);
            if (exist == null)
                throw new NotExistItemException("Sprint Not exist");
            return exist;
        }

        public void Close(Sprint sprint, string userId)
        {
            var exist = Get(sprint);
            exist.state = "Closed";
            exist.eDate = DateTime.Now;
            db.SaveChanges();
            SaveToHistory(sprint,userId);
        }

        private void SaveToHistory(Sprint sprint,string userId)
        {
            SprintHistory history = new SprintHistory()
            {
                sprintId = sprint.Id,
                closedBy = userId,
                closeDate = DateTime.Now,
            };
            db.SprintHistories.Add(history);
            db.SaveChanges();
        }
    }
}
