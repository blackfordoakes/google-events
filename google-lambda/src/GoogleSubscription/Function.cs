using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GoogleSubscription
{
    public class Function
    {
        private const string TABLE_NAME = "DeveloperEvents";

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(GoogleMessage input, ILambdaContext context)
        {
            LambdaLogger.Log($"Calling function name: {context.FunctionName}\\n");

            string interim = JsonConvert.SerializeObject(input);
            //LambdaLogger.Log($"Got: {interim}");

            var eventJson = input.GetMessage();
            //LambdaLogger.Log(eventJson);

            var eventData = SubscriptionChangeEvent.Parse(eventJson);
            LambdaLogger.Log($"Got purchase token: {eventData.SubscriptionNotification.PurchaseToken}");

            var dynamoDbClient = new AmazonDynamoDBClient();
            var table = _initializeDynamoDb(dynamoDbClient);

            /* GET implementation
            var events = await _getEvents(table);
            return events;
            */

            await _writeEvent(table, eventData);

            return $"Purchase token '{eventData.SubscriptionNotification.PurchaseToken}' written to DynamoDb.";
        }

        private async Task<List<SubscriptionChangeEvent>> _getEvents(Table dynamo)
        {
            var events = new List<SubscriptionChangeEvent>();

            var filter = new ScanFilter();
            var search = dynamo.Scan(filter);
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

        private async Task _writeEvent(Table dynamo, SubscriptionChangeEvent change)
        {
            Document dbRecord = new Document();
            dbRecord["Id"] = Guid.NewGuid();
            dbRecord["Data"] = JsonConvert.SerializeObject(change);
            dbRecord["EventTime"] = DateTime.Now;

            await dynamo.PutItemAsync(dbRecord);
        }

        private Table _initializeDynamoDb(IAmazonDynamoDB dynamo)
        {
            Table dynamoTable;

            if (!Table.TryLoadTable(dynamo, TABLE_NAME, out dynamoTable))
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
                    TableName = TABLE_NAME
                };

                dynamo.CreateTableAsync(request);

                // load it after it's ready
                _waitUntilTableReady(dynamo, TABLE_NAME);
                dynamoTable = Table.LoadTable(dynamo, TABLE_NAME);
            }

            return dynamoTable;
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
