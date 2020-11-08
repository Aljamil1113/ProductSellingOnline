/******CREATE******/
CREATE PROCEDURE [dbo].[CreateProduct]
	-- Add the parameters for the stored procedure here
	@name nvarchar(MAX),
	@price decimal(18, 2),
	@available bit,
	@image nvarchar(MAX),
	@shadeColor nvarchar(MAX),
	@productTypeId int,
	@specialTagId int,
	@quantity int,
	@id int output
AS
BEGIN
    --set IDENTITY_INSERT Products ON

	INSERT Products(Name, Price,Available,Image,ShadeColor,ProductTypeId,SpecialTagId,Quantity) 
	values (@name, @price, @available, @image, @shadeColor, @productTypeId, @specialTagId, @quantity)
	SET @id=SCOPE_IDENTITY()
	RETURN @id
	--SELECT SCOPE_IDENTITY()

	--set IDENTITY_INSERT Products OFF
END
GO
/*******************************************************/

/*******READ ALL *******/
CREATE PROCEDURE [dbo].[SelectProducts]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from Products
END
GO
/*******************************************************/

/*****READ BY ID *****/
CREATE PROCEDURE [dbo].[SelectProductByID] 
	-- Add the parameters for the stored procedure here
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from Products where Id = @id
END
GO
/*******************************************************/

/******UPDATE********/
CREATE PROCEDURE [dbo].[UpdateProduct] 
	-- Add the parameters for the stored procedure here
	@id int,
	@name nvarchar(MAX),
	@price decimal(18, 2),
	@available bit,
	@image nvarchar(MAX),
	@shadeColor nvarchar(MAX),
	@productTypeId int,
	@specialTagId int,
	@quantity int
AS
BEGIN
	UPDATE Products
	set Name = @name,
	Price = @price,
	Available = @available,
	Image = @image,
	ShadeColor = @shadeColor,
	ProductTypeId = @productTypeId,
	SpecialTagId = @specialTagId,
	Quantity = @quantity
	where Id = @id
END
GO
/*******************************************************/

/*******DELETE******/
CREATE PROCEDURE [dbo].[DeleteProduct]
	-- Add the parameters for the stored procedure here
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE from Products where Id = @id
END
GO
/*******************************************************/