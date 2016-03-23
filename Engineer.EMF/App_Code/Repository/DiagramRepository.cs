using Engineer.EMF.Utils;
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
    public class DiagramRepository : BaseRepository
    {
        public List<Attachment> ListAll(string userId)
        {
            var diagrams = new List<Attachment>();
            try
            {
                db.Attachments.Where(w=>w.state != AppConstants.DIAGRAM_STATUS_FINISIHED).ToList().ForEach(diagram =>
                {
                    diagram.UserStories.ToList().ForEach(
                        us =>
                        {
                            var userObject = us.AspNetUsers.ToList().SingleOrDefault(user => user.Id == userId);
                            if (userObject != null && !diagrams.Contains(diagram,new AttachmentComparer()))
                                diagrams.Add(diagram);
                        });
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return diagrams;
        }

        public List<Attachment> FindAll()
        {
            return db.Attachments.Where(w => w.state != AppConstants.DIAGRAM_STATUS_FINISIHED).ToList();
        }

        public List<Attachment> FindByUsers(string[] users)
        {
            var attachments = new List<Attachment>();
            users.ToList().ForEach(userId =>
            {
                attachments.AddRange(ListAll(userId));
            });
            return attachments;
        }

        public List<Attachment> FindByUsersAndStories(string[] users, string[] stories)
        {
            var attachments = new List<Attachment>();
            if(users != null)
            {
                users.ToList().ForEach(userId =>
                {
                    attachments.AddRange(ListAll(userId));
                });
            }
           
            if(stories != null)
            {
                stories.ToList().ForEach(story =>
                {
                    int storyId = int.Parse(story);
                    attachments.AddRange(db.Attachments.Where(
                        attach => 
                        attach.UserStories.Where(t => t.Id == storyId).Count() > 0 && attach.state != AppConstants.DIAGRAM_STATUS_FINISIHED
                        ).AsEnumerable().Except(attachments, new AttachmentComparer()));
                });
            }
            return attachments;
        }

        public List<Attachment> FindByStoryID(int id)
        {
            return db.Attachments.Where(w => w.UserStories.Where(s => s.Id == id).Count() > 0).ToList();
        }

        public void UpdateStatus(Attachment diagram)
        {
            var exist = Get(diagram);

            exist.state = diagram.state;
            db.SaveChanges();
        }

        public int SaveOrUpdate(Attachment diagram, string userId)
        {
            int id = 0;
            if (diagram.Id > 0)
                Update(diagram,userId);
            else
                id = Add(diagram, userId);

            return diagram.Id > 0 ? diagram.Id : id;
        }
        private int Add(Attachment diagram, string userId)
        {
            diagram.created_by = userId;
            diagram.create_date = DateTime.Now;
            diagram.state = AppConstants.DIAGRAM_STATUS_OPEN;

            Attachment attach = db.Attachments.Add(diagram);
            db.SaveChanges();
            return attach.Id;
        }

        public void Update(Attachment diagram,string userId)
        {
            var exist = Get(diagram);

            exist.name = diagram.name;
            exist.activties = diagram.activties;
            exist.update_date = DateTime.Now;
            exist.update_by = userId;
            exist.state = diagram.state;
            db.SaveChanges();
        }

        public void UpdateLock(Attachment diagram)
        {
            try
            {
                var exist = Get(diagram);
                if (exist.state != AppConstants.DIAGRAM_STATUS_FINISIHED)
                {
                    exist.@readonly = diagram.@readonly;
                    db.SaveChanges();
                }
            }
            catch (NotExistItemException ex)
            {
                Console.WriteLine(ex.ErrorMessage);
            }
        }

        public Attachment Get(Attachment diagram)
        {
            var exist = db.Attachments.SingleOrDefault(w => w.Id == diagram.Id && w.state != AppConstants.DIAGRAM_STATUS_FINISIHED);
            if (exist == null)
                throw new NotExistItemException("Diagram id: " + diagram.Id + " Not exist");
            return exist;
        }

        public void SaveToHistory(Attachment diagram, string userId)
        {
            AttachmentHistory history = new AttachmentHistory()
            {
                Graph = diagram.activties,
                UserId = userId,
                Date = DateTime.Now,
                AttachId = diagram.Id
            };
            db.AttachmentHistories.Add(history);
            db.SaveChanges();
        }

        public List<Attachment> FindBySprintID(int sprintId)
        {
            SprintRepository sRepository = new SprintRepository();
            var sprint = sRepository.Get(new Sprint() { Id = sprintId });
            var attachments = new List<Attachment>();
            if (sprint.UserStories.Count() > 0)
            {
                sprint.UserStories.ToList().ForEach(us =>
                {
                    attachments.AddRange(us.Attachments.Except(attachments, new AttachmentComparer()));
                });
            }
            return attachments;
        }
    }
}
