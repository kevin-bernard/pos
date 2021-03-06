/****** Script for SelectTopNRows command from SSMS  ******/

SELECT MAX(invdate) issuedate, MAX(invdate) edtdatetime from [ESQ+].[dbo].[artran]
where itemcode = '019-C0517';

SELECT SUM(qtyorder) salesynqty from [ESQ+].[dbo].[artran]
where itemcode = '019-C0517';

SELECT  issuedate,
		edtdatetime,
		salesynqty
  FROM [ESQ+].[dbo].[icitemqty]
  
  where itemcode = '019-C0517'