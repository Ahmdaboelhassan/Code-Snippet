-- Remove Dublicated Values

WITH DuplicateRecords AS (
    SELECT 
        *,
        ROW_NUMBER() OVER (PARTITION BY item_NameEn ORDER BY (SELECT 1)) AS RowNum
        -- PARTITION BY item_NameEn This line Group Items With Name And Numbering Them
    FROM 
        [dbo].[IC_Item]
)
delete from DuplicateRecords
WHERE RowNum > 1; -- Get The First One And Remove Other