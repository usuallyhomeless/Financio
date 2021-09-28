create database financio;
use financio;

create table financiouser(
id int primary key identity,
name varchar(50) not null,
phone varchar(13) not null,
email varchar(50) not null unique,
username varchar(20) not null unique,
password varchar(30) not null, 
dob date not null,
address varchar(100) not null,
isadmin bit default 0
);


create table cardtype(
id int primary key identity,
cardname varchar(20) not null,
charge decimal not null,
creditlimit decimal not null
);

create table bank(
id int primary key identity,
bankname varchar(30) not null unique
);


create table product(
id int primary key identity,
productname varchar(50) not null, 
productdetails text not null,
cost money not null,
imageurl text not null,
extrafeatures text 
);

create table scheme(
id int primary key identity,
schemename varchar(20) not null,
schemeduration int not null,
)

create table ifsc(
id int primary key identity,
ifsccode varchar(20) not null unique,
bankid int foreign key references bank(id)
)

create table card(
id int primary key identity,
financiouser int foreign key references financiouser(id),
registrationdate date not null,
validupto date not null,
cardlimit money default 0,
isactive bit default 0,
cardtypeid int foreign key references cardtype(id) not null,
bankid int foreign key references bank(id) not null,
accountnumber varchar(30) not null unique,
ifscid int foreign key references ifsc(id) not null,
)

create table debittransaction(
id int primary key identity,
financiouser int foreign key references financiouser(id),
productid int foreign key references product(id),
schemeid  int foreign key references scheme(id),
transactiondatetime datetime default current_timestamp,
installmentamount money default 0,
lastpaymentdatetime datetime null,
balanceleft money default 0,
isactive bit default 0,
)

create table credittransaction(
id int primary key identity,
debittransactionid int foreign key references debittransaction(id),
amount money default 0,
transactiondatetime datetime default current_timestamp
)


-- card type values
insert into cardtype values ('Gold', 500, 50000), ('Titanium', 1000, 100000);

-- bank values
insert into bank values ('HDFC'), ('ICICI'), ('KOTAK'), ('SBI'), ('CITI');

-- IFSC VALUES

insert into ifsc SELECT 'HDFC001', Id from bank where bankname='HDFC';
insert into ifsc SELECT 'HDFC002', Id from bank where bankname='HDFC';
insert into ifsc SELECT 'HDFC003', Id from bank where bankname='HDFC';
insert into ifsc SELECT 'HDFC004', Id from bank where bankname='HDFC';
insert into ifsc SELECT 'HDFC005', Id from bank where bankname='HDFC';

insert into ifsc SELECT 'ICICI001', Id from bank where bankname='ICICI';
insert into ifsc SELECT 'ICICI002', Id from bank where bankname='ICICI';
insert into ifsc SELECT 'ICICI003', Id from bank where bankname='ICICI';
insert into ifsc SELECT 'ICICI004', Id from bank where bankname='ICICI';
insert into ifsc SELECT 'ICICI005', Id from bank where bankname='ICICI';

insert into ifsc SELECT 'KOTAK001', Id from bank where bankname='KOTAK';
insert into ifsc SELECT 'KOTAK002', Id from bank where bankname='KOTAK';
insert into ifsc SELECT 'KOTAK003', Id from bank where bankname='KOTAK';
insert into ifsc SELECT 'KOTAK004', Id from bank where bankname='KOTAK';
insert into ifsc SELECT 'KOTAK005', Id from bank where bankname='KOTAK';

insert into ifsc SELECT 'SBI001', Id from bank where bankname='SBI';
insert into ifsc SELECT 'SBI002', Id from bank where bankname='SBI';
insert into ifsc SELECT 'SBI003', Id from bank where bankname='SBI';
insert into ifsc SELECT 'SBI004', Id from bank where bankname='SBI';
insert into ifsc SELECT 'SBI005', Id from bank where bankname='SBI';

insert into ifsc SELECT 'CITI001', Id from bank where bankname='CITI';
insert into ifsc SELECT 'CITI002', Id from bank where bankname='CITI';
insert into ifsc SELECT 'CITI003', Id from bank where bankname='CITI';
insert into ifsc SELECT 'CITI004', Id from bank where bankname='CITI';
insert into ifsc SELECT 'CITI005', Id from bank where bankname='CITI';


--product values
insert into product values('Earphones' , 'Earphones are a small piece of equipment which you wear over or inside your ears so that you can listen to music' , 1000 , 'assets/images/earphones.jpg', 'Colour : Blue, Type : Wired, Model Name : Stringz-38');
insert into product values('Retro Television' , 'Old Television with no DTH cable slot' , 2000 , 'assets/images/retrotelevision.jpg', 'Colour : Black, Screen size : 4.3 Inches, Display : CRT');
insert into product values('Phone' , 'Iphone with Solid build quality and Excellent display' , 60000 , 'assets/images/iphone.jpg', 'Colour : Gray, Screen size : 6.1 Inches, OS : IOS 14');
insert into product values('Printer' , 'Printer with Full HD Movie Print available' , 10000 , 'assets/images/printer.jpg', 'Colour : Black, Model Name : G30 NV, Output : Colour');
insert into product values('Microwave oven' , '20-25L capacity oven with 850W power' , 30000 , 'assets/images/microwave.jpg', 'Colour : Black, Material : Ceramic, Weight : 12000 Grams');
insert into product values('Camera' , 'Single-lens reflex digital camera' , 90000 , 'assets/images/camera.jpg', 'Colour : Black, Lens Type : Zoom, Shooting modes : Manual');
insert into product values('Washing Machine' , '6Kg Fully Automatic Front Load' , 20000 , 'assets/images/washingmachine.jpg', 'Colour : Silver, Capacity : 6.5 Kg, Warranty : 2 years');
insert into product values('Router' , ' 300Mbps ADSL2 Wireless with Modem Router' , 2000 , 'assets/images/router.jpg', 'Colour : Black, Voltage : 220 Volts, No. of ports : 7');
insert into product values('Dishwasher' , '45 kg WeightButton Control with Stainless-Steel Basket ' , 40000 , 'assets/images/dishwasher.jpg', 'Colour : Black, Form factor : Freestanding, Brand : Faber');
insert into product values('Laptop' , '15.6 Inch, Core i5 8th Gen, 8 GB RAM' , 80000 , 'assets/images/laptop.jpg', 'Colour : Black, OS : Windows 10, Processor Count : 4');
insert into product values('Headphones' , 'On-the-ear headphone' , 2000 , 'assets/images/headphones.jpg', 'Colour : Black, Weight : 168 Grams, Control Type : Media');
insert into product values('Smart TV' , 'QLED 55 inch Screen UHD TV' , 80000 , 'assets/images/smarttv.jpg', 'Colour : Black, Weight : 168 Grams, Control Type : Media');

-- scheme values
insert into scheme values('3 Months ',3),('6 Months ',6),('9 Months ',9),('12 Months ',12)

-- admin user
insert into financiouser values ('admin', '', 'admin@gmail.com', 'admin', 'adminpass', '2000-12-31', '', 1);

-- reboot method
delete from credittransaction;
delete from debittransaction;
delete from card;
delete from financiouser where name != 'admin';
select * from financiouser;

insert into financiouser values ('Ramesh Jha', '7827231289', 'care.financio@gmail.com', 'iamramesh', 'Password123@', '1975-01-26', 'C 104, Daffodil Towers, Malad West, Mumbai');