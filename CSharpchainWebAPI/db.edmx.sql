
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/20/2020 22:09:53
-- Generated from EDMX file: D:\TLHT\NCKH\NCKH_BlockchainVote\CSharpchainWebAPI\db.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [admin_vote];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__tbl_dotba__ma_do__4CA06362]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_dotbaucu_ungcuvien] DROP CONSTRAINT [FK__tbl_dotba__ma_do__4CA06362];
GO
IF OBJECT_ID(N'[dbo].[FK__tbl_dotba__ma_un__5AEE82B9]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_dotbaucu_ungcuvien] DROP CONSTRAINT [FK__tbl_dotba__ma_un__5AEE82B9];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[dm_trangthai_dotbaucu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[dm_trangthai_dotbaucu];
GO
IF OBJECT_ID(N'[dbo].[tbl_dotbaucu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_dotbaucu];
GO
IF OBJECT_ID(N'[dbo].[tbl_dotbaucu_ungcuvien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_dotbaucu_ungcuvien];
GO
IF OBJECT_ID(N'[dbo].[tbl_taikhoan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_taikhoan];
GO
IF OBJECT_ID(N'[dbo].[tbl_ungcuvien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_ungcuvien];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'tbl_taikhoan'
CREATE TABLE [dbo].[tbl_taikhoan] (
    [ma_taikhoan] bigint IDENTITY(1,1) NOT NULL,
    [sHovaten] nvarchar(255)  NULL,
    [sTendangnhap] varchar(255)  NULL,
    [sMatkhau] varchar(255)  NULL,
    [dNgaysinh] datetime  NULL,
    [bGgioitinh] bit  NULL,
    [sEmail] varchar(255)  NULL,
    [sSdt] char(11)  NULL,
    [sDiachi] nvarchar(255)  NULL,
    [iTrangthai] bit  NULL,
    [ma_quyen] int  NULL,
    [ma_xacthuc] int  NULL
);
GO

-- Creating table 'tbl_dotbaucu'
CREATE TABLE [dbo].[tbl_dotbaucu] (
    [ma_dot] bigint IDENTITY(1,1) NOT NULL,
    [sTendot] nvarchar(255)  NULL,
    [dThoigianbd] datetime  NULL,
    [dThoigiankt] datetime  NULL,
    [sGhichu] nvarchar(1)  NULL,
    [sNoiDung] varchar(max)  NULL,
    [iTrangThai] int  NULL,
    [sHinhThuc] nvarchar(50)  NULL,
    [sSoPhieu] nvarchar(50)  NULL,
    [iNguoiTao] int  NULL
);
GO

-- Creating table 'dm_trangthai_dotbaucu'
CREATE TABLE [dbo].[dm_trangthai_dotbaucu] (
    [ma_dm_trangthai_dotbaucu] int  NOT NULL,
    [tenTrangThaiDotBauCu] nvarchar(255)  NOT NULL,
    [icon] varchar(50)  NULL,
    [title] nvarchar(255)  NULL
);
GO

-- Creating table 'tbl_dotbaucu_ungcuvien'
CREATE TABLE [dbo].[tbl_dotbaucu_ungcuvien] (
    [ma_ungcuvien] bigint  NOT NULL,
    [ma_dotbaucu] bigint  NOT NULL
);
GO

-- Creating table 'tbl_ungcuvien'
CREATE TABLE [dbo].[tbl_ungcuvien] (
    [ma_ungcuvien] bigint IDENTITY(1,1) NOT NULL,
    [sHoten] nvarchar(255)  NULL,
    [bGioitinh] bit  NULL,
    [dNgaysinh] datetime  NULL,
    [sEmail] nvarchar(255)  NULL,
    [sDiachi] nvarchar(255)  NULL,
    [ma_dotbaucu] bigint  NULL,
    [sGhichu] varchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ma_taikhoan] in table 'tbl_taikhoan'
ALTER TABLE [dbo].[tbl_taikhoan]
ADD CONSTRAINT [PK_tbl_taikhoan]
    PRIMARY KEY CLUSTERED ([ma_taikhoan] ASC);
GO

-- Creating primary key on [ma_dot] in table 'tbl_dotbaucu'
ALTER TABLE [dbo].[tbl_dotbaucu]
ADD CONSTRAINT [PK_tbl_dotbaucu]
    PRIMARY KEY CLUSTERED ([ma_dot] ASC);
GO

-- Creating primary key on [ma_dm_trangthai_dotbaucu] in table 'dm_trangthai_dotbaucu'
ALTER TABLE [dbo].[dm_trangthai_dotbaucu]
ADD CONSTRAINT [PK_dm_trangthai_dotbaucu]
    PRIMARY KEY CLUSTERED ([ma_dm_trangthai_dotbaucu] ASC);
GO

-- Creating primary key on [ma_ungcuvien], [ma_dotbaucu] in table 'tbl_dotbaucu_ungcuvien'
ALTER TABLE [dbo].[tbl_dotbaucu_ungcuvien]
ADD CONSTRAINT [PK_tbl_dotbaucu_ungcuvien]
    PRIMARY KEY CLUSTERED ([ma_ungcuvien], [ma_dotbaucu] ASC);
GO

-- Creating primary key on [ma_ungcuvien] in table 'tbl_ungcuvien'
ALTER TABLE [dbo].[tbl_ungcuvien]
ADD CONSTRAINT [PK_tbl_ungcuvien]
    PRIMARY KEY CLUSTERED ([ma_ungcuvien] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ma_dotbaucu] in table 'tbl_dotbaucu_ungcuvien'
ALTER TABLE [dbo].[tbl_dotbaucu_ungcuvien]
ADD CONSTRAINT [FK__tbl_dotba__ma_do__4CA06362]
    FOREIGN KEY ([ma_dotbaucu])
    REFERENCES [dbo].[tbl_dotbaucu]
        ([ma_dot])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__tbl_dotba__ma_do__4CA06362'
CREATE INDEX [IX_FK__tbl_dotba__ma_do__4CA06362]
ON [dbo].[tbl_dotbaucu_ungcuvien]
    ([ma_dotbaucu]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------