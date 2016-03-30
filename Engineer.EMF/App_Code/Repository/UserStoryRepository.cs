using Engineer.EMF.App_Code.Utils;
using Engineer.EMF.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engineer.EMF.Models;

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
                        if (user != null && !stories.Contains(story,new UserStoryComparer()))
                            stories.Add(story);
                    }
                    //if (story.AspNetUser != null && story.AspNetUser.Id == userId && !stories.Contains(story, new UserStoryComparer()))
                    //    stories.Add(story);
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

        public List<UserStory> FindBySprint(int sprintId)
        {
            var result = new List<UserStory>();
            var sprints = db.Sprints.Where(w => w.Id == sprintId && w.state != AppConstants.DIAGRAM_STATUS_FINISIHED);
            sprints.ToList().ForEach(f =>
            {
                result.AddRange(f.UserStories);
            }
            );
            return result.Distinct(new UserStoryComparer()).ToList();
        }

        public void Update(UserStory story)
        {
            var exist = Get(story);

            exist.name = story.name;
            exist.description = story.description;
            exist.updateDate = DateTime.Now;
            exist.Project = story.Project;
            exist.state = story.state;
            exist.AspNetUsers = story.AspNetUsers;
            db.SaveChanges();
        }

        public void UpdateStoryAttachment(UserStory story)
        {
            var exist = Get(story);
            exist.UserStoryAttachments = story.UserStoryAttachments;
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
            return db.UserStories.Where(w => w.UserStoryAttachments.Where(attach => attach.attachId == diagramId).Count() > 0).ToList();
        }

        public List<UserStory> ListAll()
        {
            return db.UserStories.Where(w => w.state != AppConstants.USERSTORY_STATUS_DELETED).ToList();
        }

        public UserStoryData FindProjectAndUsers(int storyId)
        {
            var story = db.UserStories.SingleOrDefault(w => w.Id == storyId);
            var userStoryAndData = new UserStoryData();
            if(story != null)
            {
                userStoryAndData.ProjectId = story.projectId.ToString();
                userStoryAndData.Users = story.AspNetUsers.Select(s => s.Id).ToList();
            }

            return userStoryAndData;
        }
    }
}
