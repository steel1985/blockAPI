﻿using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MongoDB.Bson.IO;

namespace NEO_Block_API.lib
{
    public class mongoHelper
    {
        public string mongodbConnStr_testnet = string.Empty;
        public string mongodbDatabase_testnet = string.Empty;
        public string neoCliJsonRPCUrl_testnet = string.Empty;
        public string neoCliJsonRPCUrl_local_testnet = string.Empty;

        public string mongodbConnStr_mainnet = string.Empty;
        public string mongodbDatabase_mainnet = string.Empty;
        public string neoCliJsonRPCUrl_mainnet = string.Empty;
        public string neoCliJsonRPCUrl_local_mainnet = string.Empty;   //本地节点服务
        public string neoRpcUrl_mainnet = string.Empty;  //第三方RPC服务

        public string mongodbConnStr_privatenet = string.Empty;
        public string mongodbDatabase_privatenet = string.Empty;
        public string neoCliJsonRPCUrl_privatenet = string.Empty;

        public string mongodbConnStr_pri = string.Empty;
        public string mongodbDatabase_pri = string.Empty;
        public string neoCliJsonRPCUrl_pri = string.Empty;
        public string neoCliJsonRPCUrl_local_pri = string.Empty;

        public string mongodbConnStr_swnet = string.Empty;
        public string mongodbDatabase_swnet = string.Empty;
        public string neoCliJsonRPCUrl_swnet = string.Empty;

        public string mongodbConnStr_NeonOnline = string.Empty;
        public string mongodbDatabase_NeonOnline = string.Empty;

        public mongoHelper() {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection()    //将配置文件的数据加载到内存中
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())   //指定配置文件所在的目录
                .AddJsonFile("mongodbsettings.json", optional: true, reloadOnChange: false)  //指定加载的配置文件
                .Build();    //编译成对象  
            mongodbConnStr_testnet = config["mongodbConnStr_testnet"];
            mongodbDatabase_testnet = config["mongodbDatabase_testnet"];
            neoCliJsonRPCUrl_testnet = config["neoCliJsonRPCUrl_testnet"];
            neoCliJsonRPCUrl_local_testnet = config["neoCliJsonRPCUrl_local_testnet"];

            mongodbConnStr_mainnet = config["mongodbConnStr_mainnet"];
            mongodbDatabase_mainnet = config["mongodbDatabase_mainnet"];
            neoCliJsonRPCUrl_mainnet = config["neoCliJsonRPCUrl_mainnet"];
            neoCliJsonRPCUrl_local_mainnet = config["neoCliJsonRPCUrl_local_mainnet"];
            neoRpcUrl_mainnet = config["neoRpcUrl_mainnet"];
                
            mongodbConnStr_privatenet = config["mongodbConnStr_privatenet"];
            mongodbDatabase_privatenet = config["mongodbDatabase_privatenet"];
            neoCliJsonRPCUrl_privatenet = config["neoCliJsonRPCUrl_privatenet"];

            mongodbConnStr_pri = config["mongodbConnStr_pri"];
            mongodbDatabase_pri = config["mongodbDatabase_pri"];
            neoCliJsonRPCUrl_pri = config["neoCliJsonRPCUrl_pri"];
            neoCliJsonRPCUrl_local_pri = config["neoCliJsonRPCUrl_local_pri"];

            mongodbConnStr_swnet = config["mongodbConnStr_swnet"];
            mongodbDatabase_swnet = config["mongodbDatabase_swnet"];
            neoCliJsonRPCUrl_swnet = config["neoCliJsonRPCUrl_swnet"];

            mongodbConnStr_NeonOnline = config["mongodbConnStr_NeonOnline"];
            mongodbDatabase_NeonOnline = config["mongodbDatabase_NeonOnline"];
        }

        public JArray GetData(string mongodbConnStr,string mongodbDatabase, string coll,string findFliter)
        {
            var client = new MongoClient(mongodbConnStr);
            var database = client.GetDatabase(mongodbDatabase);
            var collection = database.GetCollection<BsonDocument>(coll);

            List<BsonDocument> query = collection.Find(BsonDocument.Parse(findFliter)).ToList();
            client = null;

            if (query.Count > 0)
            {
                var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
                JArray JA = JArray.Parse(query.ToJson(jsonWriterSettings));
                foreach (JObject j in JA)
                {
                    j.Remove("_id");
                }
                return JA;
            }
            else { return new JArray(); }      
        }

