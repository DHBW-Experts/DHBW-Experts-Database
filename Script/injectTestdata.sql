DELETE FROM [AUTH0_contacts]
GO
DELETE FROM [AUTH0_tag_validations]
DBCC CHECKIDENT ('[AUTH0_tag_validations]', RESEED, 999);
GO
DELETE FROM [AUTH0_tags]
DBCC CHECKIDENT ('[AUTH0_tags]', RESEED, 999);
GO
DELETE FROM [AUTH0_user_data]
GO
DELETE FROM [AUTH0_users]
GO
DELETE FROM [AUTH0_dhbw_domains]
GO
-----------------------------------
DELETE FROM [CONTACT]
GO
DELETE FROM [TAG-VALIDATION]
DBCC CHECKIDENT ('[TAG-VALIDATION]', RESEED, 999);
GO
DELETE FROM [TAG]
DBCC CHECKIDENT ('[TAG]', RESEED, 999);
GO
DELETE FROM [USER]
DBCC CHECKIDENT ('[USER]', RESEED, 999);
GO
DELETE FROM [DHBW]
GO
-----------------------------------
INSERT INTO [AUTH0_dhbw_domains] (
        [DOMAIN],
        [LOCATION]
    )
    VALUES (
        
        'student.dhbw-karlsruhe.de',
        'Karlsruhe'
    ),
    (
        
        'example.com',
        'Karlsruhe'
    )
GO
-----------------------------------
INSERT INTO [AUTH0_users] (
        [user_id],
        [email_prefix],
        [email_domain],
        [created_at]
        )
    VALUES (
        '626db5fc4105f20069997435',
        'mustermann.max',
        'student.dhbw-karlsruhe.de',
        '2022-04-30 22:19:40.413'
        ),
        (
        '626db6676c48dc006a2dcb17',  
        'mustermann.moritz',
        'student.dhbw-karlsruhe.de',
        '2022-04-30 22:21:27.900'
        ),
        (
        '626db6963b37a6006f7a7174',
        'mustermann.erika',
        'student.dhbw-karlsruhe.de',
        '2022-04-30 22:22:14.980'
        ),
        (
        '626db6d81d742f006f2a9fc7',
        'unverifiziert.ulrich',
        'student.dhbw-karlsruhe.de',
        '2022-04-30 22:23:21.170'
        )
GO

INSERT INTO [AUTH0_user_data] (
        [user],
        [firstname],
        [lastname],
        [course_abbr],
        [course],
        [specialization],
        [city],
        [biography],
        [rfid_id]
        )
    VALUES (
        '626db5fc4105f20069997435',
        'Max', 
        'Mustermann',
        'TINF20B2', 
        'Informatik',
        null,
        'Ettlingen',
        N'Hello World',
        123456
        ),
        (
        '626db6676c48dc006a2dcb17',
        'Moritz', 
        'Mustermann', 
        'TINF20B2', 
        'Wirtschaftsinformatik',
        'Sales and Consulting',
        'Durlach',
        N'Hello World',
        123456
        ),
        (
        '626db6963b37a6006f7a7174',
        'Erika', 
        'Mustermann',
        'TINF20B2', 
        'Elektrotechnik',
        null,
        'Karlsruhe',
        N'Hello World',
        123456
        )
GO
--------------------------------
INSERT INTO [AUTH0_tags] ([tag], [user])
    VALUES 
        ('LaTeX',       '626db5fc4105f20069997435'),
        ('Algebra',     '626db5fc4105f20069997435'),
        ('TheoInf',     '626db5fc4105f20069997435'), 
        ('LaTeX',       '626db6676c48dc006a2dcb17'),
        ('Ionic',       '626db6676c48dc006a2dcb17'),
        ('AdA',         '626db6676c48dc006a2dcb17'),
        ('TypeScript',  '626db6963b37a6006f7a7174'),
        ('NodeJS',      '626db6963b37a6006f7a7174'),
        ('SQL',         '626db6963b37a6006f7a7174')
