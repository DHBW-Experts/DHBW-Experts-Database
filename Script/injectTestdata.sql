USE [DHBW-Experts-database]

DELETE FROM [contacts]
GO
DELETE FROM [tag_validations]
DBCC CHECKIDENT ('[tag_validations]', RESEED, 999);
GO
DELETE FROM [tags]
DBCC CHECKIDENT ('[tags]', RESEED, 999);
GO
DELETE FROM [user_data]
GO
DELETE FROM [users]
GO
DELETE FROM [dhbw_domains]
GO
-----------------------------------
INSERT INTO [dhbw_domains] (
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
INSERT INTO [users] (
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
        ),
        (
        '627a158214461c006881a509',
        'unregistriert.ulrike',
        'student.dhbw-karlsruhe.de',
        '2022-05-10 09:34:26.023'
        ),
        (
        '62aa4b4870857b0a5a5fb053',
        'boehm.ralph',
        'student.dhbw-karlsruhe.de',
        '2022-06-19 09:34:26.023'
        ),
        (
        '627b8f4fdf614a006fa372d1',
        'holler.lukas',
        'student.dhbw-karlsruhe.de',
        '2022-05-11 12:26:23.772'
        )
GO

INSERT INTO [user_data] (
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
        '1A'
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
        '2B'
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
        '3C'
        ),
        (
        '62aa4b4870857b0a5a5fb053',
        'Ralph', 
        'Boehm',
        'TINF20B2', 
        'Informatik',
        null,
        'Karlsruhe',
        N'Software-Entwickler bei Atruvia, Schwerpunkt Angular',
        '04:25:10:5A:1B:5C:80'
        ),
        (
        '627b8f4fdf614a006fa372d1',
        'Lukas', 
        'Holler',
        'TINF20B2', 
        'Informatik',
        null,
        'Ettlingen',
        N'Grüße aus dem Dev-Team!',
        null
        )
GO
--------------------------------
INSERT INTO [tags] ([tag], [user])
    VALUES 
        ('LaTeX',       '626db5fc4105f20069997435'),
        ('Algebra',     '626db5fc4105f20069997435'),
        ('TheoInf',     '626db5fc4105f20069997435'), 
        ('LaTeX',       '626db6676c48dc006a2dcb17'),
        ('Ionic',       '626db6676c48dc006a2dcb17'),
        ('AdA',         '626db6676c48dc006a2dcb17'),
        ('TypeScript',  '626db6963b37a6006f7a7174'),
        ('NodeJS',      '626db6963b37a6006f7a7174'),
        ('SQL',         '626db6963b37a6006f7a7174'),
        ('Angular',     '62aa4b4870857b0a5a5fb053'),
        ('Ionic',     '62aa4b4870857b0a5a5fb053'),
        ('Java',     '62aa4b4870857b0a5a5fb053'),
        ('CS:GO',     '62aa4b4870857b0a5a5fb053')
GO
---------------------------------
INSERT INTO [tag_validations] ([tag], [validated_by], [comment])
    VALUES 
        (1000, '626db6676c48dc006a2dcb17', N'Kann er wirklich, habs gesehen!'),
        (1002, '626db6963b37a6006f7a7174', N'Hat in theoretischer Informatik eine 1.0 geschrieben'),
        (1012, '626db6676c48dc006a2dcb17', N'+rep sehr guter Spieler')
GO
---------------------------------
INSERT INTO [contacts] ([user], [contact])
    VALUES 
        ('626db5fc4105f20069997435', '626db6676c48dc006a2dcb17'),
        ('626db5fc4105f20069997435', '626db6963b37a6006f7a7174'),
        ('626db6676c48dc006a2dcb17', '626db5fc4105f20069997435'),
        ('626db6963b37a6006f7a7174', '626db5fc4105f20069997435'),
        ('626db6963b37a6006f7a7174', '626db6676c48dc006a2dcb17')
GO
