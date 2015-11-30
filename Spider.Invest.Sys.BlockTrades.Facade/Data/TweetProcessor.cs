using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Spider.Invest.Sys.BlockTrades.Facade.Entities;
using Tweetinvi.Logic;

namespace Spider.Invest.Sys.BlockTrades.Facade.Data
{
    public class TweetProcessor
    {
        private Regex _blockTradeTweetRegex =
            new Regex(
                @".*?:\s\$(?<SYMBOL>.?.*)\s(?<QTY>[0-9]+(,[0-9]+)*)\sshares\s@\s(?<PRICE>\$(?=.*\d)\d{0,6}(\.\d{1,5})?)\s\[(?<TIME>.*?)\]", RegexOptions.Compiled);
      
       

        public void ProcessTweet(Tweet tweet)
        {
            if (null == tweet)
                return;

            var id = tweet.Id;
            var idStr = tweet.IdStr;
            var dateCreated = tweet.CreatedAt;
            var localDateCreated = tweet.TweetLocalCreationDate;
            var rawString = tweet.Text.Trim();

            bool isParsed = false;
            if (_blockTradeTweetRegex.IsMatch(rawString))
            {
                var match = _blockTradeTweetRegex.Match(rawString);
                var groups = match.Groups;
                if (null != groups && groups.Count > 0)
                {
                    var symbol = groups["SYMBOL"].ToString();
                    var qty = Int64.Parse(groups["QTY"].ToString(), NumberStyles.Any);
                    var price = Decimal.Parse(groups["PRICE"].ToString(), NumberStyles.Any);
                    var tradeTime = Convert.ToDateTime(dateCreated.ToShortDateString() + " " + groups["TIME"].ToString());

                    Upsert(id, idStr, dateCreated, localDateCreated, tradeTime, symbol, qty, price,
                        Convert.ToDecimal((Convert.ToDecimal(qty)*price)));
                    isParsed = true;
                }
               

            }


            if (!isParsed)
            {
                throw new ApplicationException("The tweet does not match the regular expression");
            }
          
           

           
        }



