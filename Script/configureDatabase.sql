DROP TABLE [contacts]
GO
DROP TABLE [tag_validations]
GO
DROP TABLE [tags]
GO
DROP TABLE [user_data]
GO
DROP TABLE [users]
GO
DROP TABLE [dhbw_domains]
GO
DROP VIEW [vw_users]
GO

-- Create DHBW table
CREATE TABLE "dhbw_domains" (
    "domain"        VARCHAR(30) NOT NULL,
    "location"      VARCHAR(30) NOT NULL,

    CONSTRAINT "PK_dhbw_domains"
        PRIMARY KEY ("domain")
)
GO

-- Create USER table
CREATE TABLE "users" (
    "user_id"           CHAR(24),
    "email_prefix"      VARCHAR(30) NOT NULL,
    "email_domain"      VARCHAR(30) NOT NULL,
    "created_at"        DATETIME DEFAULT CURRENT_TIMESTAMP

    CONSTRAINT "PK_users"
        PRIMARY KEY ("user_id"),

	CONSTRAINT "FK_users_email_domain"
        FOREIGN KEY ("email_domain") REFERENCES [dhbw_domains]("domain"),

	CONSTRAINT "UQ_users_no_duplicate_emails"
        UNIQUE ("email_prefix", "email_domain")
)
GO

-- Create USER-DATA table
CREATE TABLE "user_data" (
    "user"              CHAR(24),
    "firstname"         VARCHAR(30) NOT NULL,
    "lastname"          VARCHAR(30) NOT NULL,
    "course_abbr"       VARCHAR(15) NOT NULL,
    "course"            VARCHAR(30) NOT NULL,
    "specialization"    VARCHAR(50),
    "city"              VARCHAR(30),
    "biography"         NVARCHAR(1000) COLLATE Latin1_General_100_CI_AI_SC_UTF8,
    "rfid_id"           VARCHAR(30)

	CONSTRAINT "PK_user-data"
        PRIMARY KEY ("user"),

    CONSTRAINT "FK_user_data_user_id"
        FOREIGN KEY ("user") REFERENCES [users]("user_id"),
)
GO

-- Create TAG table
CREATE TABLE "tags" (
    "tag_id"        INT IDENTITY(1000, 1),
    "tag"           VARCHAR(15) NOT NULL,
    "user"          CHAR(24) NOT NULL,
    "created_at"    DATETIME DEFAULT CURRENT_TIMESTAMP

    CONSTRAINT "PK_tags"
        PRIMARY KEY ("tag_id"),

	CONSTRAINT "FK_tags-user_id"
        FOREIGN KEY ("user") REFERENCES [user_data]("user"),

    CONSTRAINT "UQ_tags_no_duplicate_tags" 
        UNIQUE ("tag", "user")
)
GO

-- Create TAG-VALIDATION table
CREATE TABLE "tag_validations" (
    "validation_id" INT IDENTITY(1000, 1),
    "tag"           INT NOT NULL,
    "validated_by"  CHAR(24) NOT NULL,
    "comment"       NVARCHAR(250) COLLATE Latin1_General_100_CI_AI_SC_UTF8,
    "created_at"    DATETIME DEFAULT CURRENT_TIMESTAMP

    CONSTRAINT "PK_tag_validations"
        PRIMARY KEY ("validation_id"),

	CONSTRAINT "FK_tag_validations_tag"
        FOREIGN KEY ("tag") REFERENCES [tags]("tag_id"),

	CONSTRAINT "FK_tag_validations_validated_by"
        FOREIGN KEY ("validated_by") REFERENCES [user_data]("user"),

    CONSTRAINT "UQ_tag_validations_no_duplicate_validations" 
        UNIQUE ("tag", "validated_by")
)
GO

