CREATE TRIGGER [dbo].[T_InsertItemTranTmp]
       ON  [dbo].[itemtrantmp]
       AFTER INSERT
    AS 
    BEGIN
        
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for trigger here
        DECLARE @qtyorder INT
        DECLARE @is_rollback INT
        DECLARE @itemcode nvarchar(25)
		DECLARE @inserted CURSOR
		
        SET @inserted = CURSOR FOR
			SELECT qtyorder, itemcode, is_rollback 
			FROM INSERTED
	
		OPEN @inserted
		FETCH NEXT
		FROM @inserted INTO @qtyorder, @itemcode, @is_rollback
		WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT INTO logs VALUES('trigger: ' + @itemcode + ' / ' + CAST(@qtyorder AS VARCHAR) + ' -- ' + CAST(@is_rollback AS VARCHAR))
			
            EXECUTE reloadstock @itemcode, @qtyorder, @is_rollback
			
			FETCH NEXT
            FROM @inserted INTO @qtyorder, @itemcode, @is_rollback
		END
		
        CLOSE @inserted
		DEALLOCATE @inserted
    END;