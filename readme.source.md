# <img src="/src/icon.png" height="30px"> Verify.NServiceBus

[![Build status](https://ci.appveyor.com/api/projects/status/wwrri8srggv1h56j/branch/master?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-NServiceBus)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.NServiceBus.svg)](https://www.nuget.org/packages/Verify.NServiceBus/)

Adds [Verify](https://github.com/SimonCropp/Verify) support to verify [NServiceBus Test Contexts](https://docs.particular.net/nservicebus/samples/unit-testing/).

<!--- StartOpenCollectiveBackers -->

[Already a Patron? skip past this section](#endofbacking)


## Community backed

**It is expected that all developers [become a Patron](https://opencollective.com/nservicebusextensions/contribute/patron-6976) to use this tool. [Go to licensing FAQ](https://github.com/NServiceBusExtensions/Home/#licensingpatron-faq)**

Thanks to the current backers.

<img src="https://opencollective.com/nservicebusextensions/tiers/patron.svg?width=890&avatarHeight=60&button=false">

<a href="#" id="endofbacking"></a>

<!--- EndOpenCollectiveBackers -->

toc


## NuGet package

https://nuget.org/packages/Verify.NServiceBus/


## Usage

Before any test have run call:

```
VerifyNServiceBus.Enable();
```


### Verifying a context

Given the following handler:

snippet: SimpleHandler

The test that verifies the resulting context:

snippet: HandlerTest

The resulting context verification file is as follows:

snippet: MessageHandlerTests.VerifyHandlerResult.verified.txt


### Example behavior change

The next time there is a code change, that results in a different resulting interactions with NServiceBus, those changes can be visualized. For example if the `DelayDeliveryWith` is changed from 12 hours to 1 day:

snippet: SimpleHandlerV2

Then the resulting visualization diff would look as follows:


![visualization diff](/src/approvaltests-diff.png)


## Release Notes

See [closed milestones](../../milestones?state=closed).


## Icon

[Approval](https://thenounproject.com/term/approval/1759519/) designed by [Mike Zuidgeest](https://thenounproject.com/zuidgeest/) from [The Noun Project](https://thenounproject.com/).