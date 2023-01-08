CREATE TABLE [dbo].[Products] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (50) NOT NULL,
    [Description]    NVARCHAR (1024) NULL,
    [Price]          DECIMAL (18, 2)  NULL,
    [PictureUrl]     NVARCHAR (100) NULL,
    [ProductTypeId]  INT           NOT NULL,
    [ProductBrandId] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Products_ProductBrand_FK] FOREIGN KEY ([ProductBrandId]) REFERENCES [dbo].[ProductBrands] ([Id]),
    CONSTRAINT [Products_ProductTypes_FK] FOREIGN KEY ([ProductTypeId]) REFERENCES [dbo].[ProductTypes] ([Id])
);







