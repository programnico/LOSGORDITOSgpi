create database test1gpi;
go
use test1gpi;
go

create table supply(
	codSupply int not null primary key identity (1,1),
	nameSupply varchar(100) not null unique,
	stock decimal(18,2), -- start at 0 and update when suppliesEntry changes quantity, changed if you use ozs to decimal
	enable bit not null, -- 1 true, 0 false
);

create table unitMeasurement(
	codUnitMeasurement int not null primary key identity(1,1),
	nameUnitMeasurement varchar(50) unique, -- pounds, quintal 
);

create table supplyMovement(
	codSupplyMovement int not null primary key identity(1,1),
	codSupply int not null,
	quantity decimal(18,2) not null, -- is used in pounds by default
	tipo int not null, -- 1 entry, 2 recall
);

create table product(
	codProduct int not null primary key identity(1,1),
	nameProduct varchar(100) not null unique,
);

create table productUnitMeasurement(
	codProductUnitMeasurement int not null primary key identity(1,1),
	codProduct int not null,
	codUnitMeasurement int not null,
);

-- setting in pounds by default
create table settingProductSupply(
	codSettingProductSupply int not null primary key identity(1,1),
	-- codProduct int not null,
	codSupply int not null,	
	quantity int not null,
	codProductUnitMeasurement int not null,
); 

create table roles(
	codRole int not null primary key identity(1,1),
	nameRole varchar(50),
);

create table person(
	codPerson int not null primary key identity(1,1),
	codRole int not null,
	namePerson varchar(100) not null,
	email varchar(100) not null,
	password varchar(100) not null,
);

create table orders(
	codOrder int not null primary key identity(1,1),
	codPerson int not null,
	dateOrder date,
	status int not null, -- 1 create, 2 process 3 finished, 4 delivered, 
);

create table ordersDetail(
	codOrderDetail int not null primary key identity(1,1),
	codOrder int not null,	
	codProduct int not null,
	quantity int not null,
);

create table sales(
	codSale int not null primary key identity(1,1),
	dateSale date,
	codOrder int not null,
	price decimal(18,2) 
);

go

alter table supplyMovement add constraint fk_codSupply foreign key (codSupply) references supply (codSupply);
alter table settingProductSupply add constraint fk_codSupply_spd foreign key (codSupply) references supply (codSupply);
-- alter table settingProductSupplies add constraint fk_codProduct_spd foreign key (codProduct) references product (codProduct);
alter table settingProductSupply add constraint fk_codProductUnitMeasurement foreign key (codProductUnitMeasurement) references productUnitMeasurement (codProductUnitMeasurement);
alter table productUnitMeasurement add constraint fk_codProduct_pum foreign key (codProduct) references product (codProduct);
alter table productUnitMeasurement add constraint fk_codUnitMeasurement_pum foreign key (codUnitMeasurement) references unitMeasurement (codUnitMeasurement);
alter table person add constraint fk_codRole foreign key (codRole) references roles (codRole);
alter table orders add constraint fk_codPerson foreign key (codPerson) references person (codPerson);
alter table ordersDetail add constraint fk_codOrder foreign key (codOrder) references orders (codOrder);
alter table ordersDetail add constraint fk_codProduct_od foreign key (codProduct) references product (codProduct);
alter table sales add constraint fk_codOrder_s foreign key (codOrder) references orders (codOrder);


-- insert data
insert into roles values ('ADMINISTRADOR'),('SECRETARIA'), ('OPERADOR');

select * from roles;

insert into unitMeasurement values ('QUINTAL'),('LIBRA');

select * from unitMeasurement;

select * from supplyMovement;
select * from supply

select * from productUnitMeasurement;
