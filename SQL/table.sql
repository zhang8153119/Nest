if exists(select name from sysobjects where name='N_NestSet' and type='U')
	drop table N_NestSet
	go
create table N_NestSet 
(
nsID int primary key identity,
nsName varchar(128),
nsValue varchar(128)
)