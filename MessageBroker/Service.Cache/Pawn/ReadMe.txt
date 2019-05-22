

alter procedure mobi_pawn_info_cacheInitData
as
begin		
	SELECT distinct top 10
		--DocID
		--,DocType
		PawnID					AS PawnID
		,CodeNo					AS PawnCode
		,_p0.CustomerID			AS CustomerID
		,_p3.[Name]				AS CustomerName
		,ISNULL(_p2.ten_vv, '') AS CateName
		,AssetName
		,CAST(replace(replace(replace(convert(varchar, DateExpired, 120), '-',''), ':',''), ' ','') AS bigint)			AS DatetimeCreate --ExtDate
		,CAST(replace(replace(replace(convert(varchar, _p0.Created, 120), '-',''), ':',''), ' ','') AS bigint)			AS DatetimeFinish --TranXLNDate
		,Overdue				AS SumLoanDate --Overdue
		--,Comment
		--,TotalLoan
		--,_p1.UserFullName		AS UserProcess
		,Loan					AS LoanAmount --MoneyPawn
		,_p0.STATUS				AS [Status]
	FROM collect.Document _p0
		LEFT JOIN pos.[User] _p1 ON _p0.SupportID = _p1.UserID
		LEFT JOIN dbo.dmvv _p2 ON _p0.CategoryCode = _p2.ma_vv
		LEFT JOIN pos.Customer _p3 ON _p0.CustomerID = _p3.CustomerID
end

ALTER procedure [dbo].[mobi_bank_info_cacheInitData]
as
begin
	--select distinct 0 as BankId, s.BankName as [Name], s.BankBranch as Branch FROM [POS_TEST].[pos].[ShopDetail] s where s.BankName is not null
	SELECT [id] as Id
	,[Code]
	,[Name]
	,[Status]
	FROM [POS_TEST].[pos].[Bank]
end

alter procedure mobi_shop_info_cacheInitData
as
begin
	select s.ShopID, s.Code, s.Name, s.Address, s.Phone FROM [POS_TEST].[pos].[ShopDetail] s
end

ALTER procedure [dbo].[mobi_customer_info_cacheInitData]
as begin
	SELECT [PawnID]
	,[CustomerID]
	,[HouseholdRegionID]
	,[HouseholdAddress]
	,[CurrentAddressRegionID]
	,[CurrentAddress]
	,[Facebook]
	,[Zalo]
	,[Email]
	,[StayType]
	,[StayShared]
	,[StayTimeType]
	,[StayTime]
	,[Married]
	,[Child]
	,[Income]
	,[WorkType]
	,[CompanyName]
	,[CompanyPhone]
	,[Salary]
	,[SalaryType]
	,[MarriedPhone]
	,[MarriedWork]
	,[RelativesName1]
	,[RelativesPhone1]
	,[RelativesName2]
	,[RelativesPhone2]
	,[NeighborName1]
	,[NeighborName2]
	,[FriendName]
	,[FriendPhone]
	,[CompanyAddress]
	,[WorkTimeAtCompany]
	,[Position]
	,[RelativesIdentityID]
	,[RelativesIdentityDate]
	,[RelativesIdentityPlace]
	,[RelativesDoB]
	,[RelativesHouseHoldAddress]
	,[RelativesCurrentAddress]
	,[RelativesName]
	,[RelativesCompanyName]
	,[RelativesCompanyPhone]
	,[RelativesCompanyAddress]
	,[RelativesPosition]
	,[RelativesWorkTimeAtCompany]
	,[RelativesSalary]
	,[RelativesSalaryType]
	,[PaperType]
	,[PaperTypeNote]
	,[BankID]
	,[BankName]
	,[Account]
	FROM [POS_TEST].[pos].[PawnCustomer]
end


