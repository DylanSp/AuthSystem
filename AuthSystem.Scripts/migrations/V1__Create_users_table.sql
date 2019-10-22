CREATE TABLE Users(
	Id uuid PRIMARY KEY,
	Username text NOT NULL,
	SaltedHash text NOT NULL,

	UNIQUE (Username)
);