        public JArray GetDistinctData(string mongodbConnStr, string mongodbDatabase, string coll, string findFliter)
        {
            var client = new MongoClient(mongodbConnStr);
            var database = client.GetDatabase(mongodbDatabase);
            var collection = database.GetCollection<BsonDocument>(coll);

            List<BsonDocument> query = collection.Find(BsonDocument.Parse(findFliter)).ToList();
            client = null;

            if (query.Count > 0)
            {
                var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
                JArray JA = JArray.Parse(query.ToJson(jsonWriterSettings));
                foreach (JObject j in JA)
                {
                    j.Remove("_id");
                }
                return JA;
            }
            else { return new JArray(); }
        }

        public JArray GetDataPages(string mongodbConnStr, string mongodbDatabase, string coll,string sortStr, int pageCount, int pageNum, string findBson = "{}")
        {
            var client = new MongoClient(mongodbConnStr);
            var database = client.GetDatabase(mongodbDatabase);
            var collection = database.GetCollection<BsonDocument>(coll);

            List<BsonDocument> query = collection.Find(BsonDocument.Parse(findBson)).Sort(sortStr).Skip(pageCount * (pageNum-1)).Limit(pageCount).ToList();
            client = null;

            if (query.Count > 0)
            {

                var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
                JArray JA = JArray.Parse(query.ToJson(jsonWriterSettings));
                foreach (JObject j in JA)
                {
                    j.Remove("_id");
                }
                return JA;
            }
            else { return new JArray(); }
        }

        public long GetDataCount(string mongodbConnStr, string mongodbDatabase,string coll)
        {
            var client = new MongoClient(mongodbConnStr);
            var database = client.GetDatabase(mongodbDatabase);
            var collection = database.GetCollection<BsonDocument>(coll);

            var txCount = collection.Find(new BsonDocument()).Count();

            client = null;

            return txCount;
        }

        public long GetDataCount(string mongodbConnStr, string mongodbDatabase, string coll, string findBson)
        {
            var client = new MongoClient(mongodbConnStr);
            var database = client.GetDatabase(mongodbDatabase);
            var collection = database.GetCollection<BsonDocument>(coll);

            var txCount = collection.Find(BsonDocument.Parse(findBson)).Count();

            client = null;

            return txCount;
        }

        public JArray Getdatablockheight(string mongodbConnStr, string mongodbDatabase)
        {
            int blockDataHeight = -1;
            int txDataHeight = -1;
            int utxoDataHeight = -1;
            int notifyDataHeight = -1;
            int totalsysfeeDataHeight = -1;
            int NEP5DataHeight = -1;
            int fulllogDataHeight = -1;

            var client = new MongoClient(mongodbConnStr);
            var database = client.GetDatabase(mongodbDatabase);

            //var collection = database.GetCollection<BsonDocument>("block");
            //var sortBson = BsonDocument.Parse("{index:-1}");
            //var query = collection.Find(new BsonDocument()).Sort(sortBson).Limit(1).ToList();
            //if (query.Count > 0)
            //{blockDataHeight = (int)query[0]["index"];}

            //collection = database.GetCollection<BsonDocument>("tx");
            //sortBson = BsonDocument.Parse("{blockindex:-1}");
            //query = collection.Find(new BsonDocument()).Sort(sortBson).Limit(1).ToList();
            //if (query.Count > 0)
            //{ txDataHeight = (int)query[0]["blockindex"]; }

            var collection = database.GetCollection<BsonDocument>("system_counter");
            var query = collection.Find(new BsonDocument()).ToList();
            if (query.Count > 0)
            {
                foreach (var q in query)
                {
                    if ((string)q["counter"] == "block") { blockDataHeight = (int)q["lastBlockindex"]; txDataHeight = blockDataHeight; };
                    if ((string)q["counter"] == "utxo") { utxoDataHeight = (int)q["lastBlockindex"]; };
                    if ((string)q["counter"] == "notify") { notifyDataHeight = (int)q["lastBlockindex"]; };
                    if ((string)q["counter"] == "totalsysfee") { totalsysfeeDataHeight = (int)q["lastBlockindex"]; };
                    if ((string)q["counter"] == "NEP5") { NEP5DataHeight = (int)q["lastBlockindex"]; };
                    if ((string)q["counter"] == "fulllog") { fulllogDataHeight = (int)q["lastBlockindex"]; };
                }
            }

            client = null;

            JObject J = new JObject
            {
                { "blockDataHeight", blockDataHeight },
                { "txDataHeight", txDataHeight },
                { "utxoDataHeight", utxoDataHeight },
                { "notifyDataHeight", notifyDataHeight },
                { "totalsysfee", totalsysfeeDataHeight },
                { "NEP5", NEP5DataHeight },
                { "fulllogDataHeight", fulllogDataHeight }
            };
            JArray JA = new JArray
            {
                J
            };

            return JA;
        }

