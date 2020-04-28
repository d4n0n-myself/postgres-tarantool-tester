
# Types mapping and functions

Second thing to do to move from PostgreSQL to Tarantool is to ensure that every business logic element could be implemented on Tarantool layer. 
This task I split in two:
1) Ensure that Tarantool could support types that are used in Postgres;
2) Ensure that Tarantool could support functions (or other functional elements, like triggers and etc.).

I felt second part more important, so I began research on this task from looking into functions in Tarantool. 

## Mapping of functionality

### SQL Way

First of all, as I said in first part of my blog, Tarantool support SQL queries (not plpgsql). So, if we have a native SQL query in our business application,
first thing to try is to execute SQL version of a query straight-forward on Tarantool.

That might not work out as planned - some queries might be out of support, some may come from plpgsql and lose plpgsql features. 
If you interested in SQL queries that Tarantool support, i suggest diving into [this](https://www.tarantool.io/en/doc/2.3/book/sql/) page.
It also could provide you with information that you could use to transform your SQL to run in Tarantool (in example: position function require arguments split by comma, not a 'IN' keyword).

If that did not work out as well, there is another way.

### Tarantool way

As I stated in the first part of my blog, Tarantool is written on Lua language. 
Because of that, Tarantool, besides being a NewSQL database, supports capabilities of Lua using a Lua application server. (**NewSQL** would mean that a database would support ACID principles from RDBMS, NoSQL's horizontal scalability and OLTP workload traffic support.)

In this sight, if you did not managed to transform your SQL query to run in Tarantool, this is your way to implement it.
It is also a way of implementing a functions in Tarantool - in Lua. To mention, Tarantool also support [transactions](https://www.tarantool.io/en/doc/2.2/book/box/atomic/) so it could provide atomicity of operations.

### Triggers

A big part of a production database - triggers - are also supported by Tarantool.
We use them very often in our production databases, they support our data flow and provide consistency of data, so I dig into this, 
and this triggers are not the same to ones we use in Postgres. 

First of all, Tarantool triggers (also could be referenced as callbacks) categorized by the ones that support **box events** (box is basicly a connection ``facade``, or some kind of a ``database referencing object``) and **data events** (that would be bulk operations - insert, update, delete, etc.)
Tarantool treats triggers like callbacks, so:
1) triggers must have a context, like an event handler does;
2) to add a trigger, you must pass a function pointer to an event callback function. Example:
``box.session.on_connect(***your function name here***)``

This pattern will be familiar to you if you are familiar to events in OOP languages (that have events support, like C#) or 'callback' languages - as i call them - Javascript would be an example for that category.

Second huge difference is that triggers **ARE NOT CONSISTENT** in Tarantool. What i mean by that is basically that because Tarantool treats triggers as some kind of event handlers, Tarantool store it as an **IN-MEMORY** object.

But that would not be a much of problem - Tarantool provide an initialization script, so when you would start your ``cluster`` - your triggers would be restored.

If you want to know more about Triggers in Tarantool, I suggest you start from [this](https://www.tarantool.io/en/doc/2.2/book/box/triggers/) page.

## Type mapping

Moving to type mapping, first thing to know would be "How Tarantool stores its tuples".

### Tarantool types and storage format

To begin, we must remember, that Tarantool is a database and an application server in one. Application server operates Lua types, and database operates MsgPack tuples.
MsgPack is a ``lightweight`` JSON, if i may call it that way. To understand this format, lets look at the example, that [main page](https://msgpack.org/index.html) of MsgPack provide:

![msgpack image](https://github.com/d4n0n-myself/postgres-tarantool-tester/blob/master/blog/msgpack.png)

As we can see, this image represent a JSON object of 27 bytes that contain 2 fields of boolean and integer. MsgPack format would represent it this way:
1) First element ``82`` would provide data of type and element count - 2-element map;
2) Second represent a type of key - string in this case, could be integer for an array - and byte count to store it. 
3) Value itself. MsgPack format is variable-length, so the key would be 7 bytes, and value would be only 1 byte, as mentioned in [spec](https://github.com/msgpack/msgpack/blob/master/spec.md). It also works on other types - so smallest number will require only a byte, and largest - 9 bytes.

As we handled that, we now know that Tarantool stores tuples in MsgPack format.
Therefore, a Tarantool developer has a two sets of types: in Lua, to operate data in a server, and in MsgPack, to store data.
List of supported MsgPack/Lua types:

![type image](https://github.com/d4n0n-myself/postgres-tarantool-tester/blob/master/blog/types.png)

### Mapping from PG to TR

The next move is to provide a mapping from Postgres types to both of Tarantool's. 
To cover most business models, i have ended up with a list of types that i have to map to support this models.
It would be inconvinient to drop an image of full list here, so you are welcome to visit [this page](https://docs.google.com/spreadsheets/d/1smuQLYcguPEIzhOnHiYWnDdzeSULZImsyKfavvEyASc/edit?usp=sharing).



## Extension: Feature of persistence

It is not stated in a task, but i would like to mention it as a great feature of Tarantool.
When reading a Tarantool documentation, i ran into the Persistence chapter and found something familiar - Tarantool writes a WAL (Write Ahead Log) files to persist in-memory data, so it would be a familiar process to backup a data. 

WAL files bring a solution to all situations, including situations that bring the data loss and it has a big value in the world of in-memory database. But, starting from a WAL file to get data in memory would bring a ``cold`` start, but Tarantool is positioned as a fast-to-start database, so developers come up with a solution of in-memory snapshot - when a database starts, it reads the most amount of data from a snapshot on drive, and proceed to the last operation through WAL file, taking much less time to startup.

This persistence feature reminded me of possibility of PITR(Point-In-Time-Recovery) and made me realise that Tarantool developers aim to decrease RPO (Recovery Point Objective, period of time, which transactions would be lost, a period from a latest backup to database failure) and RTO (Recovery Time Objective, SLA to start from recovery) which is very important objectives in modern DBs.

That is the end of part 2.
