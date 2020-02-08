CREATE TABLE SessionCookies(
	Id uuid PRIMARY KEY,
	UserId uuid NOT NULL REFERENCES Users(Id)
);
