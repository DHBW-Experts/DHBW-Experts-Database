DROP TABLE [AUTH0_contacts]
GO
DROP TABLE [AUTH0_tag_validations]
GO
DROP TABLE [AUTH0_tags]
GO
DROP TABLE [AUTH0_user_data]
GO
DROP TABLE [AUTH0_users]
GO
DROP TABLE [AUTH0_dhbw_domains]
GO
DROP VIEW [vw_users]
GO
----------------------------------------------------------
-- NEW TABLES
----------------------------------------------------------

-- Create DHBW table
CREATE TABLE "AUTH0_dhbw_domains" (
    "domain"        VARCHAR(30) NOT NULL,
    "location"      VARCHAR(30) NOT NULL,

    CONSTRAINT "PK_dhbw_domains"
        PRIMARY KEY ("domain")
)
GO

-- Create USER table
CREATE TABLE "AUTH0_users" (
    "user_id"           CHAR(24),
    "email_prefix"      VARCHAR(30) NOT NULL,
    "email_domain"      VARCHAR(30) NOT NULL,
    "created_at"        DATETIME DEFAULT CURRENT_TIMESTAMP

    CONSTRAINT "PK_users"
        PRIMARY KEY ("user_id"),

	CONSTRAINT "FK_users_email_domain"
        FOREIGN KEY ("email_domain") REFERENCES [AUTH0_dhbw_domains]("domain"),

	CONSTRAINT "UQ_users_no_duplicate_emails"
        UNIQUE ("email_prefix", "email_domain")
)
GO

-- Create USER-DATA table
CREATE TABLE "AUTH0_user_data" (
    "user"              CHAR(24),
    "firstname"         VARCHAR(30) NOT NULL,
    "lastname"          VARCHAR(30) NOT NULL,
    "course_abbr"       VARCHAR(15) NOT NULL,
    "course"            VARCHAR(30) NOT NULL,
    "specialization"    VARCHAR(50),
    "city"              VARCHAR(30),
    "biography"         NVARCHAR(1000) COLLATE Latin1_General_100_CI_AI_SC_UTF8,
    "rfid_id"           VARCHAR(30)

	CONSTRAINT "FK_user_data_user_id"
        FOREIGN KEY ("user") REFERENCES [AUTH0_users]("user_id"),
)
GO

-- Create TAG table
CREATE TABLE "AUTH0_tags" (
    "tag_id"        INT IDENTITY(1000, 1),
    "tag"           VARCHAR(15) NOT NULL,
    "user"          CHAR(24) NOT NULL,
    "created_at"    DATETIME DEFAULT CURRENT_TIMESTAMP

    CONSTRAINT "PK_tags"
        PRIMARY KEY ("tag_id"),

	CONSTRAINT "FK_tags-user_id"
        FOREIGN KEY ("user") REFERENCES [AUTH0_users]("user_id"),

    CONSTRAINT "UQ_tags_no_duplicate_tags" 
        UNIQUE ("tag", "user")
)
GO

-- Create TAG-VALIDATION table
CREATE TABLE "AUTH0_tag_validations" (
    "validation_id" INT IDENTITY(1000, 1),
    "tag"           INT NOT NULL,
    "validated_by"  CHAR(24) NOT NULL,
    "comment"       NVARCHAR(250) COLLATE Latin1_General_100_CI_AI_SC_UTF8,
    "created_at"    DATETIME DEFAULT CURRENT_TIMESTAMP

    CONSTRAINT "PK_tag_validations"
        PRIMARY KEY ("validation_id"),

	CONSTRAINT "FK_tag_validations_tag"
        FOREIGN KEY ("tag") REFERENCES [AUTH0_tags]("tag_id"),

	CONSTRAINT "FK_tag_validations_validated_by"
        FOREIGN KEY ("validated_by") REFERENCES [AUTH0_users]("user_id"),

    CONSTRAINT "UQ_tag_validations_no_duplicate_validations" 
        UNIQUE ("tag", "validated_by")
)
GO

