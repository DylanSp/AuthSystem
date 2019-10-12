CREATE TABLE Users(
	Id uuid PRIMARY KEY,
	Username text NOT NULL,
	Base64PasswordHash text NOT NULL,
	Base64Salt text NOT NULL
);
