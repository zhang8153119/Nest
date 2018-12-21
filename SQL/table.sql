if exists(select name from sysobjects where name='N_NestSet' and type='U')
	drop table N_NestSet
	go
create table N_NestSet 
(
nsID int primary key identity,
nsName varchar(128),
nsValue varchar(128)
)

go
delete from N_NestSet 
insert into N_NestSet values ('Protect','5')
insert into N_NestSet values ('Type','¾ØÐÎ')
insert into N_NestSet values ('Size','600')
insert into N_NestSet values ('T','10')
insert into N_NestSet values ('Pop','10')
insert into N_NestSet values ('Cross','0.8')
insert into N_NestSet values ('Mutation','0.09')
insert into N_NestSet values ('Rotate','ÊÇ')
insert into N_NestSet values ('Path','')