# DHBW-Experts Database API

This repository contains the source code of the DHBW-Experts Database API.

## Setup

Make sure that the following components are installed in your system:

1. dotNET Framework(Verify by running `dotnet --version`)
2. Visual Studio Code IDE
3. VSCode C# Extension (ID: `ms-dotnettools.csharp`)

When you are done, run `dotnet nuget add source --name nuget.org https://api.nuget.org/v3/index.json` in this directory to make the IDE compatible

### press `F5` to start the Server

API can be reached by Postman or VSCode Thunder Client via http://localhost:5000. (Not yet HTTPS compatible!)

## Interfaces

### <b>Register a new User</b>
`/register`</br>
Note: Parameters in body must be sorted alphabetically!
| Method | Body | Response |
|--------|------|----------|
| POST | {<br>&emsp;"bio": "Hello World",<br>&emsp;"city": "Musters",<br>&emsp;"course": "Informatik",<br>&emsp;"courseAbr": "TINF20B2",<br>&emsp;"dhbw": "Karlsruhe",<br>&emsp;"email": "doe.john",<br>&emsp;"firstname": "John",<br>&emsp;"lastname": "Doe",<br>&emsp;"rfidid": null,<br>&emsp;"pwhash": "EXAMPLE"<br>} | `201 CREATED`<br>{<br>&emsp;"userId": 1028,<br>&emsp;"firstname": "John",<br>&emsp;"lastname": "Doe",<br>&emsp;"dhbw": "Karlsruhe",<br>&emsp;"courseAbr": "TINF20B2",<br>&emsp;"course": "Informatik",<br>&emsp;"email": "doe.john@student.dhbw-karlsruhe.de",<br>&emsp;"city": "Example City",<br>&emsp;"bio": "Hello World",<br>&emsp;"isVerified": false,<br>&emsp;"tmsCreated":<br>&emsp;"2021-11-01T15:25:56.67"<br>} |

### Verify a new User
`/register/{USER-ID}/{VERIFICATION-ID}`</br>
| Method | Body | Response |
|--------|------|----------|
| PUT | -/- | `200 OK`<br>{<br>&emsp;"userId": 1028,<br>&emsp;"firstname": "John",<br>&emsp;"lastname": "Doe",<br>&emsp;"dhbw": "Karlsruhe",<br>&emsp;"courseAbr": "TINF20B2",<br>&emsp;"course": "Informatik",<br>&emsp;"email": "doe.john@student.dhbw-karlsruhe.de",<br>&emsp;"city": "Example City",<br>&emsp;"bio": "Hello World",<br>&emsp;"isVerified": false,<br>&emsp;"tmsCreated":<br>&emsp;"2021-11-01T15:25:56.67"<br>} |

### Get user by ID
`/users/id/{USER-ID}`</br>
Info: RFID-IDs are considered sensitive information and won't be returned
| Method | Body | Response |
|--------|------|----------|
| GET | -/- | `200 OK`<br>{<br>&emsp;"userId": 1028,<br>&emsp;"firstname": "John",<br>&emsp;"lastname": "Doe",<br>&emsp;"dhbw": "Karlsruhe",<br>&emsp;"courseAbr": "TINF20B2",<br>&emsp;"course": "Informatik",<br>&emsp;"email": "doe.john@student.dhbw-karlsruhe.de",<br>&emsp;"city": "Example City",<br>&emsp;"bio": "Hello World",<br>&emsp;"isVerified": false,<br>&emsp;"tmsCreated":<br>&emsp;"2021-11-01T15:25:56.67"<br>} |

### Get user by RFID-ID
`/users/rfid/{RFID-ID}`</br>
| Method | Body | Response |
|--------|------|----------|
| GET | -/- | `200 OK`<br>{<br>&emsp;"userId": 1028,<br>&emsp;"firstname": "John",<br>&emsp;"lastname": "Doe",<br>&emsp;"dhbw": "Karlsruhe",<br>&emsp;"courseAbr": "TINF20B2",<br>&emsp;"course": "Informatik",<br>&emsp;"email": "doe.john@student.dhbw-karlsruhe.de",<br>&emsp;"city": "Example City",<br>&emsp;"bio": "Hello World",<br>&emsp;"isVerified": false,<br>&emsp;"tmsCreated":<br>&emsp;"2021-11-01T15:25:56.67"<br>} |
