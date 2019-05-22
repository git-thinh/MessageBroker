	INSERT INTO PawnInfo ([Pawn_ID] ,[PawnCode],[LoanAmount],[DatetimeCreate],[DatetimeFinish],[SumLoanDate],[AssetName],[Customer_ID],[CustomerName],[Status])
	SELECT distinct 
		PawnID					AS PawnID
		,CodeNo					AS PawnCode
		,Loan					AS LoanAmount --MoneyPawn
		,CAST(replace(replace(replace(convert(varchar, DateExpired, 120), '-',''), ':',''), ' ','') AS bigint)			AS DatetimeCreate --ExtDate
		,CAST(replace(replace(replace(convert(varchar, _p0.Created, 120), '-',''), ':',''), ' ','') AS bigint)			AS DatetimeFinish --TranXLNDate
		,Overdue				AS SumLoanDate --Overdue
		,AssetName
		,_p0.CustomerID			AS CustomerID
		,_p3.[Name]				AS CustomerName
		--,ISNULL(_p2.ten_vv, '') AS CateName
		,_p0.STATUS				AS [Status]
	FROM POS_TEST.collect.Document _p0
		LEFT JOIN POS_TEST.pos.[User] _p1 ON _p0.SupportID = _p1.UserID
		LEFT JOIN POS_TEST.dbo.dmvv _p2 ON _p0.CategoryCode = _p2.ma_vv
		LEFT JOIN POS_TEST.pos.Customer _p3 ON _p0.CustomerID = _p3.CustomerID		













