CREATE TABLE roles (
  Id nvarchar(128) NOT NULL,
  Name nvarchar(256) NOT NULL,
  PRIMARY KEY (Id)
);
GO

CREATE TABLE users (
  Id nvarchar(128) NOT NULL,
  Email nvarchar(256) DEFAULT NULL,
  EmailConfirmed bit NOT NULL,
  PasswordHash NVARCHAR(100),
  SecurityStamp NVARCHAR(100),
  PhoneNumber NVARCHAR(100),
  PhoneNumberConfirmed bit NOT NULL,
  TwoFactorEnabled bit NOT NULL,
  LockoutEndDateUtc datetime DEFAULT NULL,
  LockoutEnabled bit NOT NULL,
  AccessFailedCount int NOT NULL,
  UserName nvarchar(128) NOT NULL,
  PRIMARY KEY (Id),
  CONSTRAINT users_username UNIQUE(UserName)
);
GO

CREATE TABLE userclaims (
  Id INT IDENTITY(1,1) NOT NULL,
  UserId nvarchar(128) NOT NULL,
  ClaimType NVARCHAR(100),
  ClaimValue NVARCHAR(100),
  ClaimValueType NVARCHAR(100),
  PRIMARY KEY (Id),
  CONSTRAINT FK_userclaims_users FOREIGN KEY (UserId) REFERENCES users (Id) ON DELETE CASCADE ON UPDATE NO ACTION
);
GO

CREATE TABLE userlogins (
  LoginProvider nvarchar(128) NOT NULL,
  ProviderKey nvarchar(128) NOT NULL,
  UserId nvarchar(128) NOT NULL,
  PRIMARY KEY (LoginProvider,ProviderKey,UserId),
  CONSTRAINT FK_userlogins_users FOREIGN KEY (UserId) REFERENCES users (Id) ON DELETE CASCADE ON UPDATE NO ACTION
);
GO

CREATE TABLE userroles (
  UserId nvarchar(128) NOT NULL,
  RoleId nvarchar(128) NOT NULL,
  PRIMARY KEY (UserId,RoleId),
  CONSTRAINT FK_userroles_users FOREIGN KEY (UserId) REFERENCES users (Id) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT FK_userroles_roles FOREIGN KEY (RoleId) REFERENCES roles (Id) ON DELETE CASCADE ON UPDATE NO ACTION
) ;
GO
