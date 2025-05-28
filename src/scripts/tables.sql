use master
 
if DB_ID('ApplicationControl') is null 
    create database ApplicationControl
go

use ApplicationControl 

if not exists (select * from ApplicationControl.sys.schemas where name = 'ApplicationControl')
    exec ('create schema ApplicationControl')
go

if OBJECT_ID('ApplicationControl.Application', 'U') is  null
begin
    create table ApplicationControl.[Application] (
        Id UNIQUEIDENTIFIER DEFAULT NEWID() not null,
        ApplicationName   VARCHAR(512)  null,
        AddedDateTime DATETIME DEFAULT GETDATE(),
        AddedBy VARCHAR(255) NOT NULL,
        UpdatedDateTime DATETIME null,
        UpdatedBy VARCHAR(255) null,
        [Status] int not null default 0,
        [Description] VARCHAR(MAX) null, 
        [IsDeleted] bit null default 0, 
        CONSTRAINT [PK_Application_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
end

go 

if OBJECT_ID('ApplicationControl.ApplicationJob', 'U') is  null
begin
    create table ApplicationControl.[ApplicationJob] (
        Id UNIQUEIDENTIFIER DEFAULT NEWID() not null,
        ApplicationId   UNIQUEIDENTIFIER  not null,
        Command  VARCHAR(512) not null,
        AddedDateTime DATETIME DEFAULT GETDATE(),
        AddedBy VARCHAR(255) NOT NULL,
        UpdatedDateTime DATETIME null,
        UpdatedBy VARCHAR(255) null,
        [Status] int not null default 0,
        [Description] VARCHAR(MAX) null, 
        [IsDeleted] bit null default 0, 
        CONSTRAINT [PK_ApplicationJob_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
end

go

if OBJECT_ID('ApplicationControl.QueuedApplicationJob', 'U') is  null
begin   
    create table ApplicationControl.QueuedApplicationJob (
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
        CONSTRAINT [PK_QueuedApplicationJob_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
end

go
if OBJECT_ID('ApplicationControl.ScheduledApplicationJob', 'U') is  null
begin   
    create table ApplicationControl.ScheduledApplicationJob (
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
        CONSTRAINT [PK_ScheduledApplicationJob_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
end