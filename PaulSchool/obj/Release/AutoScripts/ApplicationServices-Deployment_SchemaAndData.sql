SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CommissioningRequirements](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CoreCoursesRequired] [int] NOT NULL,
	[ElectiveCoursesRequired] [int] NOT NULL,
 CONSTRAINT [PK_CommissioningRequirements] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET IDENTITY_INSERT [dbo].[CommissioningRequirements] ON 

GO
INSERT [dbo].[CommissioningRequirements] ([Id], [CoreCoursesRequired], [ElectiveCoursesRequired]) VALUES (1, 6, 2)
GO
SET IDENTITY_INSERT [dbo].[CommissioningRequirements] OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[NotificationID] [int] IDENTITY(1,1) NOT NULL,
	[Time] [datetime] NOT NULL,
	[Details] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Link] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ViewableBy] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Complete] [bit] NOT NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[NotificationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Instructor](
	[InstructorID] [int] IDENTITY(1,1) NOT NULL,
	[LastName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FirstMidName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Email] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[EnrollmentDate] [datetime] NOT NULL,
	[UserName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_Instructor] PRIMARY KEY CLUSTERED 
(
	[InstructorID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseTemplates](
	[CourseTemplatesID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Credits] [int] NOT NULL,
	[Elective] [bit] NOT NULL,
	[AttendingDays] [int] NOT NULL,
	[AttendanceCap] [int] NOT NULL,
	[DurationHours] [int] NOT NULL,
	[DurationMins] [int] NOT NULL,
	[Location] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Parish] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Cost] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_CourseTemplates] PRIMARY KEY CLUSTERED 
