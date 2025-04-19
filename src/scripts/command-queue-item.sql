use master
 
if DB_ID('ApplicationControl') is null 
    create database ApplicationControl
go

use ApplicationControl 

if not exists (select * from ApplicationControl.sys.schemas where name = 'ApplicationControl')
    exec ('create schema ApplicationControl')
go

create table [ApplicationControl].[CommandQueueItem] (
    Id UNIQUEIDENTIFIER DEFAULT NEWID() not null,
    Command   VARCHAR(512) not null,
    ApplicationName   VARCHAR(512)  null,
    ApplicationId   UNIQUEIDENTIFIER  not null,
    AddedDateTime DATETIME DEFAULT GETDATE(),
    AddedBy VARCHAR(255) NOT NULL,
    UpdatedDateTime DATETIME null,
    UpdatedBy VARCHAR(255) null,
    [Status] int not null default 0,
    [Message] VARCHAR(MAX) null, 
    [IsDeleted] bit null default 0, 
    CONSTRAINT [PK_EmailQueue_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);