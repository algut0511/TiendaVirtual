create database tienda;
USE tienda;

create table personas (
id_persona	numeric(12),
nombre_persona	varchar(50)	not null,
apellido_persona	varchar(50) not null,
telefono_persona	numeric (10) not null,
email_persona	varchar(50) not null,
direccion_persona	varchar(100) not null,
contrasena	varchar(8),
tipo_persona	varchar(3),
constraint pk_personas primary key (id_persona, tipo_persona));

create table categorias(
id_categorias int	primary key	identity(1,1),
nombre_categoria	varchar(50)	not null,
id_categoria_padre int,
foreign key (id_categoria_padre) references categorias (id_categorias));

create table fabricantes(
id_fabricante int primary key identity(1,1),
nombre_fabricante	varchar(50) not null,
pais_fabricantes varchar(3) not null);

create table productos(
id_producto	int	primary key	identity(1,1),
nombre_producto	varchar(50) not null,
detalle_producto	varchar(300) not null,
precio_actual	numeric(10) not null,
unidades_disponibles	int	not null default(0),
id_categoria	int	not null,
id_fabricante	int		not null,
foreign key (id_categoria) references categorias (id_categorias),
foreign key (id_fabricante) references fabricantes (id_fabricante));

create table ventas(
id_venta int primary key identity (1,1),
fecha_venta date not null,
valor_iva	numeric(10) not null,
id_persona	numeric(12) not null,
tipo_persona	varchar(3) not null,
foreign key (id_persona,tipo_persona) references personas(id_persona,tipo_persona));

create table venta_productos(
id_venta_producto	int	primary key	identity(1,1),
id_venta	int	 not null,
id_producto	int not null,
precio_venta	numeric (10) not null,
cantidad	int	not null,
foreign key (id_venta) references ventas(id_venta),
foreign key (id_producto) references productos(id_producto));

create table dominios(
tipo_dominio	varchar(50),
id_dominio	varchar(3),
valor_dominio	varchar(50),
constraint	pk_dominios	primary key (tipo_dominio,id_dominio));