-- Create CONTACTS table
CREATE TABLE "contacts" (
    "user"          CHAR(24) NOT NULL,
    "contact"       CHAR(24) NOT NULL,
    "created_at"    DATETIME DEFAULT CURRENT_TIMESTAMP

	CONSTRAINT "FK_contacts_user"
        FOREIGN KEY ("user") REFERENCES [user_data]("user"),

	CONSTRAINT "FK_contacts_contact"
        FOREIGN KEY ("contact") REFERENCES [user_data]("user"),

    CONSTRAINT "UQ_contacts_no_duplicate_contacts"
        PRIMARY KEY ("user", "contact")
)
GO

-- Create USER Views
CREATE VIEW [vw_users] AS
    SELECT 
	    [user_id], 
	    [firstname], 
	    [lastname], 
	    CONCAT([email_prefix], '@', [email_domain]) AS email, 
	    [location] AS dhbw_location,
	    [course_abbr],
	    [course],
	    [specialization],
	    [city],
        [biography],
	    CAST((SELECT CASE WHEN EXISTS(SELECT * FROM [user_data] WHERE [user] = [user_id]) THEN 1 ELSE 0 END) AS BIT) AS registered,
	    [created_at]
    FROM [users]
    LEFT JOIN [user_data] ON [users].[user_id] = [user_data].[user]
    LEFT JOIN [dhbw_domains] ON [users].[email_domain] = [dhbw_domains].[domain]
GO

-- Create ON-USER-DELETE Trigger
CREATE TRIGGER [dbo].[TGR_on_user_delete]
    ON [dbo].[users]
    INSTEAD OF DELETE
AS
    DELETE FROM [dbo].[contacts]
		WHERE 
			[dbo].[contacts].[user] IN(SELECT deleted.[user_id] FROM deleted) OR
			[dbo].[contacts].[contact] IN(SELECT deleted.[user_id] FROM deleted)

	DELETE FROM [dbo].[tag_validations]
		WHERE 
			[dbo].[tag_validations].[validated_by] IN(SELECT deleted.[user_id] FROM deleted)

	DELETE FROM [dbo].[tags]
		WHERE 
			[dbo].[tags].[user] IN(SELECT deleted.[user_id] FROM deleted)

    DELETE FROM [dbo].[user_data]
		WHERE 
			[dbo].[user_data].[user] IN(SELECT deleted.[user_id] FROM deleted)

	DELETE FROM [dbo].[users]
		WHERE 
			[dbo].[users].[user_id] IN(SELECT deleted.[user_id] FROM deleted)
GO

-- Create ON-USER-DATA-DELETE Trigger
CREATE TRIGGER [dbo].[TGR_on_user_data_delete]
    ON [dbo].[user_data]
    INSTEAD OF DELETE
AS
    DELETE FROM [dbo].[contacts]
		WHERE 
			[dbo].[contacts].[user] IN(SELECT deleted.[user] FROM deleted) OR
			[dbo].[contacts].[contact] IN(SELECT deleted.[user] FROM deleted)

	DELETE FROM [dbo].[tag_validations]
		WHERE 
			[dbo].[tag_validations].[validated_by] IN(SELECT deleted.[user] FROM deleted)

	DELETE FROM [dbo].[tags]
		WHERE 
			[dbo].[tags].[user] IN(SELECT deleted.[user] FROM deleted)

    DELETE FROM [dbo].[user_data]
		WHERE 
			[dbo].[user_data].[user] IN(SELECT deleted.[user] FROM deleted)
GO

-- Create ON-TAG-DELETE Trigger
CREATE TRIGGER [dbo].[TGR_on_tag_delete]
    ON [dbo].[tags]
    INSTEAD OF DELETE
AS
    DELETE FROM [dbo].[tag_validations]
		WHERE 
			[dbo].[tag_validations].[tag] IN(SELECT deleted.[tag_id] FROM deleted)

	DELETE FROM [dbo].[tags]
		WHERE 
			[dbo].[tags].[tag_id] IN(SELECT deleted.[tag_id] FROM deleted)
 GO