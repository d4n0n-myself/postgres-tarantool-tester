## Benchmarking

The next step to take was to make a benchmarks on Postgres and Tarantool, and compare it. 

### Prerequisites

Let's start with defining a topology in Amazon Cloud: we've got a 2 servers in 1 availibility zone (us-east-1c):
* 1st server to contain databases;
* and 2nd server to contain a benchmarker and a service connected to databases.

![Architecture](https://github.com/d4n0n-myself/postgres-tarantool-tester/blob/master/blog/Architecture.png)

#### Benchmarker

As a benchmarker I've chosen Yandex.Tank - simple and effecient tool written in Python to benchmark something via HTTP/TCP requests. 
To read more about it, please head to [documentation](https://yandextank.readthedocs.io/en/latest/index.html).

#### Service

Moving to service - I've written a simple .NET Core service that provides CRUD operations both on Postgres and Tarantool.
This service is based on a most known connectors to databases in .NET - Npgsql and Progaudi.Tarantool
(I've mentioned it in a blogs earlier - this connector is almost the only one to be maintained to actual versions 
of Tarantool and still in development).
On server, service is installed in a Docker container. 

#### Model and types in Postgres

As a model I took a basic model Author that consists of 10 properties:
```namespace IntegrationService.Models.Entities
{
	[MsgPackArray]
	public class Author : BaseEntity
	{
		[MsgPackArrayElement(0)] public long? Id { get; set; }
		[MsgPackArrayElement(1)] public string Name { get; set; }
		[MsgPackArrayElement(2)] public string FamilyName { get; set; }
		[MsgPackArrayElement(3)] public string MiddleName { get; set; }
		[MsgPackArrayElement(4)] public byte Age { get; set; }
		[MsgPackArrayElement(5)] public string BirthCountry { get; set; }
		[MsgPackArrayElement(6)] public string BirthCity { get; set; }
		[MsgPackArrayElement(7)] public byte BooksCount { get; set; }
		[MsgPackArrayElement(8)] public string Nationality { get; set; }
		[MsgPackArrayElement(9)] public DateTime BirthDateTime { get; set; }
	}
}
```

The property Id is especially set as nullable int so I would have a support of sequences in both databases. 
Attributes define the order of properties to serialize object to MsgPack format.

Table in Postgres will look like this: 
```
create table if not exists "Authors"
(
	"Id" serial unique primary key,
	"Name" text,
	"FamilyName" text,
	"MiddleName" text,
	"Age" smallint not null,
	"BirthCountry" text,
	"BirthCity" text,
	"BooksCount" smallint not null,
	"Nationality" text,
	"BirthDateTime" timestamp not null
);
```
PK_Authors stands for unique constraint.

### Benchmarks

Yandex.Tank provides simple configurations to move results of benchmarks to Yandex.Overload. 
This service will help us by generating charts based on Tank results.

#### Writes

I've decided to start with writes to databases. 
First thing to do was to write a HTTP POST request that will address service insert methods. 
This format is required by Yandex.Tank and may be referenced as 'ammo'.
It will look like this:
```
[Host: example.com]
[Connection: close]
[User-Agent: Tank]
[Content-Type: application/json]
214 /Author/Save/
{
  "name": "alexander",
  "familyName": "pushkin", 
  "middleName": "sergeevich", 
  "age": 20, 
  "birthCountry": "russia", 
  "birthCity": "moscow", 
  "booksCount": 10, 
  "nationality": "russian", 
  "birthDateTime": "2020-05-24T13:09:57.635Z"
}
```
I made a few benchmarks to define the max value of rps and instances(threads) 
that databases could handle without losing any data, connections or reaching a timeouts.
Tarantool cases:
* [1 to 300 rps](https://overload.yandex.net/275479)
* [1 to 400 rps](https://overload.yandex.net/275485)
* [400 to 800 rps](https://overload.yandex.net/275488)
* [800 to 3000 rps](https://overload.yandex.net/275489)

Based on this tests, I assumed that Tarantool's base rps value that can be handled 
is near 1000 rps and made a [postgres test up to 1000 rps](https://overload.yandex.net/275494).
This showed me that Postgres average rps that can be handled is around 450 rps. 
I made longer stress tests to ensure rps values are stable:
* [Postgres 1 to 500 rps, 12 min long](https://overload.yandex.net/275124)
* [Tarantool 1 to 1000 rps, 15 min long](https://overload.yandex.net/275491)
* [Additional Tarantool test: contant 1000 rps, 15 min long](https://overload.yandex.net/275493)

#### Reads

Request for a read would look just like a URL:
```
https://ec2-*service-address*/Author/Save?Id=1
```

Based on writes values I made next tests:
* [Tarantool 700 to 2000 rps, 15 min long](https://overload.yandex.net/275733)
* [Tarantool 1 to 1000 rps, 15 min long](https://overload.yandex.net/275724)
* [Tarantool 700 to 1000 rps, 15 min long](https://overload.yandex.net/275760)
* For comparison, [Postgres 1 to 1000 rps, 15 min long](https://overload.yandex.net/275770)

### Conslusion

This benchmarks showed that Tarantool on a vinyl drive (drive that makes IOPs from storage) is showing a great performance.
But, I would like to spread some thoughts on this results:
* This could be a not complete and not maximum peak RPS value to be handled by Tarantool due to connector and database development process.
* This results could be influenced by Amazon virtual machines load, networks load, etc. (This includes both Tarantool and Postgres) 
I monitored the CPU utilization logs on a database server, it showed 40% at maximum peak.
* My own erroneous use of the Tarantool connector. 

Therefore, we could potentially see a raise of RPS value to Tarantool through .NET.