        public Boolean InsertOneDataByCheckKey(string mongodbConnStr, string mongodbDatabase, string coll, JObject Jdata,string key,string value)
        {
            Boolean flag = false;
            var client = new MongoClient(mongodbConnStr);
            var database = client.GetDatabase(mongodbDatabase);
            var collection = database.GetCollection<BsonDocument>(coll);

            var query = collection.Find("{'" + key + "':'" + value +"'}").ToList();
            if (query.Count == 0) {
                string strData = Newtonsoft.Json.JsonConvert.SerializeObject(Jdata);
                BsonDocument bson = BsonDocument.Parse(strData);
                bson.Add("addTime", DateTime.Now);
                bson.Add("updateTime", "");
                collection.InsertOne(bson);
                flag = true;
            }

            client = null;
            return flag;
        }

        public decimal GetTotalSysFeeByBlock(string mongodbConnStr, string mongodbDatabase, int blockindex)
        {
            var client = new MongoClient(mongodbConnStr);
            var database = client.GetDatabase(mongodbDatabase);
            var collection = database.GetCollection<BsonDocument>("block_sysfee");

            decimal totalSysFee = 0;
            var query = collection.Find("{'index':" + blockindex + "}").ToList();
            if (query.Count > 0)
            {
                totalSysFee = decimal.Parse(query[0]["totalSysfee"].AsString);
            }
            else {
                totalSysFee = -1;
            }

            client = null;

            return totalSysFee;
        }

        public Boolean deleteByKey(string mongodbConnStr, string mongodbDatabase, string coll, string key, string value)
        {
            Boolean flag = false;
            var client = new MongoClient(mongodbConnStr);
            var database = client.GetDatabase(mongodbDatabase);
            var collection = database.GetCollection<BsonDocument>(coll);
            DeleteResult result=collection.DeleteOne("{'" + key + "':'" + value + "'}");
            if (result.DeletedCount>0) {
                flag = true;
            }
            client = null;
            return flag;
        }

        public Boolean updateDataByKey(string mongodbConnStr, string mongodbDatabase, string coll, JObject Jdata, string key, string value)
        {
            Boolean flag = false;
            var client = new MongoClient(mongodbConnStr);
            var database = client.GetDatabase(mongodbDatabase);
            var collection = database.GetCollection<BsonDocument>(coll);

            string strData = Newtonsoft.Json.JsonConvert.SerializeObject(Jdata);
            BsonDocument bson = BsonDocument.Parse(strData);
            //bson.Add("updateTime", DateTime.Now);

            UpdateResult result = collection.UpdateOne("{'" + key + "':'" + value + "'}", "{$set:"+bson+"}");
            if (result.ModifiedCount > 0 || result.MatchedCount > 0)
            {
                flag = true;
            }

            client = null;
            return flag;
        }

        public void insertOne(string mongodbConnStr, string mongodbDatabase, string collName, JObject J, bool isAsyn = false)
        {
            var client = new MongoClient(mongodbConnStr);
            var database = client.GetDatabase(mongodbDatabase);
            var collection = database.GetCollection<BsonDocument>(collName);

            var document = BsonDocument.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(J));

            if (isAsyn)
            {
                collection.InsertOneAsync(document);
            }
            else
            {
                collection.InsertOne(document);
            }

            client = null;
        }
    }
}
