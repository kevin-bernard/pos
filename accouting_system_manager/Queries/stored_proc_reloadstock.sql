CREATE PROCEDURE [dbo].[reloadstock]
	(@itemcode nvarchar(25), @qtyorder int, @is_rollback int)
AS
BEGIN
	SET NOCOUNT ON;
    
    DECLARE @id INT
    DECLARE @tranqty INT
    DECLARE @totaltran INT
    DECLARE @continue INT
    DECLARE @commands CURSOR
	
	IF @is_rollback = 1
	BEGIN
		SET @continue = 0;
		UPDATE ic SET ic.tranqty=(ic.tranqty + @qtyorder), ic.stockqty=(ic.tranqty + @qtyorder) FROM ictran ic WHERE trantype='R' AND tranno=(SELECT MAX(tranno) FROM ictran ic2 where ic2.itemcode=@itemcode AND trantype='R')
	END
	ELSE
	BEGIN
		SET @continue = 1;
	END
	
    

    SET @commands = CURSOR FOR
		SELECT ic.tranno, ic.tranqty, (SELECT COUNT(*) FROM ictran ic2 WHERE ic2.itemcode = ic.itemcode AND ic2.trantype = 'R') total 
		FROM ictran ic 
		WHERE ic.trantype='R' AND ic.itemcode=@itemcode
		ORDER BY trandate DESC
	
    OPEN @commands
    FETCH NEXT
    FROM @commands INTO @id, @tranqty, @totaltran
    WHILE @@FETCH_STATUS = 0 AND @continue = 1
    BEGIN
		
        INSERT INTO logs VALUES('stock: ' + CAST(@tranqty AS VARCHAR) + ' / order: ' + CAST(@qtyorder AS VARCHAR) + ' / total stock: ' + CAST(@totaltran AS VARCHAR))

        IF @tranqty > @qtyorder
        BEGIN
            SET @tranqty = @tranqty - @qtyorder;
            SET @qtyorder = 0;

            UPDATE ictran SET stockqty=@tranqty, tranqty=@tranqty WHERE tranno=@id;
        END
        ELSE
        BEGIN
            IF @totaltran = 1
            BEGIN
                UPDATE ictran SET stockqty=0, tranqty=0 WHERE tranno=@id;
                SET @qtyorder = 0;
            END
            ELSE
            BEGIN
                 SET @qtyorder = @qtyorder - @tranqty;
                 DELETE FROM ictran WHERE tranno=@id;
            END
        END


        IF @qtyorder > 0
        BEGIN
			PRINT 'NEXT'
            FETCH NEXT
            FROM @commands INTO @id, @tranqty, @totaltran
        END
        ELSE SET @continue = 0;
    END
    
	INSERT INTO logs VALUES('end')

    CLOSE @commands
    DEALLOCATE @commands
END