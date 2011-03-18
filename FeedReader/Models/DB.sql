USE FeedReader;

-- CLEANUP
DROP TABLE UserFeedItem;
DROP TABLE UserFeed;
DROP TABLE FeedItem;
DROP TABLE Feed;
DROP TABLE [User];

-- TABLES
CREATE TABLE [User]
(
	ID INT IDENTITY PRIMARY KEY,
	Email VARCHAR(130) NOT NULL UNIQUE,
	PasswordHash CHAR(40) NOT NULL,
	Registered DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE Feed
(
	ID INT IDENTITY PRIMARY KEY,
	Title VARCHAR(35),
	Description VARCHAR(2048),
	Url VARCHAR(500) UNIQUE,
	LastChecked DATETIME NOT NULL DEFAULT DATEADD(MINUTE, -15, GETDATE())
);

CREATE TABLE FeedItem
(
	ID INT IDENTITY PRIMARY KEY,
	FeedID INT REFERENCES Feed(ID),
	Title VARCHAR(200),
	Description VARCHAR(MAX),
	Url VARCHAR(500),
	Created DATETIME NOT NULL DEFAULT GETDATE(),
	UNIQUE (FeedID, Url)
);

CREATE TABLE UserFeed
(
	UserID INT REFERENCES [User](ID),
	FeedID INT REFERENCES Feed(ID),
	PRIMARY KEY (UserID, FeedID)
);

CREATE TABLE UserFeedItem
(
	UserID INT REFERENCES [User](ID),
	FeedItemID INT REFERENCES FeedItem(ID),
	[Read] BIT NOT NULL DEFAULT 1,
	PRIMARY KEY (UserID, FeedItemID)
);

-- INDEXES
CREATE INDEX UserEmailIndex ON [User](Email);

-- TEST DATA
INSERT INTO Feed (Title, Description, Url) VALUES
('Vísir', 'asdasd', 'http://visir.is/section/FRONTPAGE&Template=rss&mime=xml'),
('BBC', 'Top news from BBC', 'http://feeds.bbci.co.uk/news/rss.xml'),
('Morgunblaðið', 'Morgunblaðið blablabla...', 'http://feeds.mbl.is/mm/rss/forsida.xml'),
('CNN', '....', 'http://rss.cnn.com/rss/edition.rss');