-- Create CONTACTS table
CREATE TABLE "AUTH0_contacts" (
    "user"          CHAR(24) NOT NULL,
    "contact"       CHAR(24) NOT NULL,
    "created_at"    DATETIME DEFAULT CURRENT_TIMESTAMP

	CONSTRAINT "FK_contacts_user"
        FOREIGN KEY ("user") REFERENCES [AUTH0_users]("user_id"),

	CONSTRAINT "FK_contacts_contact"
        FOREIGN KEY ("contact") REFERENCES [AUTH0_users]("user_id"),

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
	    CAST((SELECT CASE WHEN EXISTS(SELECT * FROM [AUTH0_user_data] WHERE [user] = [user_id]) THEN 1 ELSE 0 END) AS BIT) AS registered,
	    [created_at]
    FROM [AUTH0_users]
    LEFT JOIN [AUTH0_user_data] ON [AUTH0_users].[user_id] = [AUTH0_user_data].[user]
    LEFT JOIN [AUTH0_dhbw_domains] ON [AUTH0_users].[email_domain] = [AUTH0_dhbw_domains].[domain]
GO

-- Create ON-USER-DELETE Trigger
CREATE TRIGGER [dbo].[TGR_on_user_delete]
    ON [dbo].[AUTH0_users]
    INSTEAD OF DELETE
AS
    DELETE FROM [dbo].[AUTH0_contacts]
		WHERE 
			[dbo].[AUTH0_contacts].[user] IN(SELECT deleted.[user_id] FROM deleted) OR
			[dbo].[AUTH0_contacts].[contact] IN(SELECT deleted.[user_id] FROM deleted)

	DELETE FROM [dbo].[AUTH0_tag_validations]
		WHERE 
			[dbo].[AUTH0_tag_validations].[validated_by] IN(SELECT deleted.[user_id] FROM deleted)

	DELETE FROM [dbo].[AUTH0_tags]
		WHERE 
			[dbo].[AUTH0_tags].[user] IN(SELECT deleted.[user_id] FROM deleted)

    DELETE FROM [dbo].[AUTH0_user_data]
		WHERE 
			[dbo].[AUTH0_user_data].[user] IN(SELECT deleted.[user_id] FROM deleted)

	DELETE FROM [dbo].[AUTH0_users]
		WHERE 
			[dbo].[AUTH0_users].[user_id] IN(SELECT deleted.[user_id] FROM deleted)
GO

-- Create ON-USER-DATA-DELETE Trigger
CREATE TRIGGER [dbo].[TGR_on_user_data_delete]
    ON [dbo].[AUTH0_user_data]
    INSTEAD OF DELETE
AS
    DELETE FROM [dbo].[AUTH0_contacts]
		WHERE 
			[dbo].[AUTH0_contacts].[user] IN(SELECT deleted.[user] FROM deleted) OR
			[dbo].[AUTH0_contacts].[contact] IN(SELECT deleted.[user] FROM deleted)

	DELETE FROM [dbo].[AUTH0_tag_validations]
		WHERE 
			[dbo].[AUTH0_tag_validations].[validated_by] IN(SELECT deleted.[user] FROM deleted)

	DELETE FROM [dbo].[AUTH0_tags]
		WHERE 
			[dbo].[AUTH0_tags].[user] IN(SELECT deleted.[user] FROM deleted)

    DELETE FROM [dbo].[AUTH0_user_data]
		WHERE 
			[dbo].[AUTH0_user_data].[user] IN(SELECT deleted.[user] FROM deleted)
GO

-- Create ON-TAG-DELETE Trigger
CREATE TRIGGER [dbo].[TGR_on_tag_delete]
    ON [dbo].[AUTH0_tags]
    INSTEAD OF DELETE
AS
    DELETE FROM [dbo].[AUTH0_tag_validations]
		WHERE 
			[dbo].[AUTH0_tag_validations].[tag] IN(SELECT deleted.[tag_id] FROM deleted)

	DELETE FROM [dbo].[AUTH0_tags]
		WHERE 
			[dbo].[AUTH0_tags].[tag_id] IN(SELECT deleted.[tag_id] FROM deleted)
 GO

----------------------------------------------------------
-- OLD TABLES
----------------------------------------------------------

DROP TABLE [CONTACT]
GO
DROP TABLE [TAG-VALIDATION]
GO
DROP TABLE [TAG]
GO
DROP TABLE [USER]
GO
DROP TABLE [DHBW]
GO

-- Create DHBW table
CREATE TABLE "DHBW" (
    "LOCATION"      VARCHAR(30) NOT NULL,
    "EMAIL-DOMAIN"  VARCHAR(30) NOT NULL

	CONSTRAINT "DHBW-PK"
		PRIMARY KEY ("LOCATION")
)
GO

-- Create USER table
CREATE TABLE "USER" (
    "USER-ID"           INT IDENTITY(1000, 1),
    "FIRSTNAME"         VARCHAR(30) NOT NULL,
    "LASTNAME"          VARCHAR(30) NOT NULL,
    "DHBW"              VARCHAR(30) NOT NULL,
    "COURSE-ABR"        VARCHAR(15) NOT NULL,
    "COURSE"            VARCHAR(30) NOT NULL,
    "SPECIALIZATION"    VARCHAR(50),
    "EMAIL-PREFIX"      VARCHAR(50) NOT NULL,
    "CITY"              VARCHAR(30),
    "BIOGRAPHY"         NVARCHAR(1000) COLLATE Latin1_General_100_CI_AI_SC_UTF8,
    "RFID-ID"           VARCHAR(30),
    "PW-HASH"           VARCHAR(30) NOT NULL,
    "IS-VERIFIED"       BIT NOT NULL DEFAULT 0,
    "VERIFICATION-ID"   INT NOT NULL,
    "TMS-CREATED"       DATETIME DEFAULT CURRENT_TIMESTAMP

    CONSTRAINT "USER-PK"
        PRIMARY KEY ("USER-ID"),

	CONSTRAINT "USER-FK-DHBW"
        FOREIGN KEY ("DHBW") REFERENCES [DHBW]("LOCATION"),

	CONSTRAINT "USER-UNIQUE-NO-DUPLICATE-EMAILS"
        UNIQUE ("EMAIL-PREFIX", "DHBW")
)
GO

----------------------------------------------------------

-- Create TAG table
CREATE TABLE "TAG" (
    "TAG-ID"        INT IDENTITY(1000, 1),
    "TAG"           VARCHAR(15) NOT NULL,
    "USER"          INT NOT NULL,
    "TMS-CREATED"   DATETIME DEFAULT CURRENT_TIMESTAMP

    CONSTRAINT "TAG-PK"
        PRIMARY KEY ("TAG-ID"),

	CONSTRAINT "TAG-FK-USER"
        FOREIGN KEY ("USER") REFERENCES [USER]("USER-ID"),

    CONSTRAINT "TAG-UNIQUE-NO-DUPLICATE-TAGS" 
        UNIQUE ("TAG", "USER")
)
GO

-- Create TAG-VALIDATION table
CREATE TABLE "TAG-VALIDATION" (
    "VALIDATION-ID" INT IDENTITY(1000, 1),
    "TAG"           INT NOT NULL,
    "VALIDATED-BY"  INT NOT NULL,
    "COMMENT"       NVARCHAR(250) COLLATE Latin1_General_100_CI_AI_SC_UTF8,
    "TMS-CREATED"   DATETIME DEFAULT CURRENT_TIMESTAMP

    CONSTRAINT "TAG_VALIDATION-PK"
        PRIMARY KEY ("VALIDATION-ID"),

	CONSTRAINT "TAG_VALIDATION-FK-TAG"
        FOREIGN KEY ("TAG") REFERENCES [TAG]("TAG-ID"),

	CONSTRAINT "TAG_VALIDATION-FK-USER"
        FOREIGN KEY ("VALIDATED-BY") REFERENCES [USER]("USER-ID"),

    CONSTRAINT "TAG-VALIDATION-UNIQUE-NO-DUPLICATE-VALIDATIONS" 
        UNIQUE ("TAG", "VALIDATED-BY")
)
GO

-- Create CONTACTS table
CREATE TABLE "CONTACT" (
    "USER"          INT NOT NULL,
    "CONTACT"       INT NOT NULL,
    "TMS-CREATED"   DATETIME DEFAULT CURRENT_TIMESTAMP

	CONSTRAINT "CONTACT-FK-USER"
        FOREIGN KEY ("USER") REFERENCES [USER]("USER-ID"),

	CONSTRAINT "CONTACT-FK-USER_CONTACT"
        FOREIGN KEY ("CONTACT") REFERENCES [USER]("USER-ID"),

    CONSTRAINT "CONTACT-UNIQUE-NO-DUPLICATE-CONTACTS"
        PRIMARY KEY ("USER", "CONTACT")
)
GO

-- Create ON-USER-DELETE Trigger
CREATE TRIGGER [dbo].[ON-USER-DELETE]
    ON [dbo].[USER]
    INSTEAD OF DELETE
AS
    DELETE FROM [dbo].[CONTACT]
		WHERE 
			[dbo].[CONTACT].[USER] IN(SELECT deleted.[USER-ID] FROM deleted) OR
			[dbo].[CONTACT].[CONTACT] IN(SELECT deleted.[USER-ID] FROM deleted)

	DELETE FROM [dbo].[TAG-VALIDATION]
		WHERE 
			[dbo].[TAG-VALIDATION].[VALIDATED-BY] IN(SELECT deleted.[USER-ID] FROM deleted)

	DELETE FROM [dbo].[TAG]
		WHERE 
			[dbo].[TAG].[USER] IN(SELECT deleted.[USER-ID] FROM deleted)

	DELETE FROM [dbo].[USER]
		WHERE 
			[dbo].[USER].[USER-ID] IN(SELECT deleted.[USER-ID] FROM deleted)
GO

-- Create ON-TAG-DELETE Trigger
CREATE TRIGGER [dbo].[ON-TAG-DELETE]
    ON [dbo].[TAG]
    INSTEAD OF DELETE
AS
    DELETE FROM [dbo].[TAG-VALIDATION]
		WHERE 
			[dbo].[TAG-VALIDATION].[TAG] IN(SELECT deleted.[TAG-ID] FROM deleted)

	DELETE FROM [dbo].[TAG]
		WHERE 
			[dbo].[TAG].[TAG-ID] IN(SELECT deleted.[TAG-ID] FROM deleted)
 GO

 -- Create ON-DHBW-DELETE Trigger
CREATE TRIGGER [dbo].[ON-DHBW-DELETE]
    ON [dbo].[DHBW]
    INSTEAD OF DELETE
AS
    DELETE FROM [dbo].[USER]
		WHERE 
			[dbo].[USER].[DHBW] IN(SELECT deleted.[LOCATION] FROM deleted)

	DELETE FROM [dbo].[DHBW]
		WHERE 
			[dbo].[DHBW].[LOCATION] IN(SELECT deleted.[LOCATION] FROM deleted)