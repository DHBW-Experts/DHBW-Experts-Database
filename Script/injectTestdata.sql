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
        N'Emojis sind 🆒',
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
        (1002, 1002, N'Hat in theoretischer Informatik eine 1.0 geschrieben 😯')
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