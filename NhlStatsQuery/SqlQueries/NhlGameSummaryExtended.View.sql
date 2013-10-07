CREATE VIEW [dbo].[NhlGameSummaryExtended]
	AS SELECT  *, datename(dw, Date) as Weekday, MONTH(Date) as Month FROM NhlGameSummary
