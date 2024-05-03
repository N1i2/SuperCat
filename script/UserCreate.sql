--Create database SuperCat
go
use SuperCat

Create table Users(
id int primary key  not null,
nikname varchar(30) not null,
password varchar(15) not null);
