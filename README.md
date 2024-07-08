This project is a technical challenge

Tools
IDE: JetBrains Rider
.NET: Core 8.0
Database: PostegresSQL
SO: ZorinOS

Configure Database

1) Install Postegresql RDBMS
2) Create User and Password
3) Create Database Library
4) Grant all priviliges to UserCreted on Library

Obs.: i created a method to upload a cover using S3 from AWS Services, at first it worked but after push to the repo Amazon created a security issue on my account because the AccessKey and AccessSecretKey were visible on appSettings.json file, so for fix the issue was necessary to delete all keys, user to restore my aws account.
tried two different approaches, one using Secrets Manager for storing the keys and other setting the keys using AWS CLI on my computer but both approaches were not succeeded.
All these approaches are on git commit for evaluation.