GO
---------------------------------
INSERT INTO [AUTH0_tag_validations] ([tag], [validated_by], [comment])
    VALUES 
        (1000, '626db6676c48dc006a2dcb17', N'Kann er wirklich, habs gesehen!'),
        (1002, '626db6963b37a6006f7a7174', N'Hat in theoretischer Informatik eine 1.0 geschrieben')
GO
---------------------------------
INSERT INTO [AUTH0_contacts] ([user], [contact])
    VALUES 
        ('626db5fc4105f20069997435', '626db6676c48dc006a2dcb17'),
        ('626db5fc4105f20069997435', '626db6963b37a6006f7a7174'),
        ('626db6676c48dc006a2dcb17', '626db5fc4105f20069997435'),
        ('626db6963b37a6006f7a7174', '626db5fc4105f20069997435'),
        ('626db6963b37a6006f7a7174', '626db6676c48dc006a2dcb17')
GO

----------------------------------------------------------
-- OLD TABLES
----------------------------------------------------------

INSERT INTO [DHBW] (
        [LOCATION],
        [EMAIL-DOMAIN]
    )
    VALUES (
        'Karlsruhe',
        'student.dhbw-karlsruhe.de'
    )
GO
-----------------------------------------
INSERT INTO [USER] (
        [FIRSTNAME],
        [LASTNAME],
        [DHBW],
        [COURSE-ABR],
        [COURSE],
        [SPECIALIZATION],
        [EMAIL-PREFIX],
        [CITY],
        [BIOGRAPHY],
        [PW-HASH],
        [IS-VERIFIED],
        [VERIFICATION-ID]
        )
    VALUES (
        'Max', 
        'Mustermann', 
        'Karlsruhe', 
        'TINF20B2', 
        'Informatik',
        null,  
        'mustermann.max',
        'Ettlingen',
        N'Hello World',
        'EXAMPLE-HASH',
        1,
        123456
        ),
        (
        'Moritz', 
        'Mustermann', 
        'Karlsruhe', 
        'TINF20B2', 
        'Wirtschaftsinformatik',
        'Sales and Consulting',   
        'mustermann.moritz',
        'Durlach',
        N'Hello World',
        'EXAMPLE-HASH',
        1,
        123456
        ),
        (
        'Erika', 
        'Mustermann', 
        'Karlsruhe', 
        'TINF20B2', 
        'Elektrotechnik',
        null,   
        'mustermann.erika',
        'Karlsruhe',
        N'Hello World',
        'EXAMPLE-HASH',
        1,
        123456
        ),
        (
        'Ulrich', 
        'Unverifiziert', 
        'Karlsruhe', 
        'TINF20B2', 
        'BWL',
        'Bank',   
        'unverifiziert.ulrich',
        'Rheinstetten',
        N'Hello World',
        'EXAMPLE-HASH',
        0,
        123456
        )
GO
--------------------------------
INSERT INTO [TAG] ([TAG], [USER])
    VALUES 
        ('LaTeX',       1000),
        ('Algebra',     1000),
        ('TheoInf',     1000), 
        ('LaTeX',       1001),
        ('Ionic',       1001),
        ('AdA',         1001),
        ('TypeScript',  1002),
        ('NodeJS',      1002),
        ('SQL',         1002)
GO
---------------------------------
INSERT INTO [TAG-VALIDATION] ([TAG], [VALIDATED-BY], [COMMENT])
    VALUES 
        (1000, 1001, N'Kann er wirklich, habs gesehen!'),
        (1002, 1002, N'Hat in theoretischer Informatik eine 1.0 geschrieben')
GO
---------------------------------
INSERT INTO [CONTACT] ([USER], [CONTACT])
    VALUES 
        (1000, 1001),
        (1000, 1002),
        (1001, 1000),
        (1002, 1000),
        (1002, 1001)
GO