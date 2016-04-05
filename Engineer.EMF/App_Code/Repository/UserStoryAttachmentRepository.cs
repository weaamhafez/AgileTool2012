using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.EMF
{
    public class UserStoryAttachmentRepository : BaseRepository
    {
        public UserStoryAttachment Get(UserStoryAttachment userStoryAttachment)
        {
            return db.UserStoryAttachments.SingleOrDefault(w => w.attachId == userStoryAttachment.attachId && w.userStoryId == userStoryAttachment.userStoryId);
        }

        public void UpdateStatus(UserStoryAttachment diagram)
        {
            var exist = Get(diagram);

            exist.state = diagram.state;
            db.SaveChanges();
            diagram = exist;
        }

        public AttachmentHistory GetHistory(int historyId)
        {
            return db.AttachmentHistories.SingleOrDefault(w => w.Id == historyId);
        }

        public void Add(List<UserStoryAttachment> attachs)
        {
            if(attachs != null)
            {
                db.UserStoryAttachments.AddRange(attachs);
                db.SaveChanges();
            }
        }
    }
}
