-- Set : Define All Possible Group By 
SELECT Category_Id, Itemtype, COUNT(*) as Count 
FROM [Zayed110820240406].[dbo].[IC_Item]
GROUP BY GROUPING SETS (
    (Category_Id), --Group by Category_Id  Only And Represnt The Total
    (Itemtype), --Group by Itemtype Only And Represnt The Total
    (Category_Id, Itemtype), --Group by Both And Represnt The Total
    () -- Grand Total
)

Category_Id	Itemtype	Count
	10	0	2
	1002	0	808
	2002	0	1
--NULL	0	1764  #Group by Itemtype	= 0
	10	1	1
	1002	1	15
	2002	1	62    
--10	NULL	3   #Group by Category_Id	= 10
	1002	NULL	836
	2002	NULL	64
--NULL	NULL	3562  #Grand Total
