CREATE TABLE PermissionGrants(
	Id uuid PRIMARY KEY,
	UserId uuid NOT NULL REFERENCES Users(Id),
	ResourceId uuid NOT NULL REFERENCES Resources(Id),
	PermissionType integer NOT NULL
);
