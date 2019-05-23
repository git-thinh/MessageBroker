EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserLogin', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserLogin', @level2type=N'COLUMN',@level2name=N'Source_ID'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SyncHistory', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SyncHistory', @level2type=N'COLUMN',@level2name=N'DateStart'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SyncHistory', @level2type=N'COLUMN',@level2name=N'Direction'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SyncHistory', @level2type=N'COLUMN',@level2name=N'Action'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SyncHistory', @level2type=N'COLUMN',@level2name=N'ServiceCode'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SyncHistory', @level2type=N'COLUMN',@level2name=N'Record_ID'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PawnInfo', @level2type=N'COLUMN',@level2name=N'ContactRegistrationBook_ID'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PawnInfo', @level2type=N'COLUMN',@level2name=N'ContactNeighbor_ID'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PawnContactLocate', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerInfo', @level2type=N'COLUMN',@level2name=N'IsInviter'
GO
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerInfo', @level2type=N'COLUMN',@level2name=N'Invite_ID'
GO
/****** Object:  StoredProcedure [dbo].[user_login_contact_createNew]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP PROCEDURE [dbo].[user_login_contact_createNew]
GO
/****** Object:  StoredProcedure [dbo].[user_login_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP PROCEDURE [dbo].[user_login_cacheInitData]
GO
/****** Object:  StoredProcedure [dbo].[sync_history_createNew]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP PROCEDURE [dbo].[sync_history_createNew]
GO
/****** Object:  StoredProcedure [dbo].[source_info_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP PROCEDURE [dbo].[source_info_cacheInitData]
GO
/****** Object:  StoredProcedure [dbo].[shop_info_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP PROCEDURE [dbo].[shop_info_cacheInitData]
GO
/****** Object:  StoredProcedure [dbo].[pawn_info_update]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP PROCEDURE [dbo].[pawn_info_update]
GO
/****** Object:  StoredProcedure [dbo].[pawn_info_createNew]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP PROCEDURE [dbo].[pawn_info_createNew]
GO
/****** Object:  StoredProcedure [dbo].[pawn_info_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP PROCEDURE [dbo].[pawn_info_cacheInitData]
GO
/****** Object:  StoredProcedure [dbo].[pawn_images_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP PROCEDURE [dbo].[pawn_images_cacheInitData]
GO
/****** Object:  StoredProcedure [dbo].[customer_info_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP PROCEDURE [dbo].[customer_info_cacheInitData]
GO
/****** Object:  StoredProcedure [dbo].[contact_info_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP PROCEDURE [dbo].[contact_info_cacheInitData]
GO
/****** Object:  StoredProcedure [dbo].[bank_info_deleteAllDataAndSyncNewFromPOS]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP PROCEDURE [dbo].[bank_info_deleteAllDataAndSyncNewFromPOS]
GO
/****** Object:  StoredProcedure [dbo].[bank_info_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP PROCEDURE [dbo].[bank_info_cacheInitData]
GO
/****** Object:  StoredProcedure [dbo].[_pos_check_pawn_state]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP PROCEDURE [dbo].[_pos_check_pawn_state]
GO
ALTER TABLE [dbo].[PawnContactLocate] DROP CONSTRAINT [DF_PawnContact_Type]
GO
/****** Object:  Table [dbo].[UserLogin]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[UserLogin]
GO
/****** Object:  Table [dbo].[SyncStateName]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[SyncStateName]
GO
/****** Object:  Table [dbo].[SyncHistory]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[SyncHistory]
GO
/****** Object:  Table [dbo].[SourceInfo]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[SourceInfo]
GO
/****** Object:  Table [dbo].[ShopInfo]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[ShopInfo]
GO
/****** Object:  Table [dbo].[PayAccount]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[PayAccount]
GO
/****** Object:  Table [dbo].[PawnInfoState]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[PawnInfoState]
GO
/****** Object:  Table [dbo].[PawnInfo]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[PawnInfo]
GO
/****** Object:  Table [dbo].[PawnImageType]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[PawnImageType]
GO
/****** Object:  Table [dbo].[PawnImages]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[PawnImages]
GO
/****** Object:  Table [dbo].[PawnContactLocate]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[PawnContactLocate]
GO
/****** Object:  Table [dbo].[PawnAssetLocate]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[PawnAssetLocate]
GO
/****** Object:  Table [dbo].[CustomerInfo]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[CustomerInfo]
GO
/****** Object:  Table [dbo].[ContactInfo]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[ContactInfo]
GO
/****** Object:  Table [dbo].[BankInfo]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[BankInfo]
GO
/****** Object:  Table [dbo].[AssetInfo]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[AssetInfo]
GO
/****** Object:  Table [dbo].[AssetCategory]    Script Date: 5/22/2019 10:49:48 PM ******/
DROP TABLE [dbo].[AssetCategory]
GO
/****** Object:  Table [dbo].[AssetCategory]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssetCategory](
	[AssetCategory_ID] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_AssetCategory] PRIMARY KEY CLUSTERED 
(
	[AssetCategory_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssetInfo]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssetInfo](
	[Asset_ID] [int] NOT NULL,
	[AssetCategory_ID] [int] NOT NULL,
	[Name] [nvarchar](1000) NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_AssetInfo] PRIMARY KEY CLUSTERED 
(
	[Asset_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BankInfo]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankInfo](
	[Bank_ID] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Branch] [nvarchar](255) NULL,
	[Code] [varchar](255) NULL,
	[Address] [nvarchar](1000) NULL,
	[Phone] [int] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Bank] PRIMARY KEY CLUSTERED 
(
	[Bank_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactInfo]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactInfo](
	[Contact_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[AddressCompany] [nvarchar](500) NULL,
	[AddressPlace] [nvarchar](500) NULL,
	[Phones] [varchar](500) NULL,
	[Emails] [varchar](500) NULL,
	[Facebook] [varchar](255) NULL,
	[Zalo] [varchar](255) NULL,
	[Google] [varchar](255) NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[Contact_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerInfo]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerInfo](
	[Customer_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Contact_ID] [bigint] NULL,
	[Source_ID] [int] NULL,
	[Invite_ID] [int] NULL,
	[IsInviter] [bit] NULL,
	[CMND_CCCD] [varchar](255) NULL,
	[CMND_CreateDate] [int] NULL,
	[CMND_CreatePlace] [nvarchar](2000) NULL,
	[CompanyPhone] [int] NULL,
	[Brithday] [int] NULL,
	[Gender] [int] NULL,
	[Salary] [int] NULL,
	[CompanyName] [nvarchar](255) NULL,
	[DateCreate] [bigint] NULL,
	[DateActive] [bigint] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_CustomerInfo] PRIMARY KEY CLUSTERED 
(
	[Customer_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PawnAssetLocate]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PawnAssetLocate](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Pawn_ID] [bigint] NOT NULL,
	[Asset_ID] [int] NOT NULL,
	[AssetCategory_ID] [int] NOT NULL,
 CONSTRAINT [PK_PawnAssetLocate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PawnContactLocate]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PawnContactLocate](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[Pawn_ID] [bigint] NOT NULL,
	[Contact_ID] [bigint] NOT NULL,
 CONSTRAINT [PK_PawnContact] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PawnImages]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PawnImages](
	[PawnImage_ID] [int] IDENTITY(1,1) NOT NULL,
	[Pawn_ID] [int] NULL,
	[PawnImageType_ID] [int] NULL,
	[Path] [varchar](2000) NULL,
	[Host] [varchar](36) NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_PawnImages] PRIMARY KEY CLUSTERED 
(
	[PawnImage_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PawnImageType]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PawnImageType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
 CONSTRAINT [PK_PawnImageType_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PawnInfo]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PawnInfo](
	[Pawn_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[PawnCode] [nvarchar](255) NULL,
	[LoanAmount] [bigint] NULL,
	[DatetimeCreate] [bigint] NULL,
	[DatetimeFinish] [bigint] NULL,
	[SumLoanDate] [int] NULL,
	[Customer_ID] [bigint] NULL,
	[CustomerContact_ID] [bigint] NULL,
	[Bank_ID] [int] NULL,
	[PayAccount_ID] [bigint] NULL,
	[ContractSettlementShop_ID] [int] NULL,
	[ContactNeighbor_ID] [bigint] NULL,
	[ContactColleague_ID] [bigint] NULL,
	[ContactRegistrationBook_ID] [bigint] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Pawn] PRIMARY KEY CLUSTERED 
(
	[Pawn_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PawnInfoState]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PawnInfoState](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
 CONSTRAINT [PK_PawnInfoState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PayAccount]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayAccount](
	[PayAccount_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Bank_ID] [int] NULL,
	[No] [varchar](255) NULL,
	[Name] [nvarchar](255) NULL,
	[Code] [int] NULL,
	[Status] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShopInfo]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShopInfo](
	[Shop_ID] [int] NOT NULL,
	[Code] [varchar](255) NULL,
	[Name] [nvarchar](255) NULL,
	[Address] [nvarchar](100) NULL,
	[Phone] [int] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_ShopInfo] PRIMARY KEY CLUSTERED 
(
	[Shop_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SourceInfo]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SourceInfo](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
 CONSTRAINT [PK_SourceInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SyncHistory]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SyncHistory](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Record_ID] [bigint] NULL,
	[ServiceCode] [varchar](255) NULL,
	[Action] [int] NULL,
	[Direction] [int] NULL,
	[DateStart] [bigint] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_PosSyncHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SyncStateName]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SyncStateName](
	[State_ID] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
 CONSTRAINT [PK_PosSyncState] PRIMARY KEY CLUSTERED 
(
	[State_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLogin]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLogin](
	[User_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Contact_ID] [bigint] NULL,
	[Source_ID] [int] NULL,
	[Username] [varchar](255) NULL,
	[Password] [varchar](255) NULL,
	[DeviceCode] [varchar](255) NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_UserLogin] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PawnContactLocate] ADD  CONSTRAINT [DF_PawnContact_Type]  DEFAULT ((0)) FOR [Type]
GO
/****** Object:  StoredProcedure [dbo].[_pos_check_pawn_state]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[_pos_check_pawn_state](@Pawn_ID bigint, @State int output)
AS
BEGIN
	--[ check state trong bảng pawntransaction để output ra kiểu int của State where @Pawn_ID]
	set @State = 1;
	select 1;
END
GO
/****** Object:  StoredProcedure [dbo].[bank_info_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[bank_info_cacheInitData]
as
begin
	--[ Đẩy toàn bộ bảng dữ liệu BankInfo lên CacheEngine ]

	----select distinct 0 as BankId, s.BankName as [Name], s.BankBranch as Branch FROM [POS_TEST].[pos].[ShopDetail] s where s.BankName is not null
	--SELECT [id] as Id
	--,[Code]
	--,[Name]
	--,[Status]
	--FROM [POS_TEST].[pos].[Bank]

	SELECT * FROM [dbo].[BankInfo]

end
GO
/****** Object:  StoredProcedure [dbo].[bank_info_deleteAllDataAndSyncNewFromPOS]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[bank_info_deleteAllDataAndSyncNewFromPOS]
as
begin
	--[ Delete all data of table BankInfo, After sync all data from database POS ]

	INSERT INTO [BankInfo](	[Bank_ID],[Code],[Name],[Branch],[Address],[Phone],[Status])
	SELECT					[id],	  [Code],[Name],'','',0,[Status] FROM [POS_TEST].[pos].[Bank]
end
GO
/****** Object:  StoredProcedure [dbo].[contact_info_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[contact_info_cacheInitData]
as
begin
	-- [ Push data about ContactInfo to CacheEngine ]

	SELECT * FROM [ContactInfo]
end
GO
/****** Object:  StoredProcedure [dbo].[customer_info_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[customer_info_cacheInitData]
as
begin		
	--[ Push all data of PawnInfo to CachEngine ]

	-- On db POS:
	--SELECT distinct top 10
	--	--DocID
	--	--,DocType
	--	PawnID					AS PawnID
	--	,CodeNo					AS PawnCode
	--	,_p0.CustomerID			AS CustomerID
	--	,_p3.[Name]				AS CustomerName
	--	,ISNULL(_p2.ten_vv, '') AS CateName
	--	,AssetName
	--	,CAST(replace(replace(replace(convert(varchar, DateExpired, 120), '-',''), ':',''), ' ','') AS bigint)			AS DatetimeCreate --ExtDate
	--	,CAST(replace(replace(replace(convert(varchar, _p0.Created, 120), '-',''), ':',''), ' ','') AS bigint)			AS DatetimeFinish --TranXLNDate
	--	,Overdue				AS SumLoanDate --Overdue
	--	--,Comment
	--	--,TotalLoan
	--	--,_p1.UserFullName		AS UserProcess
	--	,Loan					AS LoanAmount --MoneyPawn
	--	,_p0.STATUS				AS [Status]
	--FROM collect.Document _p0
	--	LEFT JOIN pos.[User] _p1 ON _p0.SupportID = _p1.UserID
	--	LEFT JOIN dbo.dmvv _p2 ON _p0.CategoryCode = _p2.ma_vv
	--	LEFT JOIN pos.Customer _p3 ON _p0.CustomerID = _p3.CustomerID

	SELECT * FROM CustomerInfo
	END
GO
/****** Object:  StoredProcedure [dbo].[pawn_images_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[pawn_images_cacheInitData]
as
begin
	-- [ Push data about user to CacheEngine ]

	-- select s.ShopID, s.Code, s.Name, s.Address, s.Phone FROM [POS_TEST].[pos].[ShopDetail] s
	SELECT * FROM PawnImages
end
GO
/****** Object:  StoredProcedure [dbo].[pawn_info_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[pawn_info_cacheInitData]
as
begin		
	--[ Push all data of PawnInfo to CachEngine ]

	-- On db POS:
	--SELECT distinct top 10
	--	--DocID
	--	--,DocType
	--	PawnID					AS PawnID
	--	,CodeNo					AS PawnCode
	--	,_p0.CustomerID			AS CustomerID
	--	,_p3.[Name]				AS CustomerName
	--	,ISNULL(_p2.ten_vv, '') AS CateName
	--	,AssetName
	--	,CAST(replace(replace(replace(convert(varchar, DateExpired, 120), '-',''), ':',''), ' ','') AS bigint)			AS DatetimeCreate --ExtDate
	--	,CAST(replace(replace(replace(convert(varchar, _p0.Created, 120), '-',''), ':',''), ' ','') AS bigint)			AS DatetimeFinish --TranXLNDate
	--	,Overdue				AS SumLoanDate --Overdue
	--	--,Comment
	--	--,TotalLoan
	--	--,_p1.UserFullName		AS UserProcess
	--	,Loan					AS LoanAmount --MoneyPawn
	--	,_p0.STATUS				AS [Status]
	--FROM collect.Document _p0
	--	LEFT JOIN pos.[User] _p1 ON _p0.SupportID = _p1.UserID
	--	LEFT JOIN dbo.dmvv _p2 ON _p0.CategoryCode = _p2.ma_vv
	--	LEFT JOIN pos.Customer _p3 ON _p0.CustomerID = _p3.CustomerID


	SELECT
        Pawn_ID -- long
        -------------------------------------------
        ,PawnCode -- string
        ,LoanAmount -- long
        ,DatetimeCreate -- long  
        ,DatetimeFinish -- long 
        ,SumLoanDate -- int
        -------------------------------------------
        ,'' as Asset_Name -- string
        ,'' as AssetCategory_Name -- string
        -------------------------------------------
        --// Thong tin khach hang
        ,'' as CMND_CCCD -- string
        ,'' as CMND_CreateDate -- int
        ,'' as CMND_CreateAddress -- string
        --//-------------------------------------------
        ,'' as Custorer_Name -- string
        ,'' as Custorer_Phone -- int
        ,'' as Custorer_AddressPlace -- string
        --//-------------------------------------------
        ,'' as BankName -- string
        ,'' as BankAccountNo -- string
        --//-------------------------------------------
        --//--+ ContactInfo for type is a ContactRegistrationBook_ID nguoi than tren so ho khau
        ,'' as RegistrationBook_Name -- string
        ,'' as RegistrationBook_AddressPlace -- string
        ,'' as RegistrationBook_Phone -- string
        --//--+ ContactInfo for type is a ContactColleague_ID dong nghiep
        ,'' as Colleague_Name -- string
        ,'' as Colleague_AddressPlace -- string
        ,'' as Colleague_Phone -- string
        ---//-------------------------------------------
        ,'' as ContractSettlementShop_Name -- int
        --//-------------------------------------------
        ,[Status] -- 	
	
	FROM [PawnInfo]

end
GO
/****** Object:  StoredProcedure [dbo].[pawn_info_createNew]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[pawn_info_createNew](
	--+ PayAccount
	@Bank_ID int,
	@PayAccount_No varchar(255),
	-- ContactInfo for type is a Neighbor hang xom
	--+ ContactInfo for type is a ContactRegistrationBook_ID nguoi than tren so ho khau
	@RegistrationBook_Name nvarchar(255),  
	@RegistrationBook_AddressPlace nvarchar(500),
	@RegistrationBook_Phone varchar(500),
	--+ ContactInfo for type is a ContactColleague_ID dong nghiep
	@Colleague_Name nvarchar(255),  
	@Colleague_AddressPlace nvarchar(500),
	@Colleague_Phone varchar(500),
	--+ ContactInfo for type is a Customer
	@Custorer_Name nvarchar(255),  
	@Custorer_AddressPlace nvarchar(500),
	@Custorer_Phone varchar(500),
	--+ CustomerInfo
	-- @Contact_ID, bigint
	@Invite_ID bigint, -- It is User_ID logined
	@CMND_CCCD varchar(255),
	@CMND_CreateDate int,
	@CMND_CreatePlace nvarchar(2000),
	--+ PawnInfo
	-- @PayAccount_ID 
	@PawnCode nvarchar(255),	--ma hop dong
	@LoanAmount bigint,			-- so tien vay
	@SumLoanDate int,		-- thoi han vay theo ngay
	@DatetimeFinish bigint,		--ngay tat toan hop dong
	@ContractSettlementShop_ID int,		--chon sua hang tat toan
	--+ PawnImages, PawnImageLocate
	@Image_RegistrationBook_1 nvarchar(255),	--anh tren so ho khau
	@Image_RegistrationBook_2 nvarchar(255),	--anh tren so ho khau
	@Image_VehicleRegistration_1 nvarchar(255),	--anh giay to xe
	@Image_VehicleRegistration_2 nvarchar(255),	--anh giay to xe
	@Image_InvoiceElectric_1 nvarchar(255),	--anh tren so ho khau
	@Image_Asset_1 nvarchar(255),	--anh tai san
	@Image_Asset_2 nvarchar(255)	--anh tai san
)
AS
BEGIN
	--[ Create new a Pawn ]
	----[1] Insert PayAccount return @PayAccount_ID
	----[2] Insert ContactInfo return @CustomerContact_ID, @ContactColleague_ID, @ContactRegistrationBook_ID
	----[3] Insert CustomerInfo return @Customer_ID
	----[4] Insert PawnInfo return @Pawn_ID
	----[5] Insert PawnImages return @PawnImages_ID
	----[6] Insert SyncHistory
	----[7] Export to CacheEngine
	
	DECLARE @OutputID TABLE (ID BIGINT)	
	--=============================================================================
	----[1] Insert PayAccount return @PayAccount_ID
	DECLARE @PayAccount_ID BIGINT = 0;

	INSERT INTO [PayAccount]([Bank_ID],[No],[Name],[Code],[Status])
	OUTPUT INSERTED.PayAccount_ID INTO @OutputID(ID)
    VALUES (@Bank_ID, @PayAccount_No, @Custorer_Name, 0, 1);

	SELECT @PayAccount_ID = ID FROM @OutputID;
	DELETE FROM @OutputID;	
	--=============================================================================
	----[2] Insert ContactInfo return @CustomerContact_ID, @ContactColleague_ID, @ContactRegistrationBook_ID
	DECLARE @ContactColleague_ID BIGINT = 0;

	INSERT INTO [dbo].[ContactInfo]([Name],[AddressCompany],[AddressPlace],[Phones],[Emails],[Status])
	OUTPUT INSERTED.Contact_ID INTO @OutputID(ID)
    VALUES(@Colleague_Name,'',@Colleague_AddressPlace,@Colleague_Phone,'',1);

	SELECT @ContactColleague_ID = ID FROM @OutputID;
	DELETE FROM @OutputID;
	-------------------------------------------------------------
	DECLARE @ContactRegistrationBook_ID BIGINT = 0;

	INSERT INTO [dbo].[ContactInfo]([Name],[AddressCompany],[AddressPlace],[Phones],[Emails],[Status])
	OUTPUT INSERTED.Contact_ID INTO @OutputID(ID)
    VALUES(@RegistrationBook_Name,'',@RegistrationBook_AddressPlace,@RegistrationBook_Phone,'',1);

	SELECT @ContactRegistrationBook_ID = ID FROM @OutputID;
	DELETE FROM @OutputID;
	-------------------------------------------------------------
	DECLARE @CustomerContact_ID BIGINT = 0;

	INSERT INTO [dbo].[ContactInfo]([Name],[AddressCompany],[AddressPlace],[Phones],[Emails],[Status])
	OUTPUT INSERTED.Contact_ID INTO @OutputID(ID)
    VALUES(@Custorer_Name,'',@Custorer_AddressPlace,@Custorer_Phone,'',1);

	SELECT @CustomerContact_ID = ID FROM @OutputID;
	DELETE FROM @OutputID;	
	--=============================================================================
	----[3] Insert CustomerInfo return @Customer_ID
	DECLARE @Customer_ID BIGINT = 0;
	DECLARE @DateCreate BIGINT = CAST(replace(replace(replace(convert(varchar, getdate(), 120), '-',''), ':',''), ' ','') AS bigint);

	-- [Source_ID]: 1=Xe ôm công nghệ; 2=ECPay
	INSERT INTO [CustomerInfo]([Contact_ID],[Source_ID],[Invite_ID],[IsInviter],[CMND_CCCD],[CMND_CreateDate],[CMND_CreatePlace],[DateCreate],[Status])
	OUTPUT INSERTED.Customer_ID INTO @OutputID(ID)
	VALUES				(@CustomerContact_ID,	2,		@Invite_ID,0,@CMND_CCCD,@CMND_CreateDate,@CMND_CreatePlace,@DateCreate,1)
	 
	SELECT @Customer_ID = ID FROM @OutputID;
	DELETE FROM @OutputID;
	--=============================================================================
	----[4] Insert PawnInfo return @Pawn_ID
	DECLARE @Pawn_ID BIGINT = 0;
	
	-- Status: 1	HOP_DONG_NHAP_DOI_DUYET; 2	HOP_DONG_DA_DUYET_DOI_GIAI_NGAN; 3	HOP_DONG_THANH_CONG_DANG_VAY; 4	HOP_DONG_DA_TU_CHOI
	INSERT INTO [PawnInfo](
		[PawnCode],[LoanAmount],[SumLoanDate],[DatetimeCreate],[DatetimeFinish],[Customer_ID],[CustomerContact_ID],[Bank_ID],[PayAccount_ID],
		[ContractSettlementShop_ID],[ContactNeighbor_ID],[ContactColleague_ID],[ContactRegistrationBook_ID],[Status])
	OUTPUT INSERTED.Pawn_ID INTO @OutputID(ID)
    VALUES(@PawnCode,@LoanAmount,@SumLoanDate,@DateCreate,@DatetimeFinish,@Customer_ID,@CustomerContact_ID,@Bank_ID,@PayAccount_ID,
		@ContractSettlementShop_ID,0,@ContactColleague_ID,@ContactRegistrationBook_ID, 1);

	SELECT @Pawn_ID = ID FROM @OutputID;
	DELETE FROM @OutputID;	
	--=============================================================================
	----[5] Insert PawnImages return @PawnImages_ID
	-- PawnImageType_ID
		--1	Ảnh tài sản
		--2	Ảnh hộ khẩu
		--3	Ảnh giấy tờ xe
		--4	Ảnh hóa đơn điện
		--5	Ảnh CMND_CCCD
		--99	Ảnh khác
		 
	INSERT INTO [PawnImages]([Pawn_ID],[PawnImageType_ID],[Host],[Path],[Status]) VALUES(@Pawn_ID,2,'', @Image_RegistrationBook_1, 1);
	INSERT INTO [PawnImages]([Pawn_ID],[PawnImageType_ID],[Host],[Path],[Status]) VALUES(@Pawn_ID,2,'', @Image_RegistrationBook_2, 1);
	  
	INSERT INTO [PawnImages]([Pawn_ID],[PawnImageType_ID],[Host],[Path],[Status]) VALUES(@Pawn_ID,3,'', @Image_VehicleRegistration_1, 1);
	INSERT INTO [PawnImages]([Pawn_ID],[PawnImageType_ID],[Host],[Path],[Status]) VALUES(@Pawn_ID,3,'', @Image_VehicleRegistration_2, 1);
	  
	INSERT INTO [PawnImages]([Pawn_ID],[PawnImageType_ID],[Host],[Path],[Status]) VALUES(@Pawn_ID,1,'', @Image_Asset_1, 1);
	INSERT INTO [PawnImages]([Pawn_ID],[PawnImageType_ID],[Host],[Path],[Status]) VALUES(@Pawn_ID,1,'', @Image_Asset_2, 1);
	  
	INSERT INTO [PawnImages]([Pawn_ID],[PawnImageType_ID],[Host],[Path],[Status]) VALUES(@Pawn_ID,4,'', @Image_InvoiceElectric_1, 1);
	--=============================================================================
	----[6] Insert SyncHistory
	-- Action		= 3: INSERT | 2: UPDATE | 1: DELETE
	-- Direction	=  0: Thay đổi local  1: sync_mobi_to_pos | 2: sync_pos_to_mobi
	-- State		=  0: Đợi push lên CacheEngine, 1: Đồng bộ thành công, 2: Thất bại do ... read from table SyncStateName
	exec sync_history_createNew @Record_ID = @Pawn_ID, @ServiceCode = 'pawn_info,customer_info,contact_info,pawn_images', @Action = 3, @Direction = 0;
	
	----[7] Export to CacheEngine
	SELECT 1 as Ok, '' as [Message], 'pawn_info,customer_info,contact_info,pawn_images' as ServiceCache, 
		@Pawn_ID as Pawn_ID, @PayAccount_ID as PayAccount_ID , @Customer_ID as Customer_ID 
END
GO
/****** Object:  StoredProcedure [dbo].[pawn_info_update]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[pawn_info_update]
AS
BEGIN
	--[ Execute infomation a pawn ]
	-- Execute sync data about pawn for Mobi <-> POS:
	---- The code execute it must be belong the block code Try Catch, 
	---- Output of result is sucess or fail
	---- Insert result into the table SyncHistory to push notification on CacheEngine

	declare @Pawn_ID bigint = 0;

	--[1] thực hiện tạo mới 1 hợp đồng
	-- insert into PawnInfo, PawnImages, ContactInfo, CustomerInfo

	--[2] ...

	if(@Pawn_ID > 0) begin exec sync_mobi_to_pos_PawnInfo_byUpdate @Pawn_ID; end
END
GO
/****** Object:  StoredProcedure [dbo].[shop_info_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[shop_info_cacheInitData]
as
begin
	-- select s.ShopID, s.Code, s.Name, s.Address, s.Phone FROM [POS_TEST].[pos].[ShopDetail] s
	SELECT [Shop_ID]
		,[Code]
		,[Name]
		,[Address]
		,[Phone]
		,[State]
	FROM [dbo].[ShopInfo]
end
GO
/****** Object:  StoredProcedure [dbo].[source_info_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[source_info_cacheInitData]
as
begin
	-- [ Push data about user to SourceInfo ]
	SELECT * FROM SourceInfo
end
GO
/****** Object:  StoredProcedure [dbo].[sync_history_createNew]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[sync_history_createNew]( 
	@Record_ID bigint,
	@ServiceCode varchar(255),
	@Action int,
	@Direction int
)
AS
BEGIN
	--[3] Insert into table SyncHistory to push notification on CacheEngine
	-- Action		= 3: INSERT | 2: UPDATE | 1: DELETE
	-- Direction	=  0: Thay đổi local  1: sync_mobi_to_pos | 2: sync_pos_to_mobi
	-- State		=  0: Đợi push lên CacheEngine, 1: Đồng bộ thành công, 2: Thất bại do ... read from table SyncStateName

	DECLARE @DateStart BIGINT = CAST(replace(replace(replace(convert(varchar, getdate(), 120), '-',''), ':',''), ' ','') AS bigint);

	INSERT INTO [SyncHistory]([Record_ID],[ServiceCode],[Action],[Direction],[DateStart],[Status])
    --VALUES(@Contact_ID, 'user_login,contact_info', 3, 0, @DateStart, 0);
    VALUES					(@Record_ID, @ServiceCode, @Action, @Direction, @DateStart, 0);
END
GO
/****** Object:  StoredProcedure [dbo].[user_login_cacheInitData]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[user_login_cacheInitData]
as
begin
	-- [ Push data about user to CacheEngine ]

	-- select s.ShopID, s.Code, s.Name, s.Address, s.Phone FROM [POS_TEST].[pos].[ShopDetail] s
	SELECT * FROM [UserLogin]
end
GO
/****** Object:  StoredProcedure [dbo].[user_login_contact_createNew]    Script Date: 5/22/2019 10:49:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[user_login_contact_createNew](
	@Name nvarchar(255),
	@AddressCompany nvarchar(500),
	@AddressPlace nvarchar(500),
	@Phones varchar(500),
	@Emails varchar(500)
)
AS
BEGIN
	--[ create new account for ECPay to login ]
	----- insert new items as follows: ContactInfo, UserLogin
	
	DECLARE @Contact_ID BIGINT = 0;
	DECLARE @OutputID TABLE (ID BIGINT)

	--[1] Insert into table ContactInfo
	INSERT INTO [ContactInfo]([Name],[AddressCompany],[AddressPlace],[Phones],[Emails],[Status])
	OUTPUT INSERTED.Contact_ID INTO @OutputID(ID)
	VALUES (@Name, @AddressCompany, @AddressPlace, @Phones, @Emails, 1);

	SELECT @Contact_ID = ID FROM @OutputID;
	DELETE FROM @OutputID;

	--[2] Insert into table UserLogin
	-- SourceID	= Loại nhóm: 1= xe ôm công nghệ | 2 = ecpay
	-- Status	= 0= cấm | 1= chờ duyệt | 2= kích hoạt
	DECLARE @User_ID BIGINT = 0;

	INSERT INTO [UserLogin]([Contact_ID],[Source_ID],[Username],[Password],[DeviceCode],[Status])
	OUTPUT INSERTED.User_ID INTO @OutputID(ID)
	VALUES(@Contact_ID, 2, @Phones, '','', 1); 

	SELECT @User_ID = ID FROM @OutputID;
	
	--[3] Insert into table SyncHistory to push notification on CacheEngine
	-- Action		= 3: INSERT | 2: UPDATE | 1: DELETE
	-- Direction	=  0: Thay đổi local  1: sync_mobi_to_pos | 2: sync_pos_to_mobi
	-- State		=  0: Đợi push lên CacheEngine, 1: Đồng bộ thành công, 2: Thất bại do ... read from table SyncStateName
	exec sync_history_createNew @Record_ID = @Contact_ID, @ServiceCode = 'user_login,contact_info', @Action = 3, @Direction = 0;

	--[4] Export to CacheEngine
	SELECT 1 as Ok, '' as [Message], 'user_login,contact_info' as ServiceCache, @Contact_ID as Contact_ID, @User_ID as [User_ID] 
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mã người giới thiệu, chính là mã khách hàng' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerInfo', @level2type=N'COLUMN',@level2name=N'Invite_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Khách hàng này là người giới thiệu KH khác (cộng tác viên bán hàng)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CustomerInfo', @level2type=N'COLUMN',@level2name=N'IsInviter'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0=Other, 1= Neighbor, 2=Colleague, 3 = RegistrationBook' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PawnContactLocate', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Thong tin lien lac hang xom luu trong ContactInfo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PawnInfo', @level2type=N'COLUMN',@level2name=N'ContactNeighbor_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Thong tin lien lac nguoi than tren so ho khau luu trong ContactInfo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PawnInfo', @level2type=N'COLUMN',@level2name=N'ContactRegistrationBook_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mã bản ghi đã thay đổi thông tin' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SyncHistory', @level2type=N'COLUMN',@level2name=N'Record_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mã các đầu service sẽ nhận notifications: nếu là sync -> USER, CUSTOMER, PAWN, ...; local change -> là service_name = pawn_info, user_login, ...' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SyncHistory', @level2type=N'COLUMN',@level2name=N'ServiceCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'3: INSERT | 2: UPDATE | 1: DELETE' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SyncHistory', @level2type=N'COLUMN',@level2name=N'Action'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0: Thay đổi local  1: sync_mobi_to_pos | 2: sync_pos_to_mobi' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SyncHistory', @level2type=N'COLUMN',@level2name=N'Direction'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Thời gian thực hiện' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SyncHistory', @level2type=N'COLUMN',@level2name=N'DateStart'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Trỏ vào bảng SyncStateName để lấy thông tin chi tiết' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SyncHistory', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Loại nhóm: 1= xe ôm công nghệ | 2 = ecpay' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserLogin', @level2type=N'COLUMN',@level2name=N'Source_ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0= cấm | 1= chờ duyệt | 2= kích hoạt' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserLogin', @level2type=N'COLUMN',@level2name=N'Status'
GO
