# Google Event Receiver

A prototype of receiving events from Google real-time developer notifications

## Description

This is a .NET Core Web API that will receive events from [Google](https://developer.android.com/google/play/billing/getting-ready#configure-rtdn) and place them in a DynamoDb table.

## Requirements

.NET Core
Docker
An AWS account
A Google account

## Setup

I've built the solution to run on AWS in a Docker container in ECS. The following steps are how I got this running.

1. Build Docker
2. Upload to ECR
3. Configure ECS
4. Configure DynamoDb
5. Configure Google notifications

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
