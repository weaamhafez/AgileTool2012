ALTER TABLE [dbo].[SprintHistory]
    ADD [version] INT NOT NULL;

	ALTER TABLE [dbo].[Sprint]
    ADD [version] INT NULL;

	ALTER TABLE [dbo].[UserStoryAttachment]
    ADD [version] INT NULL;

	ALTER TABLE [dbo].[AttachmentHistory]
    ADD [state]   VARCHAR (50) NULL,
        [version] INT          NULL;