create database admin_vote


use admin_vote

-- sha256
-- SELECT HASHBYTES('SHA2_256','hungoccho')


/*
-- user 
	
	ma_cb
	tendangnhap
	matkhau
	hoten
	ngaysinh
	gioitinh
	email
	sdt
	diachi
	trangthai
	ma_quyen

*/
/*

-- dotbaucu  
	ma_dot
	tendot
	thoigianbd
	thoigiankt
	ghichu

*/

/*
-- cu tri

	ma_cutri
	hoten
	gioitinh
	ngaysinh
	email
	diachi
	ma_dotbaucu

*/

/*
	-- phieubau (transaction)
*/

/*
	phanquyen
*/

-- cteate table
create table tbl_taikhoan(
	ma_taikhoan bigint identity(1,1) primary key not null,
	sHovaten nvarchar(255),
	sTendangnhap varchar(255),
	sMatkhau varchar(255),
	dNgaysinh date,
	bGgioitinh bit,
	sEmail varchar(255),
	sSdt char(11), 
	sDiachi nvarchar(255),
	iTrangthai bit,
	ma_quyen int
);

create table tbl_dotbaucu(
	ma_dot bigint identity(1,1) primary key not null,
	sTendot nvarchar(255),
	dThoigianbd datetime,
	dThoigiankt datetime,
	sGhichu	nvarchar
);

create table tbl_cutri(
	ma_cutri bigint identity(1,1) primary key not null,
	sHoten nvarchar(255),
	bGioitinh bit,
	dNgaysinh date,
	sEmail varchar(255),
	sDiachi varchar(255),
	ma_dotbaucu bigint  
);

create table tbl_dotbaucu_cutri(
	ma_cutri bigint foreign key references tbl_cutri(ma_cutri),
	ma_dotbaucu bigint foreign key references tbl_dotbaucu(ma_dot),
	primary key (ma_cutri, ma_dotbaucu)
);

create proc sp_checklogin
	@sTendangnhap varchar(255),
	@sMatKhau varchar(255),
	@ma_taikhoan bigint output,
	@ma_quyen bigint output
as
begin
	if exists (select sTendangnhap, sMatkhau, ma_quyen from tbl_taikhoan
	where sTendangnhap = @sTenDangNhap and sMatkhau = @sMatKhau)
	begin
		set @ma_quyen = (select ma_quyen from tbl_taikhoan where sTendangnhap = @sTendangnhap)
		set @ma_taikhoan = (select ma_taikhoan from tbl_taikhoan where sTendangnhap = @sTendangnhap)
	end
	else
	begin 
		set @ma_taikhoan = 0
	end
end

exec sp_checklogin '_thinguyen', '1'

create proc sp_them_dot_baucu
	@sTendot nvarchar(255),
	@dThoigianbd datetime,
	@dThoigiankt datetime,
	@sGhichu nvarchar(255)
as
begin 
	insert into tbl_dotbaucu (sTendot, dThoigianbd, dThoigiankt, sGhichu) values
	(@sTendot, @dThoigianbd, @dThoigiankt, @sGhichu)
end

exec sp_them_dot_baucu 'Test', '2000-02-03', '2000-02-04', '1'

select * from tbl_dotbaucu