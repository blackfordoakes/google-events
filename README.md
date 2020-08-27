# Google Event Receiver

A prototype of receiving events from Google real-time developer notifications

## Description

There are two parts of this project:  A .NET Core Web API that can read/write messages to a DynamoDb and a Lambda function that takes an Google Real-Time event and writes it to DynamoDb. The Web APIs mainly for testing and the Lambda is used to actually integrate with Google Developer Events. The rest of this README will go into detail about the Lambda.

## Requirements

.NET Core
An AWS account
A Google account

## AWS Setup

To set up the AWS portion, first you must deploy the Lambda function.

```dotnet lambda deploy-function GoogleSubscription```

I have added a role `myBasicExecutionRole` that allows Lambda to access to DynamoDb. If you have a different role, change the name in aws-lambda-tools-defaults.json.

Next, we have to set up API Gateway to serve our Lambda. Go to "API Gateway" in the AWS Console and click Create API. Select a public "REST API" and click Build. Use most of the defaults (REST protocol, new API) and give your API a name. 

Under "Actions", select "Create Resourse" and call this resource "event" while enabling CORS.

Under the event resource, select Actions -> Create Method for POST. Select the integration type as Lambda. Make sure Use Lambda Proxy Integration is OFF. Enter the name of your Lambda function and hit save. When AWS asks you to grant permissions to execute your Lambda, allow it.


## Google Setup

The following steps are found [here](https://developer.android.com/google/play/billing/getting-ready#configure-rtdn), but reproduced for quick access.

Go to the Pub/Sub section of the [Google Console](https://console.cloud.google.com/cloudpubsub/topic/list). Click "Create Topic". Give your topic a name and click create. Scroll down and click on "Create Subscription". Enter a subscription name and set the type as "Push". Enter the URL of your AWS Resource as the Endpoint URL. Leave everything else as default and click Create.

Grant Google access to publish to your topic by going to the permissions on your topic and adding the service account google-play-developer-notifications@system.gserviceaccount.com, and grant it the role of Pub/Sub Publisher.

Then, enable real-time notifications as specified in the documentation.

## Contributing Guidelines

Thanks for taking the time to contribute! If you want it to do something different than it does, feel free to fork this repo. Just let people know you were inspired by me and we're all good. :)

## Issues

Ensure the bug was not already reported by searching on GitHub under issues. If you're unable to find an open issue addressing the bug, open a new issue.

Please pay attention to the following points while opening an issue.

### Write detailed information

Detailed information is very helpful to understand an issue.

For example:

* How to reproduce the issue, step-by-step.
* The expected behavior (or what is wrong).

## Pull Requests

Pull Requests are always welcome.

Ensure the PR description clearly describes the problem and solution. It should also include the relevant issue number (if applicable).

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

MIT © [blakfordoakes]()