(
	[CourseTemplatesID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET IDENTITY_INSERT [dbo].[CourseTemplates] ON 

GO
INSERT [dbo].[CourseTemplates] ([CourseTemplatesID], [Title], [Credits], [Elective], [AttendingDays], [AttendanceCap], [DurationHours], [DurationMins], [Location], [Parish], [Description], [Cost]) VALUES (1, N'Custom (edit title)', 3, 0, 10, 30, 0, 0, NULL, NULL, NULL, CAST(10.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[CourseTemplates] ([CourseTemplatesID], [Title], [Credits], [Elective], [AttendingDays], [AttendanceCap], [DurationHours], [DurationMins], [Location], [Parish], [Description], [Cost]) VALUES (2, N'Prayer', 3, 0, 10, 30, 1, 30, N'Corpus Christi', N'GoodParish', N'A course taken in Corpus Christi', CAST(10.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[CourseTemplates] ([CourseTemplatesID], [Title], [Credits], [Elective], [AttendingDays], [AttendanceCap], [DurationHours], [DurationMins], [Location], [Parish], [Description], [Cost]) VALUES (3, N'Spirituality', 3, 0, 10, 30, 1, 30, N'Corpus Christi', N'GoodParish', N'A course taken in Corpus Christi', CAST(10.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[CourseTemplates] ([CourseTemplatesID], [Title], [Credits], [Elective], [AttendingDays], [AttendanceCap], [DurationHours], [DurationMins], [Location], [Parish], [Description], [Cost]) VALUES (4, N'Macroeconomics', 3, 1, 8, 30, 1, 30, N'Corpus Christi', N'GoodParish', N'A course taken in Corpus Christi', CAST(10.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[CourseTemplates] ([CourseTemplatesID], [Title], [Credits], [Elective], [AttendingDays], [AttendanceCap], [DurationHours], [DurationMins], [Location], [Parish], [Description], [Cost]) VALUES (5, N'Calculus', 1, 1, 3, 25, 1, 30, N'Corpus Christi', N'GoodParish', N'A course taken in Corpus Christi', CAST(10.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[CourseTemplates] ([CourseTemplatesID], [Title], [Credits], [Elective], [AttendingDays], [AttendanceCap], [DurationHours], [DurationMins], [Location], [Parish], [Description], [Cost]) VALUES (6, N'Trigonometry', 1, 1, 8, 25, 1, 30, N'Corpus Christi', N'GoodParish', N'A course taken in Corpus Christi', CAST(10.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[CourseTemplates] ([CourseTemplatesID], [Title], [Credits], [Elective], [AttendingDays], [AttendanceCap], [DurationHours], [DurationMins], [Location], [Parish], [Description], [Cost]) VALUES (7, N'Day of Reflection', 1, 0, 1, 1, 24, 0, N'Corpus Christi', N'GoodParish', N'Required for Commissioning', CAST(0.00 AS Decimal(18, 2)))
GO
SET IDENTITY_INSERT [dbo].[CourseTemplates] OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[StudentID] [int] IDENTITY(1,1) NOT NULL,
	[LastName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FirstMidName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Email] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[StreetAddress] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[City] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[State] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ZipCode] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Phone] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DateOfBirth] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ParishAffiliation] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MinistryInvolvement] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[EnrollmentDate] [datetime] NOT NULL,
	[UserName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET IDENTITY_INSERT [dbo].[Student] ON 

GO
INSERT [dbo].[Student] ([StudentID], [LastName], [FirstMidName], [Email], [StreetAddress], [City], [State], [ZipCode], [Phone], [DateOfBirth], [ParishAffiliation], [MinistryInvolvement], [EnrollmentDate], [UserName]) VALUES (1, N'Admin', N'First', N'as@email.com', N'123 Fake', N'Yep', N'Texas', N'89798', N'1230293293', N'12/12/1212', N'none', N'nonez', CAST(0x0000A0A900645A83 AS DateTime), N'Admin')
GO
INSERT [dbo].[Student] ([StudentID], [LastName], [FirstMidName], [Email], [StreetAddress], [City], [State], [ZipCode], [Phone], [DateOfBirth], [ParishAffiliation], [MinistryInvolvement], [EnrollmentDate], [UserName]) VALUES (2, N'AdminSuper', N'Super', N'super@gmeila.com', N'123 Fake', N'City', N'State', N'78515', N'1231231232', N'08/11/1111', NULL, NULL, CAST(0x0000A0A900647CBC AS DateTime), N'SuperAdmin')
GO
INSERT [dbo].[Student] ([StudentID], [LastName], [FirstMidName], [Email], [StreetAddress], [City], [State], [ZipCode], [Phone], [DateOfBirth], [ParishAffiliation], [MinistryInvolvement], [EnrollmentDate], [UserName]) VALUES (3, N'Robertson', N'Jake', N'asdfljkasdf@email.com', N'123 Real', N'AlmondVille', N'Texas', N'78335', N'2105556876', N'05/09/1946', N'All Parish', N'One Ministry', CAST(0x0000A0A900649533 AS DateTime), N'Student1')
GO
SET IDENTITY_INSERT [dbo].[Student] OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecommendationForCommissioning](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RecommendersFirstName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RecommendersMidName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RecommendersLastName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ActiveInRecommendersParish] [bit] NOT NULL,
	[Experienced] [bit] NOT NULL,
	[ExhibitsUnderstanding] [bit] NOT NULL,
	[RecommendersThoughts] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SignedByRecommender] [bit] NOT NULL,
	[SignedByApplicant] [bit] NOT NULL,
	[DateFiled] [datetime] NOT NULL,
	[Student_StudentID] [int] NULL,
 CONSTRAINT [PK_RecommendationForCommissioning] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO

CREATE NONCLUSTERED INDEX [IX_Student_StudentID] ON [dbo].[RecommendationForCommissioning] 
(
	[Student_StudentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[CourseID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Credits] [int] NOT NULL,
	[Elective] [bit] NOT NULL,
	[InstructorID] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[AttendingDays] [int] NOT NULL,
	[AttendanceCap] [int] NOT NULL,
	[SeatsTaken] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[DurationHours] [int] NOT NULL,
	[DurationMins] [int] NOT NULL,
	[Location] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Parish] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Cost] [decimal](18, 2) NOT NULL,
	[AdminDenialReason] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Approved] [bit] NOT NULL,
	[Completed] [bit] NOT NULL,
	[Archived] [bit] NOT NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[CourseID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO

CREATE NONCLUSTERED INDEX [IX_InstructorID] ON [dbo].[Course] 
(
	[InstructorID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InstructorApplication](
	[InstructorApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[Experience] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[WillingToTravel] [bit] NOT NULL,
	[Approved] [bit] NOT NULL,
 CONSTRAINT [PK_InstructorApplication] PRIMARY KEY CLUSTERED 
(
	[InstructorApplicationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO

CREATE NONCLUSTERED INDEX [IX_StudentID] ON [dbo].[InstructorApplication] 
(
	[StudentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendance](
	[AttendanceID] [int] IDENTITY(1,1) NOT NULL,
	[CourseID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[AttendanceDay] [int] NOT NULL,
	[Present] [bit] NOT NULL,
 CONSTRAINT [PK_Attendance] PRIMARY KEY CLUSTERED 
(
	[AttendanceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO

CREATE NONCLUSTERED INDEX [IX_CourseID] ON [dbo].[Attendance] 
(
	[CourseID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO

CREATE NONCLUSTERED INDEX [IX_StudentID] ON [dbo].[Attendance] 
(
	[StudentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationCommissioning](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[ReCommissioning] [bit] NOT NULL,
	[RecommendationFiled] [bit] NOT NULL,
	[PersonalStatement] [nvarchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DayOfReflection] [bit] NOT NULL,
	[ApplicationFeePaid] [bit] NOT NULL,
	[MeetsMinimumRequirements] [bit] NOT NULL,
	[DateFiled] [datetime] NOT NULL,
	[DateApproved] [datetime] NULL,
	[AdminDenialReason] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Approved] [bit] NOT NULL,
	[Recommendation_Id] [int] NULL,
 CONSTRAINT [PK_ApplicationCommissioning] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO

CREATE NONCLUSTERED INDEX [IX_Recommendation_Id] ON [dbo].[ApplicationCommissioning] 
(
	[Recommendation_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO

CREATE NONCLUSTERED INDEX [IX_StudentID] ON [dbo].[ApplicationCommissioning] 
(
	[StudentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollment](
	[EnrollmentID] [int] IDENTITY(1,1) NOT NULL,
	[CourseID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[Grade] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Comments] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Paid] [bit] NOT NULL,
 CONSTRAINT [PK_Enrollment] PRIMARY KEY CLUSTERED 
(
	[EnrollmentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO

CREATE NONCLUSTERED INDEX [IX_CourseID] ON [dbo].[Enrollment] 
(
	[CourseID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO

CREATE NONCLUSTERED INDEX [IX_StudentID] ON [dbo].[Enrollment] 
(
	[StudentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EducationalBackground](
	[EducationalBackgroundID] [int] IDENTITY(1,1) NOT NULL,
	[UniversityOrCollege] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[AreaOfStudy] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Degree] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[YearReceived] [nvarchar](4) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[InstructorApplication_InstructorApplicationID] [int] NULL,
 CONSTRAINT [PK_EducationalBackground] PRIMARY KEY CLUSTERED 
(
	[EducationalBackgroundID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO

CREATE NONCLUSTERED INDEX [IX_InstructorApplication_InstructorApplicationID] ON [dbo].[EducationalBackground] 
(
	[InstructorApplication_InstructorApplicationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
ALTER TABLE [dbo].[RecommendationForCommissioning]  WITH CHECK ADD  CONSTRAINT [FK_RecommendationForCommissioning_Student_Student_StudentID] FOREIGN KEY([Student_StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[RecommendationForCommissioning] CHECK CONSTRAINT [FK_RecommendationForCommissioning_Student_Student_StudentID]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Instructor_InstructorID] FOREIGN KEY([InstructorID])
REFERENCES [dbo].[Instructor] ([InstructorID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Instructor_InstructorID]
GO
ALTER TABLE [dbo].[InstructorApplication]  WITH CHECK ADD  CONSTRAINT [FK_InstructorApplication_Student_StudentID] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InstructorApplication] CHECK CONSTRAINT [FK_InstructorApplication_Student_StudentID]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [FK_Attendance_Course_CourseID] FOREIGN KEY([CourseID])
REFERENCES [dbo].[Course] ([CourseID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_Attendance_Course_CourseID]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [FK_Attendance_Student_StudentID] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_Attendance_Student_StudentID]
GO
ALTER TABLE [dbo].[ApplicationCommissioning]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationCommissioning_RecommendationForCommissioning_Recommendation_Id] FOREIGN KEY([Recommendation_Id])
REFERENCES [dbo].[RecommendationForCommissioning] ([Id])
GO
ALTER TABLE [dbo].[ApplicationCommissioning] CHECK CONSTRAINT [FK_ApplicationCommissioning_RecommendationForCommissioning_Recommendation_Id]
GO
ALTER TABLE [dbo].[ApplicationCommissioning]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationCommissioning_Student_StudentID] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationCommissioning] CHECK CONSTRAINT [FK_ApplicationCommissioning_Student_StudentID]
GO
ALTER TABLE [dbo].[Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Enrollment_Course_CourseID] FOREIGN KEY([CourseID])
REFERENCES [dbo].[Course] ([CourseID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Enrollment] CHECK CONSTRAINT [FK_Enrollment_Course_CourseID]
GO
ALTER TABLE [dbo].[Enrollment]  WITH CHECK ADD  CONSTRAINT [FK_Enrollment_Student_StudentID] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Enrollment] CHECK CONSTRAINT [FK_Enrollment_Student_StudentID]
GO
ALTER TABLE [dbo].[EducationalBackground]  WITH CHECK ADD  CONSTRAINT [FK_EducationalBackground_InstructorApplication_InstructorApplication_InstructorApplicationID] FOREIGN KEY([InstructorApplication_InstructorApplicationID])
REFERENCES [dbo].[InstructorApplication] ([InstructorApplicationID])
GO
ALTER TABLE [dbo].[EducationalBackground] CHECK CONSTRAINT [FK_EducationalBackground_InstructorApplication_InstructorApplication_InstructorApplicationID]
GO
