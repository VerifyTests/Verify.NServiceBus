# <img src="/src/icon.png" height="30px"> Verify.NServiceBus

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://ci.appveyor.com/api/projects/status/haolkpausmys1ur4?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-NServiceBus)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.NServiceBus.svg)](https://www.nuget.org/packages/Verify.NServiceBus/)

Adds [Verify](https://github.com/VerifyTests/Verify) support to verify [NServiceBus Test Contexts](https://docs.particular.net/nservicebus/testing/).

**See [Milestones](../../milestones?state=closed) for release notes.**


## NuGet package

https://nuget.org/packages/Verify.NServiceBus/


## Usage

<!-- snippet: enable -->
<a id='snippet-enable'></a>
```cs
[ModuleInitializer]
public static void Initialize() =>
    VerifyNServiceBus.Initialize();
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L3-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-enable' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Verifying a context

Given the following handler:

<!-- snippet: SimpleHandler -->
<a id='snippet-simplehandler'></a>
```cs
public class MyHandler :
    IHandleMessages<MyRequest>
{
    public async Task Handle(MyRequest message, HandlerContext context)
    {
        await context.Publish(
            new MyPublishMessage
            {
                Property = "Value"
            });

        await context.Reply(
            new MyReplyMessage
            {
                Property = "Value"
            });

        var sendOptions = new SendOptions();
        sendOptions.DelayDeliveryWith(TimeSpan.FromHours(12));
        await context.Send(
            new MySendMessage
            {
                Property = "Value"
            },
            sendOptions);

        await context.ForwardCurrentMessageTo("newDestination");
    }
}
```
<sup><a href='/src/Tests/Snippets/MyHandler.cs#L1-L33' title='Snippet source file'>snippet source</a> | <a href='#snippet-simplehandler' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

The test that verifies the resulting context:

<!-- snippet: HandlerTest -->
<a id='snippet-handlertest'></a>
```cs
[Fact]
public async Task VerifyHandlerResult()
{
    var handler = new MyHandler();
    var context = new TestableMessageHandlerContext();

    var message = new MyRequest();
    await handler.Handle(message, context);

    await Verify(context);
}
```
<sup><a href='/src/Tests/Snippets/HandlerTests.cs#L4-L18' title='Snippet source file'>snippet source</a> | <a href='#snippet-handlertest' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

The resulting verification file is as follows:

<!-- snippet: HandlerTests.VerifyHandlerResult.verified.txt -->
<a id='snippet-HandlerTests.VerifyHandlerResult.verified.txt'></a>
```txt
{
  RepliedMessages: [
    {
      MyReplyMessage: {
        Property: Value
      }
    }
  ],
  ForwardedMessages: [
    newDestination
  ],
  SentMessages: [
    {
      MySendMessage: {
        Property: Value
      },
      Options: {
        DeliveryDelay: 12:00:00
      }
    }
  ],
  PublishedMessages: [
    {
      MyPublishMessage: {
        Property: Value
      }
    }
  ]
}
```
<sup><a href='/src/Tests/Snippets/HandlerTests.VerifyHandlerResult.verified.txt#L1-L29' title='Snippet source file'>snippet source</a> | <a href='#snippet-HandlerTests.VerifyHandlerResult.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Recording

Recording allows all message interaction with the test context to be captured and then verified.

Switch usages of `TestableMessageHandlerContext` to `RecordingHandlerContext`.

<!-- snippet: RecordingHandlerTests -->
<a id='snippet-recordinghandlertests'></a>
```cs
[Fact]
public async Task VerifyHandlerResult()
{
    Recording.Start();
    var handler = new MyHandler();
    var context = new RecordingHandlerContext();

    var message = new MyRequest();
    await handler.Handle(message, context);

    await Verify("some other data");
}
```
<sup><a href='/src/Tests/Snippets/RecordingHandlerTests.cs#L4-L19' title='Snippet source file'>snippet source</a> | <a href='#snippet-recordinghandlertests' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

The resulting context verification file is as follows:

<!-- snippet: RecordingHandlerTests.VerifyHandlerResult.verified.txt -->
<a id='snippet-RecordingHandlerTests.VerifyHandlerResult.verified.txt'></a>
```txt
{
  target: some other data,
  messages: [
    {
      Publish: {
        MyPublishMessage: {
          Property: Value
        }
      }
    },
    {
      Reply: {
        MyReplyMessage: {
          Property: Value
        }
      }
    },
    {
      Send: {
        MySendMessage: {
          Property: Value
        },
        Options: {
          DeliveryDelay: 12:00:00
        }
      }
    }
  ]
}
```
<sup><a href='/src/Tests/Snippets/RecordingHandlerTests.VerifyHandlerResult.verified.txt#L1-L29' title='Snippet source file'>snippet source</a> | <a href='#snippet-RecordingHandlerTests.VerifyHandlerResult.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Verifying a Saga

Given the following handler:

<!-- snippet: SimpleSaga -->
<a id='snippet-simplesaga'></a>
```cs
public class MySaga :
    NServiceBus.Saga<MySaga.MySagaData>,
    IHandleMessages<MyRequest>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper) =>
        mapper.ConfigureMapping<MyRequest>(message => message.OrderId)
            .ToSaga(sagaData => sagaData.OrderId);

    public async Task Handle(MyRequest message, HandlerContext context)
    {
        await context.Publish(
            new MyPublishMessage
            {
                Property = "Value"
            });

        Data.MessageCount++;
    }

    public class MySagaData :
        ContainSagaData
    {
        public Guid OrderId { get; set; }
        public int MessageCount { get; set; }
    }
}
```
<sup><a href='/src/Tests/Snippets/MySaga.cs#L1-L30' title='Snippet source file'>snippet source</a> | <a href='#snippet-simplesaga' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

The test that verifies the resulting context:

<!-- snippet: SagaTest -->
<a id='snippet-sagatest'></a>
```cs
[Fact]
public async Task VerifySagaResult()
{
    var saga = new MySaga
    {
        Data = new()
    };

    var context = new TestableMessageHandlerContext();

    var message = new MyRequest();
    await saga.Handle(message, context);

    await Verify(new
    {
        context,
        saga
    });
}
```
<sup><a href='/src/Tests/Snippets/SagaTests.cs#L4-L26' title='Snippet source file'>snippet source</a> | <a href='#snippet-sagatest' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

The resulting verification file is as follows:

<!-- snippet: SagaTests.VerifySagaResult.verified.txt -->
<a id='snippet-SagaTests.VerifySagaResult.verified.txt'></a>
```txt
{
  context: {
    PublishedMessages: [
      {
        MyPublishMessage: {
          Property: Value
        }
      }
    ]
  },
  saga: {
    MessageCount: 1
  }
}
```
<sup><a href='/src/Tests/Snippets/SagaTests.VerifySagaResult.verified.txt#L1-L14' title='Snippet source file'>snippet source</a> | <a href='#snippet-SagaTests.VerifySagaResult.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Example behavior change

The next time there is a code change, that results in a different resulting interactions with NServiceBus, those changes can be visualized. For example if the `DelayDeliveryWith` is changed from 12 hours to 1 day:

<!-- snippet: SimpleHandlerV2 -->
<a id='snippet-simplehandlerv2'></a>
```cs
await context.Publish(
    new MyPublishMessage
    {
        Property = "Value"
    });

