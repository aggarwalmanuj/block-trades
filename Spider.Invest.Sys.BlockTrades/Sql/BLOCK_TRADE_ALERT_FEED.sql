CREATE TABLE [dbo].[BLOCK_TRADE_ALERT_FEED] (
    [Id]               BIGINT        IDENTITY (1, 1) NOT NULL,
    [TwitterId]        BIGINT        NOT NULL,
    [TwitterIdStr]     NVARCHAR (50) NOT NULL,
    [DateCreated]      DATETIME      NOT NULL,
    [LocalDateCreated] DATETIME      NOT NULL,
    [DateTraded]       DATETIME      NOT NULL,
    [Symbol]           NVARCHAR (50) NOT NULL,
    [Size]             INT           NOT NULL,
    [Price]            MONEY         NOT NULL,
    [Amount]           MONEY         NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

CREATE UNIQUE INDEX [IX_UNQ_TWITTER_ID] ON [dbo].[BLOCK_TRADE_ALERT_FEED] ([TwitterId])

GO

CREATE UNIQUE INDEX [IX_UNQ_TWITTER_ID_STR] ON [dbo].[BLOCK_TRADE_ALERT_FEED] ([TwitterIdStr])
