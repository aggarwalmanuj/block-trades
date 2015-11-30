using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Spider.Invest.Sys.BlockTrades.Facade.Data;
using Tweetinvi;

namespace Spider.Invest.Sys.BlockTrades.Facade
{
    public class BlockTradeTweetHarvester
    {
        private static  readonly BlockTradeTweetHarvester  _singleton = new BlockTradeTweetHarvester();
        private System.Timers.Timer _timer = new Timer(15*60*1000);
        private BlockTradeTweetHarvester()
        {
            TwitterCredentials.SetCredentials(ConfigurationSettings.AppSettings["twitter_access_token"],
                ConfigurationSettings.AppSettings["twitter_access_token_secret"],
                ConfigurationSettings.AppSettings["twitter_access_consumer_key"],
                ConfigurationSettings.AppSettings["twitter_access_consumer_key_secret"]);

            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true;
            _timer.Start();

            GetAndProcessTweets("BlockTradeAlert");
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Harvest();
        }

        public void Harvest()
        {
            GetAndProcessTweets("BlockTradeAlert");
        }


        private void GetAndProcessTweets(string username)
        {
            long? sinceId = new TweetProcessor().GetMaxTweetId();

            var param = Timeline.CreateUserTimelineRequestParameter(username);


            param.ExcludeReplies = true;
            param.IncludeEntities = false;
            param.MaximumNumberOfTweetsToRetrieve = 3200;
            param.TrimUser = true;
            if (sinceId.HasValue)
                param.SinceId = sinceId.Value;

            var tweets = Timeline.GetUserTimeline(param);
            ProcessTweets(tweets);

            if (null != tweets && tweets.Count() > 0)
            {
                var lastTweet = tweets.Last();
                var lastTweetId = lastTweet.Id;

                GetAndProcessTweets(username, lastTweetId, sinceId);
            }




        }

        private void GetAndProcessTweets(string username, long lastId, long? sinceId)
        {


            var param = Timeline.CreateUserTimelineRequestParameter(username);

            param.ExcludeReplies = true;
            param.IncludeEntities = false;
            param.MaximumNumberOfTweetsToRetrieve = 50000;
            param.TrimUser = true;
            param.MaxId = lastId - 1;
            if (sinceId.HasValue)
                param.SinceId = sinceId.Value;


            var tweets = Timeline.GetUserTimeline(param);
            ProcessTweets(tweets);

            if (null != tweets && tweets.Count() > 0)
            {
                var lastTweet = tweets.Last();
                var lastTweetId = lastTweet.Id;

                GetAndProcessTweets(username, lastTweetId, sinceId);
            }


        }

        private void ProcessTweets(IEnumerable<Tweetinvi.Core.Interfaces.ITweet> tweets)
        {
            if (null == tweets || tweets.Count() <= 0)
                return;

            TweetProcessor processor = new TweetProcessor();

            foreach (var tweet in tweets)
            {
                processor.ProcessTweet(tweet as Tweetinvi.Logic.Tweet);
            }
        }



        public static BlockTradeTweetHarvester Instance
        {
            get
            {
                return _singleton;
            }
        }


    }
}
