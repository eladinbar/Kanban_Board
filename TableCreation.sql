CREATE TABLE "Users" (
	"email"	TEXT NOT NULL,
	"Password"	TEXT NOT NULL,
	"Nickname"	TEXT NOT NULL,
	PRIMARY KEY("email")
);

CREATE TABLE "Boards" (
	"email"	TEXT NOT NULL,
	"TaskCounter"	INTEGER NOT NULL,
	PRIMARY KEY("email")
	FOREIGN KEY (email)
		REFERENCES Users (email)
);

CREATE TABLE "Columns" (
	"email"	TEXT NOT NULL,
	"Name" TEXT NOT NULL,
	"Ordinal"	INTEGER NOT NULL,
	"Limit"	INTEGER NOT NULL,
	PRIMARY KEY("email","Name")
	FOREIGN KEY (email)
		REFERENCES Boards (email)
);

CREATE TABLE "Tasks" (
	"email"	TEXT NOT NULL,
	"ColumnName"	TEXT NOT NULL,
	"Id"	INTEGER NOT NULL,
	"Title"	TEXT NOT NULL,
	"Description"	TEXT,
	"DueDate"	INTEGER NOT NULL,
	"CreationDate"	INTEGER NOT NULL,
	"LastChangedDate"	INTEGER NOT NULL,
	FOREIGN KEY("email") REFERENCES "Columns"("email"),
	FOREIGN KEY("ColumnName") REFERENCES "Columns"("Name"),
	PRIMARY KEY("email","ColumnName","Id")
);

CREATE TABLE "Log" (




);
