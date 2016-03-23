using Engineer.EMF.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.EMF
{
    public class UserStoryRepository : BaseRepository
    {
        public List<UserStory> FindByUser(string userId)
        {
            var stories = new List<UserStory>();
            try
            {
                db.UserStories.Where(w=>w.state != AppConstants.USERSTORY_STATUS_DELETED).ToList().ForEach(story=>
                {
                    if(story.AspNetUsers != null)
                    {
                        var user = story.AspNetUsers.ToList().SingleOrDefault(s => s.Id == userId);
                        if (user != null)
                            stories.Add(story);
                    }
                    
                }
                
              );
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Unknow error, please contact adminstrator");
            }

            return stories;
        }

        public void SaveOrUpdate(UserStory story, string userId)
        {
            if (story.Id > 0)
                Update(story);
            else
                Add(story, userId);
        }
        private void Add(UserStory story, string userId)
        {
            story.creator = userId;
            story.state = AppConstants.USERSTORY_STATUS_OPEN;

            db.UserStories.Add(story);
            db.SaveChanges();
        }

        public void Update(UserStory story)
        {
            var exist = Get(story);

            exist.name = story.name;
            exist.description = story.description;
            exist.updateDate = DateTime.Now;
            exist.Project = story.Project;
            exist.state = story.state;
            db.SaveChanges();
        }

        public void UpdateState(UserStory story)
        {
            var exist = Get(story);
            exist.state = story.state;
            db.SaveChanges();
        }

        public void Delete(UserStory story)
        {
            var exist = Get(story);

            exist.state = AppConstants.USERSTORY_STATUS_DELETED;
            db.SaveChanges();

            
        }

        public UserStory Get(UserStory story)
        {
            var exist = db.UserStories.SingleOrDefault(w => w.Id == story.Id);
            if (exist == null)
                throw new NotExistItemException("Project Not exist");
            return exist;
        }


        public void SaveToHistory(UserStory story, string userId)
        {
            UserStoryHistory history = new UserStoryHistory()
            {
                userStoryId = story.Id,
                closedBy = userId,
                closeDate = DateTime.Now,
            };
            db.UserStoryHistories.Add(history);
            db.SaveChanges();
        }


        public UserStory FindByID(string Id)
        {
            int sId = int.Parse(Id);
            return db.UserStories.SingleOrDefault(s => s.Id == sId && s.state != AppConstants.USERSTORY_STATUS_DELETED);
        }

        public List<UserStory> FindByDiagramID(int diagramId)
        {
            return db.UserStories.Where(w => w.Attachments.Where(attach => attach.Id == diagramId).Count() > 0).ToList();
        }

        public List<UserStory> ListAll()
        {
            return db.UserStories.Where(w => w.state != AppConstants.USERSTORY_STATUS_DELETED).ToList();
        }
    }
}
