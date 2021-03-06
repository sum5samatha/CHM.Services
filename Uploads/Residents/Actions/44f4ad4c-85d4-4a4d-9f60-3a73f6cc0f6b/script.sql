USE [CHM]
GO
/****** Object:  Table [dbo].[PainMonitoring]    Script Date: 12/27/2016 12:07:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PainMonitoring](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_PainMonitoring_ID]  DEFAULT (newid()),
	[ResidentID] [uniqueidentifier] NOT NULL,
	[OrganizationID] [uniqueidentifier] NOT NULL,
	[PartsID] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[Modified] [datetime] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NULL,
 CONSTRAINT [PK_PainMonitoring] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Residents]    Script Date: 12/27/2016 12:07:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Residents](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Residents_ID]  DEFAULT (newid()),
	[OrganizationID] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](250) NOT NULL,
	[LastName] [nvarchar](250) NULL,
	[NickName] [nvarchar](250) NULL,
	[Gender] [char](1) NOT NULL,
	[DOB] [date] NOT NULL,
	[DOJ] [date] NULL,
	[Telephone] [nvarchar](20) NULL,
	[Mobile] [nvarchar](20) NULL,
	[GPDetails] [nvarchar](max) NULL,
	[Nok] [nvarchar](250) NULL,
	[NokTelephoneNumber] [nvarchar](20) NULL,
	[NokAddress] [nvarchar](max) NULL,
	[NokPreferred] [nvarchar](50) NULL,
	[SocialWorker] [nvarchar](250) NULL,
	[ReasonForAdmission] [nvarchar](max) NULL,
	[IsAccepted] [bit] NULL,
	[IsActive] [bit] NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF__Residents__Creat__412EB0B6]  DEFAULT (getdate()),
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[Modified] [datetime] NOT NULL CONSTRAINT [DF__Residents__Modif__4222D4EF]  DEFAULT (getdate()),
	[ModifiedBy] [uniqueidentifier] NOT NULL,
	[AdmittedFrom] [date] NULL,
	[NHS] [nvarchar](20) NULL,
	[MedicalHistory] [nvarchar](max) NULL,
 CONSTRAINT [PK_Residents] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sections]    Script Date: 12/27/2016 12:07:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sections](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Sections_ID]  DEFAULT (newid()),
	[Name] [nvarchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF__Sections__Create__2E1BDC42]  DEFAULT (getdate()),
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[Modified] [datetime] NOT NULL CONSTRAINT [DF__Sections__Modifi__2F10007B]  DEFAULT (getdate()),
	[ModifiedBy] [uniqueidentifier] NOT NULL,
	[HasScore] [bit] NULL,
 CONSTRAINT [PK_Sections] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[PainMonitoring]  WITH CHECK ADD  CONSTRAINT [FK_PainMonitoring_Organizations] FOREIGN KEY([OrganizationID])
REFERENCES [dbo].[Organizations] ([ID])
GO
ALTER TABLE [dbo].[PainMonitoring] CHECK CONSTRAINT [FK_PainMonitoring_Organizations]
GO
ALTER TABLE [dbo].[PainMonitoring]  WITH CHECK ADD  CONSTRAINT [FK_PainMonitoring_Residents] FOREIGN KEY([ResidentID])
REFERENCES [dbo].[Residents] ([ID])
GO
ALTER TABLE [dbo].[PainMonitoring] CHECK CONSTRAINT [FK_PainMonitoring_Residents]
GO
ALTER TABLE [dbo].[PainMonitoring]  WITH CHECK ADD  CONSTRAINT [FK_PainMonitoring_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[PainMonitoring] CHECK CONSTRAINT [FK_PainMonitoring_Users]
GO
ALTER TABLE [dbo].[PainMonitoring]  WITH CHECK ADD  CONSTRAINT [FK_PainMonitoring_Users1] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[PainMonitoring] CHECK CONSTRAINT [FK_PainMonitoring_Users1]
GO
ALTER TABLE [dbo].[Residents]  WITH CHECK ADD  CONSTRAINT [FK_Residents_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Residents] CHECK CONSTRAINT [FK_Residents_CreatedBy]
GO
ALTER TABLE [dbo].[Residents]  WITH CHECK ADD  CONSTRAINT [FK_Residents_ModifiedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Residents] CHECK CONSTRAINT [FK_Residents_ModifiedBy]
GO
ALTER TABLE [dbo].[Residents]  WITH CHECK ADD  CONSTRAINT [FK_Residents_Organizations] FOREIGN KEY([OrganizationID])
REFERENCES [dbo].[Organizations] ([ID])
GO
ALTER TABLE [dbo].[Residents] CHECK CONSTRAINT [FK_Residents_Organizations]
GO
ALTER TABLE [dbo].[Sections]  WITH NOCHECK ADD  CONSTRAINT [FK_Sections_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Sections] CHECK CONSTRAINT [FK_Sections_CreatedBy]
GO
ALTER TABLE [dbo].[Sections]  WITH NOCHECK ADD  CONSTRAINT [FK_Sections_ModifiedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Sections] CHECK CONSTRAINT [FK_Sections_ModifiedBy]
GO
