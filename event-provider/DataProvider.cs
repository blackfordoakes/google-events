using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Events.Provider.Interfaces;
using Events.Provider.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Events.Provider
{
    internal class DataProvider : IDataProvider
    {
        private Table _dynamo;

        public DataProvider(IAmazonDynamoDB dynamo, IOptions<DatabaseSettings> options)
        {
            _initializeDynamoDb(dynamo, options.Value);
        }

        public async Task<List<SubscriptionChangeEvent>> GetEvents()
        {
            var events = new List<SubscriptionChangeEvent>();

            var filter = new ScanFilter();
            var search = _dynamo.Scan(filter);
            do
            {
                var documents = await search.GetNextSetAsync();
                foreach (var doc in documents)
                {
                    var evt = JsonConvert.DeserializeObject<SubscriptionChangeEvent>(doc["Data"]);
                    events.Add(evt);
                }
            } while (!search.IsDone);

            return events;
        }

        public async Task WriteEvent(SubscriptionChangeEvent change)
        {
            Document dbRecord = new Document();
            dbRecord["Id"] = Guid.NewGuid();
            dbRecord["Data"] = JsonConvert.SerializeObject(change);
            dbRecord["EventTime"] = DateTime.Now;

            await _dynamo.PutItemAsync(dbRecord);
        }

        private void _initializeDynamoDb(IAmazonDynamoDB dynamo, DatabaseSettings settings)
        {
            if (!Table.TryLoadTable(dynamo, settings.DynamoTableName, out _dynamo))
            {
                // if the table does not exist, create it on the fly
                var request = new CreateTableRequest
                {
                    AttributeDefinitions = new List<AttributeDefinition>()
                    {
                        new AttributeDefinition
                        {
                            AttributeName = "Id",
                            AttributeType = "S"
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "Id",
                            KeyType = "HASH" // partition key
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    },
                    TableName = settings.DynamoTableName
                };

                dynamo.CreateTableAsync(request);

                // load it after it's ready
                _waitUntilTableReady(dynamo, settings.DynamoTableName);
                _dynamo = Table.LoadTable(dynamo, settings.DynamoTableName);
            }
        }

        private void _waitUntilTableReady(IAmazonDynamoDB dynamo, string tableName)
        {
            string status = null;
            do
            {
                try
                {
                    var res = dynamo.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = tableName
                    }).Result;

                    status = res.Table.TableStatus;
                }
                catch (ResourceNotFoundException)
                {
                    // DescribeTable is eventually consistent. So you might
                    // get resource not found. We handle the potential exception.
                    System.Threading.Thread.Sleep(5000); // Wait 5 seconds.
                }
            } while (status != "ACTIVE");
        }
    }
}
