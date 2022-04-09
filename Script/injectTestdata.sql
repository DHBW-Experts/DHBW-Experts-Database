/*
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
*/
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
        'mustermann.max',
        'Ettlingen',
        'Hello World',
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
        'mustermann.moritz',
        'Durlach',
        'Hello World',
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
        'mustermann.erika',
        'Karlsruhe',
        'Hello World',
        'EXAMPLE-HASH',
        1,
        123456
        ),
        (
        'Ulrich', 
        'Unverifiziert', 
        'Karlsruhe', 
        'TINF20B2', 
        'Elektrotechnik', 
        'unverifiziert.ulrich',
        'Karlsruhe',
        N'EMOJI 💩',
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
INSERT INTO [CONTACT] ([USER], [CONTACT])
    VALUES 
        (1000, 1001),
        (1000, 1002),
        (1001, 1000),
        (1002, 1000),
        (1002, 1001)
GO