        public IEnumerable<BlockTradeSummary> GetBlockTradeSummariesByDate(string symbol, DateTime fromDate, DateTime toDate)
        {
            List<BlockTradeSummary> returnValue = new List<BlockTradeSummary>();

            string sqlStatement = @"SET NOCOUNT ON
                                        BEGIN

	                                        SELECT  Sum(Amount)/Sum(Size) as Price, 
                                                    Sum(size) as TotalSize, 
                                                    Sum(amount) as TotalAmount, 
                                                    Count(Price) as TotalTrades,
                                                    DATEFROMPARTS (YEAR(DateTraded), Month(DateTraded), Day(DateTraded)) as TradeDate
                                            FROM    [dbo].[BLOCK_TRADE_ALERT_FEED]
                                            WHERE   Symbol = @Symbol
                                            AND     DateTraded >= @DateFrom
                                            AND     DateTraded <= @DateTo
                                            GROUP BY DATEFROMPARTS (YEAR(DateTraded), Month(DateTraded), Day(DateTraded))
                                            ORDER BY TradeDate DESC

                                        END";



            SqlCommand cmd = new SqlCommand(sqlStatement);
            cmd.Parameters.Add("Symbol", symbol);
            cmd.Parameters.Add("DateFrom", fromDate);
            cmd.Parameters.Add("DateTo", toDate);


            using (
                var conn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["lakSysBlockTradesFeed"].ConnectionString))
            {
                conn.Open();

                cmd.Connection = conn;

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            returnValue.Add(new BlockTradeSummary()
                            {
                                Price = Convert.ToDecimal(reader[0]),
                                TotalShares = Convert.ToInt64(reader[1]),
                                TotalAmount = Convert.ToDecimal(reader[2]),
                                TotalTrades = Convert.ToInt32(reader[3]),
                                TradeDate = Convert.ToDateTime(reader[4])
                            });
                        }
                    }
                }
            }




            return returnValue;
        }

        public IEnumerable<BlockTradeSummary> GetBlockTradeSummaries(string symbol, DateTime fromDate, DateTime toDate)
        {
            List<BlockTradeSummary> returnValue = new List<BlockTradeSummary>();

            string sqlStatement = @"SET NOCOUNT ON
                                        BEGIN

	                                        SELECT  Price, 
                                                    Sum(size) as TotalSize, 
                                                    Sum(amount) as TotalAmount, 
                                                    Count(Price) as TotalTrades
                                            FROM    [dbo].[BLOCK_TRADE_ALERT_FEED]
                                            WHERE   Symbol = @Symbol
                                            AND     DateTraded >= @DateFrom
                                            AND     DateTraded <= @DateTo
                                            GROUP BY Price
                                            ORDER BY TotalAmount DESC

                                        END";

            SqlCommand cmd = new SqlCommand(sqlStatement);
            cmd.Parameters.Add("Symbol", symbol);
            cmd.Parameters.Add("DateFrom", fromDate);
            cmd.Parameters.Add("DateTo", toDate);


            using (
                var conn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["lakSysBlockTradesFeed"].ConnectionString))
            {
                conn.Open();

                cmd.Connection = conn;

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            returnValue.Add(new BlockTradeSummary()
                            {
                                Price = Convert.ToDecimal( reader[0]),
                                TotalShares = Convert.ToInt64( reader[1]),
                                TotalAmount = Convert.ToDecimal(reader[2]),
                                TotalTrades = Convert.ToInt32(reader[3])
                            });
                        }
                    }
                }
            }




            return returnValue;
        }

        public long? GetMaxTweetId()
        {
            string sqlStatement = @"SELECT MAX(TwitterId) FROM [dbo].[BLOCK_TRADE_ALERT_FEED] ";

            SqlCommand cmd = new SqlCommand(sqlStatement);

            long? returnValue = null;

            using (
                var conn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["lakSysBlockTradesFeed"].ConnectionString))
            {
                conn.Open();

                cmd.Connection = conn;

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            returnValue = reader.GetInt64(0);
                        }
                    }
                }
            }


            return returnValue;
        }

        private void Upsert(long id, string idStr, DateTime dateCreated, DateTime localDateCreated, DateTime tradeTime, string symbol, long qty, decimal price, decimal amount)
        {
            string sqlStatement = @"SET NOCOUNT ON
                                        BEGIN

	                                        UPDATE [dbo].[BLOCK_TRADE_ALERT_FEED]
	                                        SET DateCreated = @DateCreated,
	                                        LocalDateCreated = @LocalDateCreated ,
	                                        DateTraded = @DateTraded ,
	                                        Symbol = @Symbol,
	                                        Size = @Size ,
	                                        Price = @Price ,
	                                        Amount = @Amount
	                                        WHERE TwitterIdStr = @TwitterIdStr

	                                        IF (@@ROWCOUNT = 0 )
	                                        BEGIN
		                                        INSERT INTO [dbo].[BLOCK_TRADE_ALERT_FEED] (TwitterId, TwitterIdStr, DateCreated, LocalDateCreated, DateTraded, Symbol, Size, Price, Amount  )
		                                        VALUES (@TwitterId, @TwitterIdStr, @DateCreated, @LocalDateCreated, @DateTraded, @Symbol, @Size, @Price, @Amount  )
	                                        END
                                        END";

            SqlCommand cmd = new SqlCommand(sqlStatement);
            cmd.Parameters.Add("TwitterId", id);
            cmd.Parameters.Add("TwitterIdStr", idStr);
            cmd.Parameters.Add("DateCreated", dateCreated);
            cmd.Parameters.Add("LocalDateCreated", localDateCreated);
            cmd.Parameters.Add("DateTraded", tradeTime);
            cmd.Parameters.Add("Symbol", symbol);
            cmd.Parameters.Add("Size", qty);
            cmd.Parameters.Add("Price", price);
            cmd.Parameters.Add("Amount", amount);

            using (
                var conn =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["lakSysBlockTradesFeed"].ConnectionString))
            {
                conn.Open();

                cmd.Connection = conn;

                cmd.ExecuteNonQuery();
            }





        }
    }
}