await context.Reply(
    new MyReplyMessage
    {
        Property = "Value"
    });

var sendOptions = new SendOptions();
sendOptions.DelayDeliveryWith(TimeSpan.FromDays(1));
await context.Send(
    new MySendMessage
    {
        Property = "Value"
    },
    sendOptions);

await context.ForwardCurrentMessageTo("newDestination");
```
<sup><a href='/src/Tests/Snippets/MyHandlerV2.cs#L6-L31' title='Snippet source file'>snippet source</a> | <a href='#snippet-simplehandlerv2' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Then the resulting visualization diff would look as follows:

![visualization diff](/src/approvaltests-diff.png)


### Message to Handler mapping

`MessageToHandlerMap` allows verification of message that do not have a handler.

For example:

<!-- snippet: MessageToHandlerMap -->
<a id='snippet-messagetohandlermap'></a>
```cs
var map = new MessageToHandlerMap();
map.AddMessagesFromAssembly<MyMessage>();
map.AddHandlersFromAssembly<MyHandler>();
await Verify(map);
```
<sup><a href='/src/Tests/MessageToHandlerMapTests.cs#L7-L12' title='Snippet source file'>snippet source</a> | <a href='#snippet-messagetohandlermap' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Would result in:

<!-- snippet: MessageToHandlerMapTests.Integration.verified.txt -->
<a id='snippet-MessageToHandlerMapTests.Integration.verified.txt'></a>
```txt
{
  MessagesWithNoHandler: [
    MessageToHandlerMapTests.MessageWithNoHandler
  ]
}
```
<sup><a href='/src/Tests/MessageToHandlerMapTests.Integration.verified.txt#L1-L5' title='Snippet source file'>snippet source</a> | <a href='#snippet-MessageToHandlerMapTests.Integration.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->



## Icon

[Approval](https://thenounproject.com/term/approval/1759519/) designed by [Mike Zuidgeest](https://thenounproject.com/zuidgeest/) from [The Noun Project](https://thenounproject.com/).
